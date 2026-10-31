// oled-host: SDL2 window emulating the 128x64 SSD1306 OLED. Runs a command (ueforth)
// as a child: keyboard -> child's stdin, ESC "OLED" + 1024-byte frames <- child's stdout.
// Other child output goes to stderr. Test hooks (env): OLED_TYPE (text typed after start,
// \n = Enter), OLED_QUIT_MS (exit after N ms), OLED_DUMP=1 (print last frame as ASCII),
// OLED_DUMP_FILE=path (write last raw 1024-byte frame).
#include <SDL2/SDL.h>
#include <poll.h>
#include <signal.h>
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <sys/wait.h>
#include <unistd.h>

#define W 128
#define H 64
#define FRAME 1024
static const unsigned char MARK[5] = {27, 'O', 'L', 'E', 'D'};

static unsigned char frame[FRAME];
static int have_frame, fp, mp, in_frame;

static int lit(int x, int y) { return (frame[(y >> 3) * W + x] >> (y & 7)) & 1; }

static void parse(const unsigned char *b, int n, int *dirty) {
  for (int i = 0; i < n; i++) {
    unsigned char c = b[i];
    if (in_frame) {
      frame[fp++] = c;
      if (fp == FRAME) { in_frame = 0; have_frame = 1; *dirty = 1; }
    } else if (c == MARK[mp]) {
      if (++mp == 5) { mp = 0; in_frame = 1; fp = 0; }
    } else {
      if (mp) fwrite(MARK, 1, mp, stderr);
      mp = (c == MARK[0]);
      if (!mp) fputc(c, stderr);
    }
  }
}

int main(int argc, char **argv) {
  if (argc < 2) { fprintf(stderr, "usage: oled-host CMD [ARGS...]\n"); return 2; }
  int scale = getenv("OLED_SCALE") ? atoi(getenv("OLED_SCALE")) : 6;
  signal(SIGPIPE, SIG_IGN);
  int to_c[2], from_c[2];
  if (pipe(to_c) || pipe(from_c)) { perror("pipe"); return 1; }
  pid_t pid = fork();
  if (pid == 0) {
    dup2(to_c[0], 0); dup2(from_c[1], 1);
    close(to_c[1]); close(from_c[0]);
    execvp(argv[1], argv + 1);
    perror("exec"); _exit(127);
  }
  close(to_c[0]); close(from_c[1]);

  if (SDL_Init(SDL_INIT_VIDEO)) { fprintf(stderr, "SDL: %s\n", SDL_GetError()); return 1; }
  SDL_SetHint(SDL_HINT_RENDER_SCALE_QUALITY, "0");
  SDL_Window *win = SDL_CreateWindow("Heltec V3 OLED (emulated)", SDL_WINDOWPOS_CENTERED,
                                     SDL_WINDOWPOS_CENTERED, W * scale, H * scale, 0);
  SDL_Renderer *ren = SDL_CreateRenderer(win, -1, 0);
  SDL_Texture *tex = SDL_CreateTexture(ren, SDL_PIXELFORMAT_ARGB8888,
                                       SDL_TEXTUREACCESS_STREAMING, W, H);
  Uint32 px[W * H];
  for (int i = 0; i < W * H; i++) px[i] = 0xFF000008;
  SDL_StartTextInput();

  const char *typed = getenv("OLED_TYPE");
  Uint32 quit_ms = getenv("OLED_QUIT_MS") ? (Uint32)atoi(getenv("OLED_QUIT_MS")) : 0;
  Uint32 start = SDL_GetTicks(), type_at = start + 800;
  int running = 1, dirty = 1;
  while (running) {
    struct pollfd p = {from_c[0], POLLIN, 0};
    if (poll(&p, 1, 8) > 0) {
      unsigned char buf[4096];
      ssize_t n = read(from_c[0], buf, sizeof buf);
      if (n <= 0) running = 0; else parse(buf, (int)n, &dirty);
    }
    SDL_Event e;
    while (SDL_PollEvent(&e)) {
      if (e.type == SDL_QUIT) running = 0;
      else if (e.type == SDL_TEXTINPUT) { if (write(to_c[1], e.text.text, strlen(e.text.text)) < 0) {} }
      else if (e.type == SDL_KEYDOWN) {
        const char *s = 0;
        switch (e.key.keysym.sym) {
          case SDLK_RETURN: case SDLK_KP_ENTER: s = "\r"; break;
          case SDLK_BACKSPACE: s = "\b"; break;
        }
        if (s && write(to_c[1], s, 1) < 0) {}
      }
    }
    if (typed && SDL_GetTicks() >= type_at) {
      for (const char *t = typed; *t; t++) {
        char ch = *t == '\n' ? '\r' : *t;
        if (write(to_c[1], &ch, 1) < 0) {}
      }
      typed = 0;
    }
    if (dirty && have_frame) {
      for (int y = 0; y < H; y++)
        for (int x = 0; x < W; x++) px[y * W + x] = lit(x, y) ? 0xFF55AAFF : 0xFF000008;
      SDL_UpdateTexture(tex, 0, px, W * 4);
      SDL_RenderClear(ren); SDL_RenderCopy(ren, tex, 0, 0); SDL_RenderPresent(ren);
      dirty = 0;
    }
    if (quit_ms && SDL_GetTicks() - start > quit_ms) running = 0;
  }
  if (getenv("OLED_DUMP") && have_frame)
    for (int y = 0; y < H; y += 2) {
      for (int x = 0; x < W; x++) {
        int t = lit(x, y), b = lit(x, y + 1);
        fputs(t && b ? "█" : t ? "▀" : b ? "▄" : " ", stdout);
      }
      fputc('\n', stdout);
    }
  if (getenv("OLED_DUMP_FILE") && have_frame) {
    FILE *f = fopen(getenv("OLED_DUMP_FILE"), "wb");
    if (f) { fwrite(frame, 1, FRAME, f); fclose(f); }
  }
  kill(pid, SIGTERM); waitpid(pid, 0, 0);
  SDL_Quit();
  return 0;
}

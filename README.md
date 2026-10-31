# ESP32 Forth RNS

Self-contained pocket terminal and Reticulum transport node built on a Heltec WiFi LoRa 32 V3 (ESP32-S3) with an interactive Forth environment.

**Status:** concept and planning. The only hardware running today is a Heltec V3 with the stock prebuilt microReticulum image. The Forth layer, case, and extensions are not built yet. This repository currently contains planning and architecture documentation.

## Project Goals

The immediate goal is to get the software stack (Forth layer, TUI, input mapping, bridge to microReticulum) working **in emulation first**, before committing to hardware. The case design follows later and does not block software work.

The envisioned device combines:

- A full interactive Forth command-line / TUI environment (ESP32Forth or compatible)
- microReticulum as the networking backend (standalone transport node + modem for a phone), with full RNode functionality preserved and operated through the REPL
- Compact MessageEase-style thumb keyboard
- Mechanical form factor that stays slim in a pocket and flips open into a usable terminal
- Optional multi-transport (LoRa + audio modem + Bluetooth audio)
- Ability to run classic text-based software and games from flash

To my knowledge, no existing product or finished open-source build combines these elements.

## Hardware

### Board
- **Model**: Heltec HTIT-WB32LAF (Heltec WiFi LoRa 32 V3)
- **MCU**: ESP32-S3FN8 (dual-core 240 MHz, 8 MB flash, 512 KB SRAM)
- **Radio**: SX1262 LoRa (915 MHz band for California / US915)
- **Display**: Onboard 0.96" 128×64 OLED (SSD1306, blue). External green/yellow monochrome OLEDs exist but the onboard display will be retained.
- **GPIO**: Approximately 20 usable pins exposed (including TX/RX). Sufficient for additional buttons, sensors, or audio. TODO: document the pin map after radio and OLED take their pins.
- **Wi-Fi / Bluetooth**: Built into the ESP32-S3 (relevant to the SSH and audio extensions).
- **Current firmware**: the standard prebuilt microReticulum 1.86 image for this board, flashed unmodified and running in transport node mode. No custom firmware yet.

### Power

**Current prototype (running today)**
- **Battery**: small LiPo pack.
- **Settings applied**: display intensity 1, blank after 15 s, Wi-Fi off, reduced TX power, and airtime limits (short-term ~12 %, long-term ~7 %). The airtime limits are self-imposed to preserve battery life, not regulatory.
- **Thermal**: ESP32-S3 package runs warm under continuous RX. Heatsink or further duty cycling recommended.

**Planned build (idea only, not built)**
- **Battery**: single 18650 Li-ion cell (3000–3500 mAh, ~11–13 Wh).
- **Estimated runtime** (calculated, not measured): 40–70+ hours continuous RX at 40–80 mA with display blanked; multi-day to 1–2 weeks with light sleep and duty cycling.
- **Charging**: onboard USB-C charger supports the cell; external 18650 chargers for faster swaps. TODO: verify the V3 charge circuit is suitable for an 18650.

### Case Concept (Pivoting T-Design)
*Planned. The case will be designed in SolidWorks and printed on industrial 3D printers. Nothing has been modeled yet.*

- **Closed (portrait)**: Heltec board and 18650 stacked in a slim slab roughly the width of the board. Screen faces out. MessageEase buttons and extra triggers sit under the pivoted board and are physically covered. Only stock PRG and RST buttons remain reachable. Over-center or light detent holds it shut.
- **Open (landscape T)**: Thumb-flip the board 90° around a pivot centered on the screen. OLED becomes landscape at the top of the T; 18650 becomes the vertical handle/tail. Four MessageEase buttons are exposed exactly where a thumb rests when gripping the battery. Second over-center locks it open.
- **Mechanics**: Short steel pin or printed living hinge with metal bushing. Battery leads and button wiring via flex cable or buttons on the fixed handle half. Optional magnetic reed or microswitch for electrical interlock. Small torsion spring + cam or printed over-center link. Higher-infill or metal inserts on pivot bosses and battery cradle.
- **Result**: Closed = slim pocketable RNode with normal buttons and portrait status. Open = landscape TUI + thumb keyboard with 18650 as grip. Snap shut and keyboard disappears/disabled. No external switch required. Fully printable on industrial 3D printers.

### Input
- The author already uses a MessageEase-style keyboard daily on a phone, so the learning curve is not a concern.
- MessageEase-style 4-button keyboard (simultaneous presses encode the 9 positions of the classic layout; tap vs hold for variants).
- Additional mode/trigger buttons (symbols, numbers, etc.) easily added in the printed case.
- Physical covering when closed prevents accidental presses.
- Button lock / interlock so pocketed presses cannot execute commands (including dangerous Forth words such as `FORGET`).
- Optional USB keyboard support for development or full typing.

## Software Architecture

### Reticulum / microReticulum Layers

*Design decision: Forth is a thin layer on top of the existing backend. It does not replace any Reticulum or RNode logic.*

**Kept in C++ (microReticulum core)**:
- **Identity**: X25519 key agreement + Ed25519 signatures. Constant-time crypto stays in the existing library. TODO: verify how destination hashes are derived (name hash + identity hash, truncated) against the Reticulum manual.
- **Packet**: Wire format (2-byte header + addresses + context + data ≤ 464 bytes), hashing, packing flags, proofs.
- **Interface**: SX1262 radio driver, RX/TX, airtime accounting, IFAC.
- **Link handshake crypto**: X25519 key exchange and session-key derivation primitives. TODO: verify the exact handshake and cipher details against the Reticulum manual.

**Also stays in C++**:
- **Transport**: path tables, announce propagation, next-hop forwarding. Reimplementing these in Forth was considered and rejected: matching the reference behavior exactly is risky, and mistakes could misbehave on a shared network.
- **RNode functionality**: the existing RNode behavior (radio configuration, KISS interface, transport mode) is preserved as is.

**Exposed to Forth through a thin bridge**:
- **RNode control from the REPL**: Forth words to read and set radio parameters (frequency, bandwidth, spreading factor, TX power, airtime limits), toggle transport mode, and read status and statistics. This replaces `rnodeconf` for on-device use.
- **Messaging primitives**: send and receive packets, create destinations, trigger announces, read received announces, sign and verify.
- **Application layer in Forth**: display, user input, chat, LXMF-style messaging UI, duty-cycling policy.
- **Provisioning / Config**: simple Forth words reading/writing blocks or SPIFFS.

### Dual-Role Operation
- **Standalone peer / transport node**: Own Identity, uses microReticulum's own path handling, can originate and forward packets when the phone is absent.
- **Modem for phone**: Thin interface (USB, BLE, or serial) presents the SX1262 as a KISS-style or raw-frame interface (TODO: decide whether to stay RNode-KISS-compatible, as the stock firmware already is). Phone’s full Reticulum Transport owns large path tables, multi-interface routing, and announce logic. ESP32 hands received frames to the phone and accepts frames to transmit.
- Mode switch owned by Forth. Both roles can run concurrently when the phone is attached; fallback to light mode when disconnected.

### Forth Layer
- ESP32Forth (or compatible) as the interactive environment, which removes the need to write a custom Forth. TODO: pin a version.
- Integration: ESP32Forth must call into the C++ bridge entry points. TODO: decide how (custom-built ESP32Forth with added C functions, versus another mechanism), since microReticulum_Firmware is a separate project.
- Full command-line / TUI. User is familiar with TUIs and Linux terminals; adapting to Forth with a “funny keyboard” is acceptable.
- Custom 3×5 ASCII bitmap font (each character 2 bytes → full 7-bit set in 192 bytes) yielding ~32×10 character TUI on the 128×64 OLED.
- Words that call C++ entry points for radio send/recv, Identity sign/verify, packet pack/unpack, etc.
- Announce policy, modem bridge, OLED rendering, button handling, sleep policy.
- Once a word has a stable stack effect and passes unit tests it becomes a black box.
- Development aid: fine-tuned LLM that emits a Forth word from a desired stack effect, optionally refined by a genetic algorithm against unit tests for size/speed.

### Spitballed Ideas (not committed scope)
*Longer-term ideas that are not part of the core plan. None are designed or scheduled.*

- **Audio modem**: Bell 103 (300-baud FSK) via bit-banging / PWM / DAC / ADC or I2S, with Goertzel detection. Speaker + mic, 3.5 mm TRRS jack (headphone/mic for cell calls), optional old-school 4-pin modular phone plug (with isolation) and phone jack. Bluetooth Classic HFP so the device can be used as a Bluetooth audio device during cell calls.
- **ECC for audio**: XOR-block based ECC with per-block CRC to identify and recover damaged blocks. Encryption already available via Reticulum.
- **SSH client**: Thin SSH protocol layer (handshake state machine, packet framing, MAC, channel, PTY) that reuses microReticulum’s existing X25519 and Ed25519 primitives instead of duplicating curve code. The session cipher (AES-GCM or ChaCha20-Poly1305) would need to be added; it is not assumed to be in microReticulum. Exposed as Forth words. Uses the ESP32-S3 Wi-Fi. Full libssh/libssh2 ports and PocketSSH exist for reference; the goal is to avoid duplicating the elliptic-curve implementation.
- **Flash content**: Classic 80s micro games (e.g. Elite ports), text-based Forth spreadsheet. 8 MB flash is ample.

## Milestones

Pure-Forth work comes first: it needs no radio and runs entirely in emulation.

1. **3×5 font renderer**: port the author's existing 3×5 ASCII font into Forth words that draw characters to a 128×64 framebuffer (~32×10 text grid).
2. **Chord decoder**: Forth words mapping 4-button chords (tap vs hold) to characters, tested against scripted button input.
3. **Line editor and REPL on the OLED**: input line, scrolling output, and stack display, running in emulation.
4. **Words as black boxes**: unit-test harness for stable words; store them in SPIFFS/blocks.
5. **Bridge stubs**: C++ entry points for radio and RNode control stubbed on the host; Forth words call them through the shared memory window (see Memory Budget).
6. **Vertical slice**: one message sent and received between two nodes, driven from the Forth REPL.
7. **RNode control from the REPL**: Forth words for radio parameters, transport mode, and status.
8. **Real backend**: relink against microReticulum on the physical board; measure free heap, airtime, current draw, and heat.
9. **Input prototype**: breadboard the 4-button keyboard.
10. **Case and 18650 build**: SolidWorks design, printing, and battery measurements.

Spitballed ideas come after these and are not scheduled.

## Memory Budget

*Estimates, not measurements. Measure on the board at milestone 8.*

- **Total**: the ESP32-S3FN8 has 512 KB of on-chip SRAM and, on the Heltec V3, no PSRAM. Flash (8 MB) does not count toward RAM.
- **Not available to applications**: part of the 512 KB is reserved for the ROM/IRAM/cache and the FreeRTOS and driver overhead, so the usable heap after boot is typically in the range of 300 KB. Wi-Fi and Bluetooth stacks take a large further share when enabled (Wi-Fi is currently disabled).
- **microReticulum footprint**: unknown. It depends on its path table, packet buffers, and announce queues. TODO: record free heap (`esp_get_free_heap_size()` and the largest free block) with the stock firmware idle, in transport mode, and under load.
- **Forth arena**: reserve a fixed 64–128 KB block, allocated once at boot, as the only memory Forth can address. This keeps Forth bugs (stray `!` or `C!`) from corrupting the networking backend.
- **Bridge across the window**: C++ entry points take offsets inside the arena (buffer address + length), and the C++ side bounds-checks every offset before touching memory. Packets and results are exchanged by copying through buffers inside the arena. Forth never holds raw pointers into the C++ heap.
- **Open question**: ESP32Forth's default memory layout; check whether the arena size and location can be configured, or whether the build needs patching.

## Development Approach

**Phase 1 (current focus): software in emulation.** Everything below through step 4 runs without the physical board. **Phase 2:** relink and validate on hardware. **Phase 3:** CAD and print the case (SolidWorks, industrial 3D printers).

1. Reduce radio, Identity, and Packet operations to a small set of C++ entry points.
2. Stub those entry points on the host (log, loopback, or canned frames) or inside Wokwi.
3. Develop Forth TUI, MessageEase mapping, modem bridge, mode switch, and bridge words against the stubs.
4. Unit-test words; commit stable ones as black boxes to SPIFFS.
5. Relink against real microReticulum when the Forth layer and bridge are solid.
6. Flash the physical Heltec only for airtime, heat, and hardware validation.

No high-fidelity cycle-accurate Heltec V3 emulator with SX1262 exists. Candidate tools: Wokwi for UI/button/display work; QEMU/Renode or host native builds for C++ logic. TODO: pick the toolchain and add a repo layout. Radio-dependent testing uses stubs until the real board.

## Current Status (as of late October 2026)

- Hardware: Heltec V3 with a small LiPo pack (not the planned 18650).
- Heltec V3 successfully flashed with the stock microReticulum firmware 1.86 after resolving “firmware corrupt” / hash mismatch issues (full chip erase + rnodeconf --autoinstall --force-update).
- Transport node mode activated via rnodeconf --tnc.
- Display intensity set to 1, blanking timeout 15 s, Wi-Fi disabled, TX/RX settings reduced to preserve the small battery.
- Unsigned firmware warning is expected and harmless (microReticulum builds are not signed with the official RNode key).
- Not started (concept only): Forth layer, emulation setup, case design, audio modem, SSH client.
- Frequency band confirmed as 915 MHz for legal operation in California.

## Related Projects

- [microReticulum](https://github.com/attermann/microReticulum)
- [microReticulum_Firmware](https://github.com/attermann/microReticulum_Firmware)
- [ESP32Forth](https://esp32forth.appspot.com/)
- Reticulum Network Stack (markqvist/Reticulum)
- PocketSSH (ESP32-S3 T-Deck SSH terminal) and LibSSH-ESP32 / libssh2_esp32 ports for reference

## License

TBD. Check the licenses of microReticulum, microReticulum_Firmware, and ESP32Forth before choosing one, since they constrain what this project can use.

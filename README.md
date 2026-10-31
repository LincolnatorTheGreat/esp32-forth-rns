# ESP32 Forth RNS

Self-contained pocket terminal and Reticulum transport node built on a Heltec WiFi LoRa 32 V3 (ESP32-S3) with an interactive Forth environment.

**Status:** concept and planning. The only hardware running today is a Heltec V3 with the stock prebuilt microReticulum image. The Forth layer, case, and extensions are not built yet. This repository currently contains planning and architecture documentation.

## Project Goals

The immediate goal is to get the software stack (Forth layer, TUI, input mapping, bridge to microReticulum) working **in emulation first**, before committing to hardware. The case design follows later and does not block software work.

The envisioned device combines:

- A full interactive Forth command-line / TUI environment (ESP32Forth or compatible)
- microReticulum as the networking backend (standalone transport node + modem for a phone)
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
- MessageEase-style 4-button keyboard (simultaneous presses encode the 9 positions of the classic layout; tap vs hold for variants).
- Additional mode/trigger buttons (symbols, numbers, etc.) easily added in the printed case.
- Physical covering when closed prevents accidental presses.
- Button lock / interlock so pocketed presses cannot execute commands (including dangerous Forth words such as `FORGET`).
- Optional USB keyboard support for development or full typing.

## Software Architecture

### Reticulum / microReticulum Layers

*Proposed split. This is a design hypothesis to validate in emulation, not a settled decision.*

**Kept in C++ (microReticulum core)**:
- **Identity**: X25519 key agreement + Ed25519 signatures. Constant-time crypto stays in the existing library. TODO: verify how destination hashes are derived (name hash + identity hash, truncated) against the Reticulum manual.
- **Packet**: Wire format (2-byte header + addresses + context + data ≤ 464 bytes), hashing, packing flags, proofs.
- **Interface**: SX1262 radio driver, RX/TX, airtime accounting, IFAC.
- **Link handshake crypto**: X25519 key exchange and session-key derivation primitives. TODO: verify the exact handshake and cipher details against the Reticulum manual.

**Moved or simplified in Forth (experiment)**:
- **Transport**: Path tables, announce propagation, next-hop forwarding, job scheduler. For a single-radio pocket node a minimal “forward if next hop exists, otherwise drop/announce” loop may be small. No node ever knows a full path—only the next hop. Open questions: correctness of forwarding and announce rate-limiting against the reference implementation, and whether Forth is fast enough. If not, Transport stays in C++.
- **Destination / Announce**: Creation, signing, broadcast timing, and handling of received announces (crypto calls stay in C++).
- **Link state machine**: Handshake sequencing, keep-alives, request/response, timeouts (crypto steps in C++).
- **Application layer**: LXMF store-and-forward, display, user input, duty-cycling, chat, etc.
- **Provisioning / Config**: Replaced by simple Forth words reading/writing blocks or SPIFFS.

### Dual-Role Operation
- **Standalone peer / light transport node**: Own Identity, light path table (dozens of next-hop entries), can originate and forward packets when the phone is absent.
- **Modem for phone**: Thin interface (USB, BLE, or serial) presents the SX1262 as a KISS-style or raw-frame interface (TODO: decide whether to stay RNode-KISS-compatible, as the stock firmware already is). Phone’s full Reticulum Transport owns large path tables, multi-interface routing, and announce logic. ESP32 hands received frames to the phone and accepts frames to transmit.
- Mode switch owned by Forth. Both roles can run concurrently when the phone is attached; fallback to light mode when disconnected.

### Forth Layer
- ESP32Forth (or compatible) as the interactive environment, which removes the need to write a custom Forth. TODO: pin a version.
- Integration: ESP32Forth must call into the C++ bridge entry points. TODO: decide how (custom-built ESP32Forth with added C functions, versus another mechanism), since microReticulum_Firmware is a separate project.
- Full command-line / TUI. User is familiar with TUIs and Linux terminals; adapting to Forth with a “funny keyboard” is acceptable.
- Custom 3×5 ASCII bitmap font (each character 2 bytes → full 7-bit set in 192 bytes) yielding ~32×10 character TUI on the 128×64 OLED.
- Words that call C++ entry points for radio send/recv, Identity sign/verify, packet pack/unpack, etc.
- Light path table, announce policy, modem bridge, OLED rendering, button handling, sleep policy.
- Once a word has a stable stack effect and passes unit tests it becomes a black box.
- Development aid: fine-tuned LLM that emits a Forth word from a desired stack effect, optionally refined by a genetic algorithm against unit tests for size/speed.

### Optional Extensions
- **Audio modem**: Bell 103 (300-baud FSK) via bit-banging / PWM / DAC / ADC or I2S, with Goertzel detection. Speaker + mic, 3.5 mm TRRS jack (headphone/mic for cell calls), optional old-school 4-pin modular phone plug (with isolation) and phone jack. Bluetooth Classic HFP so the device can be used as a Bluetooth audio device during cell calls.
- **ECC for audio**: XOR-block based ECC with per-block CRC to identify and recover damaged blocks. Encryption already available via Reticulum.
- **SSH client**: Thin SSH protocol layer (handshake state machine, packet framing, MAC, channel, PTY) that reuses microReticulum’s existing X25519 and Ed25519 primitives instead of duplicating curve code. The session cipher (AES-GCM or ChaCha20-Poly1305) would need to be added; it is not assumed to be in microReticulum. Exposed as Forth words. Uses the ESP32-S3 Wi-Fi. Full libssh/libssh2 ports and PocketSSH exist for reference; the goal is to avoid duplicating the elliptic-curve implementation.
- **Flash content**: Classic 80s micro games (e.g. Elite ports), text-based Forth spreadsheet. 8 MB flash is ample.

## Development Approach

**Phase 1 (current focus): software in emulation.** Everything below through step 4 runs without the physical board. **Phase 2:** relink and validate on hardware. **Phase 3:** CAD and print the case (SolidWorks, industrial 3D printers).

1. Reduce radio, Identity, and Packet operations to a small set of C++ entry points.
2. Stub those entry points on the host (log, loopback, or canned frames) or inside Wokwi.
3. Develop Forth TUI, MessageEase mapping, light path table, modem bridge, mode switch, and bridge words against the stubs.
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

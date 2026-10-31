# ESP32 Forth RNS

Self-contained pocket terminal and Reticulum transport node built on a Heltec WiFi LoRa 32 V3 (ESP32-S3) with an interactive Forth environment.

This repository currently contains planning and architecture documentation. Implementation is ongoing.

## Hardware

- **Board**: Heltec HTIT-WB32LAF (Heltec WiFi LoRa 32 V3)
  - ESP32-S3FN8 (dual-core 240 MHz, 8 MB flash, 512 KB SRAM)
  - SX1262 LoRa radio
  - 0.96" 128×64 OLED (SSD1306, blue)
  - Exposed GPIO (~20 usable pins including TX/RX)
- **Battery**: Single 18650 Li-ion cell (3000–3500 mAh) for multi-day runtime
- **Case concept**: Pivoting board design
  - Closed: slim portrait slab, buttons covered, only stock PRG/RST exposed
  - Open: landscape T-shape with 18650 as handle, MessageEase-style 4-button thumb keyboard exposed
  - Over-center or detent mechanism; optional spring/latch
  - Physical covering of buttons when closed acts as interlock

## Software Architecture

### Core (C++)
- microReticulum firmware (attermann/microReticulum_Firmware) providing:
  - Identity (X25519 + Ed25519)
  - Packet framing and hashing
  - SX1262 radio interface
  - Optional Link crypto primitives
- Thin C++ bridge exposing radio, crypto, and packet operations as callable entry points for Forth

### Interactive Layer (Forth)
- ESP32Forth (or compatible) as the user-facing environment
- Full interactive command line / TUI
- Light path table and announce policy (standalone peer/transport node)
- Modem bridge mode when phone is attached (phone owns heavy Transport tables)
- OLED TUI with custom 3×5 ASCII bitmap font (192 bytes for 7-bit set)
- MessageEase-style 4-button input (plus mode/trigger buttons)
- Duty-cycle / light-sleep policy
- Optional higher protocols (LXMF, simple chat, etc.)

### Dual-role operation
- **Standalone**: light path table, own Identity, can originate and forward packets
- **Phone-attached**: acts as radio modem; phone runs full Reticulum Transport; ESP32 falls back to light mode when disconnected

### Additional capabilities under consideration
- Bell 103 soft modem (bit-banged FSK) with speaker/mic, 3.5 mm TRRS, optional modular phone plug, Bluetooth HFP
- XOR-block ECC with per-block CRC for audio path
- Stripped-down SSH client reusing microReticulum’s X25519/Ed25519 primitives (thin protocol layer only)
- Classic 80s games or text-based Forth spreadsheet stored in flash
- LLM + genetic algorithm assisted Forth word generation from stack effects (development tool)

## Power and Thermal

- Target: 1–2 weeks runtime on 18650 with light sleep + low-power radio settings
- Display: intensity 1, blank after 15 s
- Wi-Fi off when not needed
- Airtime limits (short-term ~12 %, long-term ~7 %) and lower TX power to reduce heat
- ESP32-S3 package can run warm under continuous RX; heatsink or further duty cycling recommended

## Development Approach

1. Stub the C++ radio/crypto entry points on the host or in Wokwi
2. Develop Forth TUI, button mapping, light path table, and bridge words against stubs
3. Unit-test words; treat stable ones as black boxes
4. Relink against real microReticulum when ready
5. Flash Heltec only for airtime and hardware validation

No full Heltec V3 emulator with SX1262 exists; radio-dependent work uses stubs until hardware testing.

## Current Status

- Heltec V3 successfully flashed with microReticulum firmware (version 1.86)
- Transport node mode activated
- Display dimmed/blanked, Wi-Fi disabled
- Unsigned firmware warning is expected (microReticulum builds are not signed with the official RNode key)
- Case design and Forth integration in planning/prototyping stage

## Related Projects

- [microReticulum](https://github.com/attermann/microReticulum)
- [microReticulum_Firmware](https://github.com/attermann/microReticulum_Firmware)
- [ESP32Forth](https://esp32forth.appspot.com/)
- Reticulum Network Stack (markqvist/Reticulum)

## License

TBD. Planning document only at this stage.

# Ironstrait

Offline Mercenary sandbox prototype for Nate’s personal class shooter.

**Engine (pinned):** Unity **6000.3.29f1** (Unity 6.3 LTS) · C# · URP `com.unity.render-pipelines.universal` **17.3.0** · Input System package present (legacy `Input` API used in v0) · uGUI **2.0.0**

Working title undecided; repo/folder name is provisional.

## What this is

A Cursor-scaffolded Unity project that opens on macOS/Windows once Unity Hub has **6.3 LTS (6000.3.29f1)** installed. The cloud VM that authored this pass does **not** run the Unity Editor — gameplay is implemented in C# with runtime graybox construction so you can press Play after package resolve.

Design source (copied into repo): `docs/Class_Shooter_Design_Reference.md` (Version 0.24).

## Open / run (Mac)

1. Install [Unity Hub](https://unity.com/download).
2. Install editor **Unity 6.3 LTS → 6000.3.29f1** (Modules: Mac Build Support if you will build standalone; Windows optional later).
3. Hub → **Open** → select this repository folder (`ironstrait`).
4. Wait for Package Manager to restore URP / Input System / uGUI.
5. **First-open URP wiring (required once):**
   - `Assets → Create → Rendering → URP Asset (with Universal Renderer)`
   - Save under `Assets/Settings/` (e.g. `URP-HighFidelity`).
   - `Edit → Project Settings → Graphics` → set **Scriptable Render Pipeline Settings** to that asset.
   - `Edit → Project Settings → Quality` → assign the same (or quality-specific) URP asset on the active quality level.
6. Open `Assets/Scenes/BootMenu.unity` and press **Play**.
7. Menu: **Play Sandbox** → graybox arena with Mercenary controls.

### Controls (sandbox)

| Action | Default |
| --- | --- |
| Move | WASD |
| Look | Mouse |
| Jump | Space |
| Crouch | Left Ctrl / C |
| Slide (stub) | ⌘ (Mac) / Left Alt (elsewhere) |
| Leap (stub) | Q |
| Shotgun | LMB |
| Grenade launcher | RMB |
| Console | \` |
| Pause / menu | Esc |

Console: `help`, `kill`, `respawn`, `reset`, `set health 150`, `set level 8`, `target clear`, `target spawn`.

Settings JSON path (after first save): `Application.persistentDataPath` / `user_settings_v1.json`.

Balance numbers: `Assets/Resources/Config/mercenary_baseline.json`.

## Playable now vs stubbed

See Project store plan: will also live at `docs/` in the Ironstrait Project Context as `prototype-v0-plan.md`.

**Playable:** menu, settings persistence, FPS move + air strafe, dual-weapon fire, auto reload, dummy targets + score/level HUD, health packs, basic console, graybox arena.

**Stubbed / minimal:** wallclimb, full slide/leap feel, melee, audio, polish VFX, most level-upgrade *gameplay* application beyond multipliers already wired for grenade/shotgun, networking, other classes, modes.

## Package pins

| Package | Version |
| --- | ---: |
| Unity Editor | 6000.3.29f1 |
| URP | 17.3.0 |
| Input System | 1.14.2 |
| uGUI | 2.0.0 |

Exact Hub patch may float within 6000.3.x; if Hub offers a newer **6000.3.*** LTS patch, prefer matching `ProjectSettings/ProjectVersion.txt` or allow Hub to upgrade the patch and re-pin in git after a clean open.

## License / intent

Personal noncommercial prototype. Not for monetization.

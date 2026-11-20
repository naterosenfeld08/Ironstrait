# Ironstrait

Offline Mercenary sandbox prototype for Nate’s personal class shooter.

**Repo:** https://github.com/naterosenfeld08/Ironstrait (**public**)  
**Engine (pinned):** Unity **6000.3.29f1** (Unity 6.3 LTS) · C# · URP `com.unity.render-pipelines.universal` **17.3.0** · Input System package present (legacy `Input` API used in v0) · uGUI **2.0.0**

Working title undecided; repository name is `Ironstrait`.

## What this is

A Cursor-scaffolded Unity project. Install Unity Hub **6.3 LTS (6000.3.29f1)** on your Mac, open this repo, and press Play. Gameplay is C# with a runtime-built graybox arena so scenes need minimal hand wiring.

Design source (in repo): `docs/Class_Shooter_Design_Reference.md` (Version 0.24).  
Status / plan: `docs/prototype-v0-plan.md`.

## Locked prototype decisions

- **Networking:** fully deferred until offline gunfeel passes.
- **Reload:** full-empty totals — shotgun **3.5s** for 6 shells, grenade launcher **4s** for 3 rounds (per-shell/round auto reload derived from those totals).
- **Level 2 Mercenary upgrade:** left **empty** (not every level needs an upgrade).
- **Hardware notes:** Nate’s Mac model / chip / RAM / macOS version are **unknown** — profile later; do not treat this Linux cloud VM as a Mac substitute.

## Open / run (Mac) — required for Nate

Unity Hub + 6.3 LTS are **not** installed on Nate’s machine yet. Do this locally:

1. Install [Unity Hub](https://unity.com/download).
2. Install editor **Unity 6.3 LTS → 6000.3.29f1** (add **Mac Build Support** if you will build standalone).
3. `git clone https://github.com/naterosenfeld08/Ironstrait.git` then Hub → **Open** that folder.
4. Wait for Package Manager to restore URP / Input System / uGUI.
5. **First-open URP wiring** (auto helper may do this; otherwise once by hand):
   - `Assets → Create → Rendering → URP Asset (with Universal Renderer)` → save under `Assets/Settings/`
   - `Edit → Project Settings → Graphics` → assign that URP asset
   - `Edit → Project Settings → Quality` → assign on the active quality level
6. Open `Assets/Scenes/BootMenu.unity` → **Play** → **Play Sandbox**.

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
| Console open / close | \` (Backquote) |
| Console close | Esc (closes console only; does not pause) |
| Pause / menu | Esc (when console is closed) |

Console: `help`, `kill`, `respawn`, `reset`, `set health 150`, `set level 8`, `target clear`, `target spawn`.  
Developer console toggles closed with **Backquote** or **Esc**.

Settings JSON (after first save): `Application.persistentDataPath` / `user_settings_v1.json`.  
Balance numbers: `Assets/Resources/Config/mercenary_baseline.json`.

## Playable now vs stubbed

**Playable:** menu, settings persistence, FPS move + air strafe (collision speed exploit clamped), dual-weapon fire with isolated viewmodel recoil, auto reload, dummy targets + score/level HUD, health packs, basic console (close with \` / Esc), graybox arena, low-poly viewmodels, placeholder SFX.

**Stubbed / deferred:** wallclimb, full slide/leap feel, melee, polish VFX, most upgrade gameplay beyond wired multipliers, **networking**, other classes, modes.

**Audio / viewmodels (v0 polish):** composed low-poly primitive shotgun + grenade launcher (not cubes); procedural placeholder SFX for fire / reload / hit / jump (no external pack — original generated clips, no attribution required).

## Package pins

| Package | Version |
| --- | ---: |
| Unity Editor | 6000.3.29f1 |
| URP | 17.3.0 |
| Input System | 1.14.2 |
| uGUI | 2.0.0 |

Exact Hub patch may float within 6000.3.x; prefer matching `ProjectSettings/ProjectVersion.txt` or re-pin after a clean open.

## License / intent

Personal noncommercial prototype. Not for monetization.

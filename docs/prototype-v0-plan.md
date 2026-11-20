---
cursor:
  subagentId: "bc-a46c5e1c-73e5-5df3-9256-80dc89db50c1"
---

# Prototype v0 — Mercenary sandbox plan / status

**Date:** 2026-11-20 (updated with Nate’s clarifying answers)  
**Design source:** `docs/class-shooter-design-reference.md` (v0.24)  
**GitHub:** https://github.com/naterosenfeld08/Ironstrait (**public**)  
**PR:** https://github.com/naterosenfeld08/Ironstrait/pull/1 (`cursor/mercenary-sandbox-v0-50c1` → `main`)  
**Local code:** `/agent/ironstrait` · branch `cursor/mercenary-sandbox-v0-50c1`

## Verdict

Offline Mercenary graybox sandbox is scaffolded as Unity **6000.3.29f1** / URP **17.3.0** with runtime-built arena, dual weapons, HUD, menu/settings, and score placeholders. **Pushed** to public GitHub; PR open. Cloud VM installed Unity Editor **6000.3.29f1** via Unity CLI (`~/Unity/Hub/Editor/6000.3.29f1`); Nate still needs Hub + 6.3 LTS on his Mac for playtesting.

## Locked decisions (2026-11-20)

| # | Topic | Decision |
| --- | --- | --- |
| 1 | Mac model | Unknown — document honestly; do not block |
| 2 | Unity on Nate’s Mac | Hub + 6.3 LTS **not** installed yet — he still needs local Hub |
| 3 | Repo visibility | **Public** `naterosenfeld08/Ironstrait` |
| 4 | Networking | **Fully deferred** until offline gunfeel passes |
| 5 | Reload timing | **Full-empty totals** (3.5s / 6 shells, 4s / 3 grenades) — keep; tweak later |
| 6 | Level 2 Mercenary | **Leave empty** — not every level needs an upgrade |

## What shipped

| Area | Status |
| --- | --- |
| Unity project skeleton + pinned versions in README | Done |
| Design doc → `docs/Class_Shooter_Design_Reference.md` | Done |
| Boot menu: Play Sandbox / Settings / Quit | Done |
| Versioned JSON settings → `persistentDataPath/user_settings_v1.json` | Done |
| Editable balance JSON (`mercenary_baseline.json`) | Done |
| FPS controller: WASD, look, jump, crouch, air strafe | Done |
| Shotgun hitscan (deterministic plus) + GL projectiles | Done |
| Independent dual fire + automatic reloads (full-empty totals) | Done |
| Health (150) + out-of-combat regen + health packs | Done |
| Dummy targets + score/level HUD | Done (upgrade multipliers partial) |
| Level 2 upgrade slot | Empty by design |
| Dev console (partial) | Done |
| Graybox arena + static cube viewmodels | Done |
| First-open URP auto-assign editor helper | Done (unverified in Editor) |

## Stubbed / deferred

- Wallclimb / wallride; slide & leap charge stubs only
- Quick melee, audio, polish VFX/animation
- Remaining progression upgrades (3+); level 2 stays empty
- `set damage_multiplier` console override
- **Networking / other classes / modes** (explicitly deferred)
- Standalone Windows/macOS ZIP builds
- Signed macOS distribution

## How to open / run (Nate’s Mac)

1. Install [Unity Hub](https://unity.com/download).
2. Install editor **Unity 6.3 LTS → 6000.3.29f1** (Mac Build Support module as needed).
3. Clone `https://github.com/naterosenfeld08/Ironstrait` (after push lands).
4. Hub → Open the repo folder; wait for Package Manager.
5. First open: URP helper should assign `Assets/Settings/` pipeline assets (or assign manually per README).
6. Open `Assets/Scenes/BootMenu` → Play → **Play Sandbox**.

### Host / machine notes

- **Nate’s Mac:** model / chip / RAM / macOS version **unknown** (not provided). Profile separately once known.
- **This cloud VM:** Linux `x86_64`, Ubuntu 24.04 — not a Mac substitute for Apple Silicon / macOS packaging checks.

## Unity install outcome (cloud VM)

| Item | Result |
| --- | --- |
| Unity CLI | Installed 1.0.1 (`~/.local/bin/unity`) |
| Editor 6000.3.29f1 (linux-x64) | **Installed** → `/home/ubuntu/Unity/Hub/Editor/6000.3.29f1` |
| Unity account / license on VM | Signed out / unknown — batch Play may be limited without `unity auth login` |
| Nate’s Mac Hub + 6.3 | **Still required** (not installed on his machine) |

## Open technical risks

1. First Editor open may re-serialize hand-authored scenes/metas; pink materials until URP assigned.
2. Gunfeel unverified until Editor Play on Nate’s Mac.
3. Same-frame dual lethal + grenade self-damage jump impulse need playtest.
4. Input System package listed; v0 still uses legacy `Input.*`.
5. Cloud VM Unity license unsigned — headless validate may not fully prove Play Mode.

## Next iteration priorities

1. Land push/PR → Nate clones and opens on Mac after Hub install.
2. Confirm Play works; capture console errors.
3. Tune move / air accel / pellet spread from feel.
4. Flesh slide/leap; add melee stub.
5. Wire remaining upgrades (skip empty level 2).
6. Netcode only after offline gunfeel passes.

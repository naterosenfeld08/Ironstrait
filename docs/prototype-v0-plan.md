---
cursor:
  subagentId: "bc-a46c5e1c-73e5-5df3-9256-80dc89db50c1"
---

# Prototype v0 — Mercenary sandbox plan / status

**Date:** 2026-11-20  
**Design source:** `docs/class-shooter-design-reference.md` (v0.24) — coherent enough to scaffold; numerical values remain provisional assistant defaults.  
**Code location:** `/agent/ironstrait` (intended GitHub: `naterosenfeld08/ironstrait`)

## Verdict

Offline Mercenary graybox sandbox is scaffolded as a Unity **6000.3.29f1** / URP **17.3.0** project with runtime-built arena, dual weapons, HUD, menu/settings, and score placeholders. **Not editor-verified on this cloud VM** (no Unity Editor installed). **GitHub repo create/push blocked** from this environment (no `GH_TOKEN`, no create-repo MCP, run has `repoUrl: null`).

## What shipped

| Area | Status |
| --- | --- |
| Unity project skeleton + pinned versions in README | Done |
| Design doc copied to `docs/Class_Shooter_Design_Reference.md` | Done |
| Boot menu: Play Sandbox / Settings / Quit | Done |
| Versioned JSON settings → `persistentDataPath/user_settings_v1.json` | Done |
| Editable balance JSON (`mercenary_baseline.json`) | Done |
| FPS controller: WASD, look, jump, crouch, air strafe | Done |
| Shotgun hitscan (deterministic plus pattern) + GL projectiles | Done |
| Independent dual fire + automatic reloads | Done |
| Health (150) + out-of-combat regen + health packs | Done |
| Dummy targets (damage flash, HP label, respawn) | Done |
| Score from damage + level thresholds (HUD) | Done (upgrade *multipliers* partially applied) |
| Minimal HUD (HP, ammo, score/level, crosshair) | Done |
| Dev console (`help`, kill/respawn/reset, set health/level, targets) | Done (partial) |
| Graybox arena (walls, cover, hill, packs) | Done (runtime-built) |
| Static cube viewmodels | Done |
| First-open URP auto-assign editor helper | Done (unverified) |

## Stubbed / deferred

- Wallclimb / wallride; slide & leap are **charge stubs** (Q / ⌘), not final feel
- Quick melee, audio, polish VFX/animation
- Full level-upgrade kit (level 2 open; recoil removal obsolete; outer pellet damage split at 10)
- `set damage_multiplier` console override
- Networking / other classes / modes / LAN
- Standalone Windows/macOS ZIP builds (need local Editor)
- Signed macOS distribution

## How to open / run

1. Install Unity Hub + **6000.3.29f1** (6.3 LTS).
2. Open the `ironstrait` project folder.
3. Let packages resolve; URP helper should assign `Assets/Settings/` pipeline assets (or assign manually per README).
4. Open `Assets/Scenes/BootMenu` → Play → **Play Sandbox**.

## Open technical risks

1. **Repo hosting:** cannot push until Nate creates empty GitHub repo (or adds `GH_TOKEN` secret) and grants Cursor GitHub App access.
2. **First Editor open:** hand-authored scenes/metas may need Unity to re-serialize; URP pink materials until pipeline assigned.
3. **Gunfeel unverified** without Editor play mode on this VM.
4. **Reload interpretation** uses full-empty totals split per shell/round (design assistant default).
5. **Same-frame dual lethal** and grenade self-damage jump impulse need playtest.
6. **Input System package** is listed but v0 still uses legacy `Input.*` (`activeInputHandler: Both`).

## Next iteration priorities (suggested)

1. Nate opens project on Mac → confirm Play works; note any console errors.
2. Tune move speed / air accel / pellet spread from feel.
3. Flesh slide/leap to design distances; add melee stub.
4. Wire remaining progression upgrades + manual `set level` playtests.
5. Only then: netcode spike for 2-player (deferred per offline-first).

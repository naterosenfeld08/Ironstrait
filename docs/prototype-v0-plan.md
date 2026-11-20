---
cursor:
  subagentId: "bc-867878a0-5909-5d71-9189-eefd1eaf3150"
---

# Prototype v0 — Mercenary sandbox plan / status

**Date:** 2026-11-20 (playtest sync)  
**Design source:** `docs/class-shooter-design-reference.md` (v0.24)  
**GitHub:** https://github.com/naterosenfeld08/Ironstrait (**public**)  
**PR #1:** merged · **PR #2 (polish):** merged · **Playtest sync:** `cursor/playtest-sync-bd19` → `main`

## Verdict

Offline Mercenary graybox sandbox is on **main** (Unity **6000.3.29f1** / URP **17.3.0**), including polish (recoil isolation, bhop clamp, console Esc, viewmodels, SFX). **Playtest sync:** agents merge to `main` via automerge for `cursor/**` (or `automerge` label); Nate double-clicks `scripts/pull-main.command` (Unity closed preferred) then Play. **Networking still deferred.**

## Locked decisions (unchanged)

| # | Topic | Decision |
| --- | --- | --- |
| 1 | Mac model | Unknown — document honestly; do not block |
| 2 | Unity on Nate’s Mac | Hub + 6.3 LTS needed locally |
| 3 | Repo visibility | **Public** `naterosenfeld08/Ironstrait` |
| 4 | Networking | **Fully deferred** until offline gunfeel passes |
| 5 | Reload timing | **Full-empty totals** (3.5s / 6 shells, 4s / 3 grenades) |
| 6 | Level 2 Mercenary | **Leave empty** |

## What shipped

| Area | Status |
| --- | --- |
| Unity skeleton + README pins | Done |
| Design doc in repo | Done |
| Menu / settings JSON / balance JSON | Done |
| FPS + air strafe + walkable-floor jump probe | Done (polish) |
| Collision speed clamp (no prop launch stack) | Done (polish) |
| Shotgun hitscan + GL projectiles | Done |
| Independent dual fire + full-empty reloads | Done |
| Per-weapon viewmodel recoil isolation | Done (polish) |
| Composed low-poly shotgun + GL viewmodels | Done (polish) |
| Procedural placeholder SFX (fire/reload/hit/jump) | Done (polish) |
| Dev console close with Backquote **or Esc** | Done (polish) |
| Health / dummies / score HUD / empty level 2 | Done |
| Graybox arena | Done |
| Mac pull script (`scripts/pull-main.sh` / `.command`) | Done |
| README Playtest loop | Done |
| Automerge workflow (`cursor/**` / `automerge` → main) | Done |

## Stubbed / deferred

- Wallclimb / wallride; slide & leap stubs only
- Quick melee, polish VFX
- Remaining progression upgrades (3+); level 2 stays empty
- `set damage_multiplier` console override
- **Networking / other classes / modes**
- Standalone ZIP builds / signed macOS

## Retest (polish)

1. **Recoil:** Fire shotgun only — GL viewmodel stays still. Fire GL only — shotgun stays still. Fire both same frame — each kicks independently.
2. **Bhop / boxes:** Hold jump into cover cubes / hill edges — should not launch or stack huge speed. Flat-floor bhop + air strafe should still feel responsive.
3. **Console:** \` opens; \` or Esc closes (Esc must not also pause). Esc with console closed still pauses.
4. **Assets:** Viewmodels look like guns (composed primitives). Fire/reload/hit/jump make placeholder sounds.

## Playtest (Nate)

1. Close Unity (preferred).
2. Double-click `scripts/pull-main.command` (or `./scripts/pull-main.sh`).
3. Open project → Play → Play Sandbox.

## Next

1. Nate confirms polish on Mac after Hub install + pull script.
2. Tune move / air accel / pellet spread from feel.
3. Flesh slide/leap; melee stub.
4. Netcode only after offline gunfeel passes.

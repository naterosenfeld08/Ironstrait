# Class Shooter Design Reference

Version 0.24 | Updated November 19, 2026 | Working title undecided

## Purpose and use

This is Nate's living design reference for a personal, noncommercial first person, class based, team based shooter. The goal is a game tailored to his gameplay preferences. Popularity and monetization are not design objectives.

Consult this reference before proposing systems or character kits. Update the same document as decisions change. Preserve the distinction between confirmed direction, illustrative ideas, and unresolved questions. New suggestions are not accepted decisions until Nate adopts them. Numerical values are provisional unless explicitly locked. This document records design intent, not playtest findings.

## Core experience

Fast, responsive movement; deliberate weapons that deal substantial chunks of damage; demanding but learnable combat; independently capable classes; rewarding coordination; and visible score based progression during a match. Becoming skilled should produce a consistent advantage through execution and decisions.

## Confirmed direction

### Rules and player control

- Every restriction or mechanic that adds friction should have a specific gameplay or balance purpose.
- No enemy stuns. The Shaman's proposed M syringe is an explicit exception to the general no-enemy-slows policy. Small, explicit knockback may be used by select weapons or abilities. Ordinary damage does not interrupt movement. The Mercenary's intentional grenade jump remains part of his kit.
- Limit healing, especially during active fights, so solid hits create a lasting opening. Favor small amounts or healing that costs time, positioning, or disengagement. Exact healing systems and support roles remain open.

### Weapons and ammunition

- Extremely satisfying gunfeel is a primary goal. Shots should feel impactful through firing behavior, hit feedback, animation, and sound.
- Most weapons should fire individual, relatively slow, high damage shots. This is a majority preference, not a ban on automatic weapons.
- Inspirations include TF2 Demoman and Soldier; Overwatch Junkrat, Roadhog, and Cassidy; and Counter Strike AWP, Desert Eagle, Nova, and XM. These references concern appealing weapon feel, not wholesale adoption of their damage rules or kits.
- Hitscan should reward precise aim; projectiles should reward prediction.
- Weapons have limited loaded capacity and unlimited reserve ammunition. Reloading is the ammunition constraint.
- Exclude random bullet spread, random critical hits, and meaningful combat randomness. Deterministic spread patterns are not yet decided.

### Movement

- Every character should be very mobile, with fast and responsive movement that does not feel slippery.
- Desired mechanics across the game include wallclimbs, varied jumps, slides, wallrides, and explosive jumps. It is not decided that every character receives every mechanic.
- Movement must feel good in first person and avoid locking players into movement animations.
- Evasive, potentially erratic movement and small hitboxes are desired, but their interaction with slow firing weapons must be tested. Small hitboxes are a preference, not a finalized dimension.
- No fall damage on valid routes and drops. Use clearly marked death zones and barriers outside the playable space.
- Allow full air strafing with precise, responsive keyboard control. Air acceleration, directional reversal, speed limits, and how this affects the intended predictability of the lateral leap need testing.

### Score based progression

- Personal score increases through damage, healing, assists, final blows, damage mitigation, and other support. The latest working rule removes score loss on death. Killing a higher-level enemy earns extra credit. Numerical values below are assistant placeholders.
- Progression carries between rounds on the same side, resets at side swaps, and resets for a new match.
- Simplified progression working rule: begin a match at level 1 with zero score, clamp score at zero, and derive level from score thresholds. Level 2 behaves like any other upgrade threshold. The earned level 2 floor is removed. This is the assistant's reversible implementation choice in response to Nate's request to simplify. Level thresholds and late joining remain open.
- Falling below a threshold immediately removes that upgrade. Buffs or effects already applied keep their ordinary duration after the caster dies. How this rule applies to damage already in flight, deployments, and live snapshots is open.
- High level provides a substantial advantage. Nate expects a level 8 Mercenary to strongly outperform a level 3 Mercenary. Appropriate positioning and execution should still permit meaningful lower-level responses.
- Score thresholds determine upgrades. Fractional score is allowed. Fractional mechanical levels remain undefined.
- The scoreboard should show exact levels. In-world level numerals may be omitted to reduce clutter. Attack-specific cues and upgrade readability remain important.
- Level 10 remains late-game progression. The exceptional level 11 or god-like power-up is removed.
- Solo eligibility uses enemy damage history cleared by full recovery, with an expiry for old contributions whose duration remains to define. Small heals do not clear it. Self damage is not another attacker's contribution. Nate accepted this revision. Assist eligibility includes significant damage or a relevant recent buff, with about a 15-second candidate window. Significant damage and exact attribution remain to define.
- Support credit follows useful outcomes. Applying buffs repeatedly without benefit does not earn score. Exact qualifying events and how utility credit stacks with assist credit remain open.
- Nate proposes outlines beginning at level 8 and becoming brighter or more defined at levels 9 and 10. Assistant prototype recommendation is a thin team-colored silhouette contour, visible only where the character is normally visible, without bloom, flashing, or through-wall reveal. Preserve the same team hue across levels. Exact style and occlusion behavior are recommendations awaiting confirmation.

| Scoring event | Assistant placeholder | Qualification to test |
| --- | ---: | --- |
| Enemy damage | +1 per 100 HP | Actual health removed, excluding overkill |
| Ally healing | +1 per 100 HP | Effective healing only |
| Ally damage mitigation | +0.5 per 100 HP | Actual prevented damage rather than merely holding guard |
| Final blow | +1 | Paid once to the killer |
| Solo kill | +1 extra | Adds to the final blow bonus for a total kill bonus of +2, plus damage credit |
| Assist | +0.5 | Once per eligible assisting player per kill |
| Other support | +0.25 | Per meaningful qualifying buff use, not every tick or repeated application |
| Higher-level kill bonus | +0.25 per level difference, capped at +1 | Compare killer and victim levels immediately before the kill |
| Death | 0 | Working rule removes score loss on death |

These are bookkeeping placeholders for evaluation, without selected level thresholds or a target leveling rate. Support credit, assist eligibility, self-damage healing, and repeated farming need explicit attribution. Partial healing does not clear damage history. Support scoring requires actual benefit.

### Lethality and fairness

- Exclude dedicated map-spanning attacks and sniping. Longer effective ranges may reach the outer edge of midrange. Opponents should be able to understand what exposed them and act on that information.

- Reject lethal single hits or effectively guaranteed lethal sequences requiring little setup or execution.
- Explicit rejected examples: instant lethal sniper quickscopes, backstabs, critical rockets, Roadhog hook combos, Widowmaker charged shots, Hanzo arrows, and AWP bodyshots.
- Lethal outcomes can be acceptable when earned through substantial setup, demanding combinations, or team coordination. No universal minimum time to kill is established.
- Accepted conceptual examples: a prepared Demoman style sticky trap, or landing both a Junkrat style grenade and concussion mine with different trajectories and speeds.
- Trap preparation is intended to expose the attacker to being jumped by a fast, high damage class. Nate considers this vulnerability a legitimate cost, even if the eventual victim arrives after preparation.
- Do not automatically require every prepared lethal attack to give its eventual victim a direct reaction window. Trap visibility, disarming, duration, and other counterplay rules remain open.
- The precise boundary between acceptable burst combinations and unacceptable guaranteed execution still needs definition.
- No combatant, including an exceptionally empowered one, should be unkillable. Every class matchup should retain a realistic possibility of victory through appropriate fight selection and execution; this does not require equal odds in every situation. Earlier protected spawn areas and immunity remain a separate unresolved boundary of this principle.

### Teamplay and independent contribution

- Coordinated teammates using voice communication should be more successful.
- A player joining a random server silently should still be able to earn kills, capture objectives, and contribute meaningfully.
- Independent fighting and cooperation should both be viable. They are not required to have equal effectiveness in every situation.
- Preferred team size is 7v7, with 6v6 a comparison candidate. The roster should lean toward damage classes with some tankier and supporting classes; mandatory role slots are not decided. No duplicate classes per team.
- Nate supports extra solo kill credit as compensation for independent risk, while expecting coordinated objective play to remain the best way to win. Whether attribution achieves that intent needs testing.

### Spawning and environment

- Favor fixed, understandable spawn points or areas and consistent rules.
- Spawn rooms are fully protected. No projectiles enter, damage applies, or debuffs affect occupants. Crossing the exit threshold grants a provisional three seconds of immunity. Nate explicitly wants this to help break spawn camps. Whether it covers self damage, permits attacking, or refreshes on reentry needs definition. Effects on self-damaging rollouts need testing.
- Make spawn exit thresholds recognizable. Neutralizing immunity by attacking and preventing immunity refresh are assistant options, awaiting a decision.
- Reliable collision and readable map geometry are priorities. Avoid snagging, clipping, and geometry that frustrates movement.
- Use grouped respawns that bring teammates back together after clustered deaths. Slightly delay early deaths and shorten later waits to reduce staggering. Nearly immediate respawn is acceptable when joining the team wave. Target maximum waits remain about 7 to 10 seconds. Grouping window and wait bounds need testing. Travel time is separate.

### Core modes and match flow

- Focus on King of the Hill, Payload, and two point Attack Defend. Capture the Flag and Deathmatch are deferred possibilities.
- King of the Hill is best of three, with roughly three minutes of cumulative ownership to win a round. The owner's clock pauses while enemies contest. Nate proposes overtime if zero is reached with both teams present, then the first team to fully capture wins. The exact zero trigger needs definition because contesting already pauses the clock. A separate match deadline versus an expiring ownership clock must be distinguished. Whether overtime overrides accumulated ownership is unresolved.
- Payload has three escort speeds for one, two, and three pushing players. Three players provide maximum speed, leaving four free to fight in 7v7. More players can accompany the payload without increasing speed. The cart gives minor healing only to the pushing team and rolls back after a delay without attacker contact. Checkpoints establish forward attacker spawns. Speed values, contest behavior, heal rate, delay, and rollback limits remain open.
- Attack Defend has sequential points A then B. Capturing both wins the attacking round, failing to capture both loses it. Partial capture progress does not persist. Capturing A secures forward attacker spawns and has strategic value. Earlier progress and completion-time side comparisons require reconciliation with this new binary round outcome. Reset speed of partial captures, round time, time extensions, and overall match scoring remain open.
- Coordinated attacks should break chokes and be rewarded. Chokes can exist in King of the Hill. Payload routes should avoid forcing the cart itself through a one-sided choke. Defender-favored areas can exist, while the payload path should avoid arbitrary high ground advantages beside defender spawn. Alternate approaches, respawn travel, and return timing need tests.
- Matches should have timers. Progression now carries between rounds, so pace against the match structure. Side swap comparison fairness must be revisited.
- Good matchmaking is a priority. Specific skill, latency, party, queue time, and population tradeoffs remain unresolved.

### Skill and roster

- Seek exceptional mastery depth in aim, movement, prediction, positioning, and class mechanics.
- Lower skill demand characters can exist. A uniformly high skill floor is no longer a requirement.
- Character names, roster size, roles, kits, and ability resource systems are not decided.
- Avoid unnecessary content bloat and weak sound design.

### Art direction

The graphic novel *Watchmen* is an illustration influence. The later retro 3D execution brief below uses DUSK as the primary implementation reference. The game should look cartoonish, painterly, Gothic, and gunmetal-heavy. The world combines the atmosphere of 1960s Russian Siberia with the 19th-century American Southwest. Atompunk and Boreal Gothic describe the visual blend. These are influences for a shared setting. Its exact date and geography remain open.

- The world favors early, non-electronic technology and material retrofuturism over magic or spirituality. Mechanical devices and recognizable, intimidating weapons should anchor the setting. Exact technology limits remain open.
- Characters have sleek, strongly stylized silhouettes that remain recognizable at distance and in motion. They are gruff and rough around the edges. The direction excludes ragged costumes, gore-soaked presentation, TF2-style scrappiness, and Overwatch-style gloss and saturated color. The finish should avoid flashiness, photorealism, and a visibly futuristic look.
- Wardrobe includes bomber vests, plaids, jeans, cargo pants, varied boots, and substantial use of leather. Base character colors include grays, bone whites, dark browns, navy, dark orange, and dark reds, with silver and black accents.
- The preferred team colors are muted green and muted purple. Class shape and team identification must both read during combat. Team affiliation should be immediately legible on characters, projectiles, team sides of maps, and team-owned dropped items. Exact color values, placement, neutral object treatment, side swap behavior, and supplemental identification cues remain open.
- Visual clarity is a top priority. Avoid large effects and blinding lights. Keep the HUD minimal and first-person weapons from covering too much of the screen. The Mercenary's two viewmodels are a stress test for keeping targets and incoming threats visible.
- Maps should be visually simple enough that characters, shots, and objectives stay readable. Candidate settings include Siberian refineries, small American towns, Canadian lumber operations, British naval ports, and Aztec ruins in Mexico. Specific maps and how these regions fit into the world remain open.
- Use Attackers and Defenders as provisional side labels where applicable. Employers, factions, and the reason for the conflict remain undecided.
- Humor is welcome where it preserves gameplay. Art, effects, and presentation must preserve combat information and control. In a conflict between a visual flourish and combat readability, gameplay takes priority.

Prototype checks proposed by the assistant: identify classes at representative fighting distances from front, side, and back while moving. Test green and purple team treatments against snow, timber, refinery steel, and desert stone, including under color-vision simulations. Keep broad map shapes legible and reserve dense detail for areas that do not compete with combat signals.

## Illustrative mechanics not committed designs


### The Mercenary

The Mercenary is the face of the game and the reference class for tuning combat. A scrappy, mobile glass cannon, he contributes through damage and uses a left-hand shotgun and a right-hand grenade launcher simultaneously. He should be an unusually reliable damage dealer and a strong independent duelist. Whether he proves strongest overall depends on the rest of the roster and playtesting. His specific visual design and setting details are open. His visual design should follow the shared art direction above.

#### Baseline kit

| Element | First tuning pass |
| --- | --- |
| Shotgun | Left click. Hitscan, six shells, five pellets in a tight fixed pattern at 15 damage each, up to 75. High recoil, slow fire, relatively quick reload. |
| Grenade launcher | Right click. Three rounds, deterministic projectile path, contact detonation on surfaces, 50 direct damage and 15 splash damage with a small radius. Low recoil, very slow fire, long reload. Confirm whether 50 is the total direct-hit damage. |
| Melee | Shared quick melee input that swings a weapon or stock without switching. Damage and timing open. |
| Health and recovery | 150 HP, average body size, no innate resistance. Shared regeneration is 10 HP per second after 10 seconds without taking damage. Health pickups exist in the example fight. |
| Movement | Medium walking speed. Shared crouch, jump, slide, and lateral leap. Slide and leap target the same distance; the leap is faster with a more predictable trajectory. His launcher enables a self-damaging grenade jump. No other unique movement or special ability is specified. |

Prototype with independent firing and 50 total damage on a direct grenade hit. Test point-blank dual fire for easy lethal burst. Same-frame scheduling and reload interactions remain open. Range, falloff, projectile speed, cadence, self damage, recoil behavior, and pellet geometry need tuning. Air movement, melee, and cross-weapon interactions should preserve deliberate shots.

#### Combat and counterplay

He pressures midrange with grenades and converts openings with the shotgun. At long range he can threaten static positions with explosives, but opponents should be able to dodge visible shots. At close range the grenade becomes easier to land but risks self damage, and the tight shotgun pattern offers little forgiveness for a tracking error. The shotgun is still dangerous up close. A coordinated attack or pressure after he spends his ammunition should force him to retreat or take a bad fight.

Example with corrected ammo accounting: fire one grenade across a choke, land shotgun hits as the opponent closes, spend the second grenade to jump after the retreating target, and use the third to finish. He then has an empty launcher and reduced health. A slide into a lateral leap helps him retreat. Remaining shotgun shells still matter. Expert play involves leading the grenade, snapping to the hitscan target, choosing when to spend health and a round on movement, and routing the exit.

#### Proposed progression

| Level | Nate's proposal |
| --- | --- |
| 2 | Previous safety-net upgrade removed. Replacement upgrade remains open. |
| 3 | Reduce grenade self damage by 15%. |
| 5 | Reduce reload time by 15% for both weapons. |
| 6 | Remove shotgun recoil. |
| 7 | Increase direct grenade damage by 20%. |
| 8 | Increase shotgun pellet damage by 10%. |
| 10 | Double grenade splash radius, increase shotgun pellet count from five to eight, and reduce grenade self damage by an additional 50%. |

With 50 total direct grenade damage, a full shotgun hit plus a direct grenade deals 125 at baseline and 142.5 at level 8. Neither kills a healthy 150 HP Mercenary. A baseline full combo can kill a 125 HP minimum-health opponent. Two baseline full shotgun hits deal exactly 150. Single-click weapon damage must remain below 125 against full-health opponents unless an explicitly approved exception applies.

Accepted first prototype: retain the five-pellet core and test three weaker outer pellets at level 10. Three 5-damage outer pellets plus five 16.5-damage core pellets give a maximum of 97.5, below the roster minimum of 125 HP. The lower damage candidate remains accepted until revised. Test before simultaneously adding radius and self-damage upgrades. Confirm how self-damage reductions stack and whether jump impulse is independent of self damage.

### The Behemoth

Reworked on November 16. His main weapon is a massive shotgun. The previous hook, jab, and uppercut weapon kit is superseded. Nate wants to avoid movement or control locks during attacks. Fist animations do not inherently require such locks, but the shotgun is the selected direction.

He retains his enormous frame, largest hitbox, 250 HP, fast base movement, 50% knockback resistance, and two charges of jump, leap, and slide. Movement chains and the shared no-fall-damage rule apply. His proposed identity remains a close-range tank who protects individual teammates by intercepting attacks. His previous background is a professional boxer in his mid twenties, 7 feet 6 inches tall and about 400 pounds.

| Element | Current direction |
| --- | --- |
| Shotgun | Four shells, five pellets at 20 damage each, 100 maximum per shot. Much wider spread than the Mercenary, heavy impact, slow firing, fast reloading. Pattern remains deterministic under shared rules. Exact cadence, reload style, spread angle, and falloff remain open. |
| Guard | Retained dedicated hold input, with E or Q candidate bindings. Reduces frontal incoming damage by 50%. Three seconds of guard resource and initial 75% movement slow. Partial meter recovers faster than fully drained meter. |
| Movement | Retained fast base speed and double charges for jump, leap, and slide. Exact charge recovery remains open under the shared rules. |
| Quick melee | Universal quick melee remains. Its specific animation and damage are open. |

The proposed 100-damage meatshot is compatible with the confirmed 125 HP roster minimum. It leaves a minimum-health opponent at 25 HP and a 150 HP Mercenary at 50 HP. Two full shots kill either. Three full shots deal 300 and kill an unguarded 250 HP Behemoth. The earlier recommendation to reduce it to 80 damage is withdrawn because it relied on superseded lower-health assumptions.

Guard interaction is open. Assistant recommendation is to test guard interrupting reload and preventing firing, without resetting the time until the next shot. This preserves deliberate cadence and tests whether guarding between shots otherwise eliminates the intended punish window.

Progression needs a new pass. The shared level 2 safety net is removed. Previous guard-related proposals at levels 5 and 6, movement contact at level 8, and level 10 cooldown changes remain references to revisit rather than automatic transfers into this weapon kit. The old fist combos and uppercut final-blow reset do not apply to the current shotgun weapon.


### Combat rules from questionnaire 1 to 23

- A clean duel should usually end in two to four well placed hits by the better player. A healthy opponent in reasonable position should normally survive the opening exchange and have a chance to use movement and weapons. Low health and severe positional errors can shorten a fight.
- A fair lethal combo comes from a readable threat that both players can account for and act on. Awareness can come from positioning or audio before line of sight. Awareness alone does not justify an attack that removes the defender's ability to contest the fight. The Counter-Strike example illustrates mutual awareness and stakes. The game's restriction on easy one-taps still applies.
- Close range is the center of combat, roughly five to ten meters. The effective range ceiling is the outer edge of midrange. No dedicated snipers or long-range classes. A projectile can occasionally travel farther. No baseline headshot bonuses. A conditional bonus from an ally or a level upgrade is a possibility and would need to respect lethality rules.
- The general character maximum-health floor is 125 HP. The Mercenary has 150 HP. Nate's later Shaman proposal explicitly creates a 100 HP exception, to evaluate with his kit. Earlier 85 HP classes are superseded. This is not a global health rescale.
- No stuns or slows imposed on opponents. A weapon or ability can apply small knockback as an explicit exception. Ordinary damage never interrupts movement or applies knockback by default. Voluntary guard slowdown remains part of the Behemoth's guard.
- Allies pass through one another and enemies collide. Friendly players intercept fire at close range and shots pass through them at midrange or farther. Exact distance measurement and how this applies to each shot type are open. Preserve the Behemoth's role intercepting enemy fire and test muzzle overlap with a friendly Behemoth.
- Universal quick melee is fast, low commitment, and low damage. It can chip or finish a wounded opponent. A backhand faster than the Behemoth's jab is illustrative.
- Every class has jump, crouch, slide, and leap. Wall climbing and wall riding are class specific. Momentum retention and bunny hopping are intentional experiments, including leap into slide into chained jumps. Air reversal and the separate uses of leap and slide need playtesting.
- Slide and leap charges recover with time. Jump resets on landing and crouch on key release. Slide requires jump and crouch. A Behemoth who spends both jumps and both leaps can stay airborne but cannot start an air slide without a jump available. Exact costs, cooldown timing and speed limits are open. Movement actions allow combat inputs and do not impose action locks by themselves. Weapon and guard interaction rules still need definition.
- All in-bounds drops are valid with no fall damage. Clearly marked death zones and barriers define the playable boundary.
- Shared passive recovery is 10 HP per second after 10 seconds without taking damage. Place plentiful health packs for convenient recovery. Avoid making individual packs the main territory worth contesting. In-combat healing is very small and can reward a well-timed skillshot by letting an ally survive an extra hit.
- Support utility is broader than healing. Illustrative concepts include syringes granting brief lifesteal, flat damage reduction, or jump height; a small healing ring roughly three character widths across; and a low-damage skillshot that briefly reveals a target. Dedicated and other classes can supply utility. Supports should be optional, with coordinated teams likely gaining more value than casual teams. Lifesteal, damage reduction, and stacked utility need the same combat breakpoint checks as healing.
- No duplicate classes on a team. Class switching has a proposed grace period of about 15 seconds before prior progression is erased. The stated misclick use case suggests restoring progress by switching back; whether any score is active or transferable during that interval is unresolved. Progression here refers to score-based progression.
- Provide pings for locations, enemies, health packs, objectives, and calls to push, defend, regroup, or retreat.

### The Shaman

Third reference character. A short, small-framed, cloaked Siberian doctor with 100 HP, medium base speed, universal movement, and no special escape or resistance. The 100 HP is Nate's explicit roster-floor exception. Mercenary remains 150 HP. Formulas and injections are medical and chemical. He is intended to become an enemy priority target through unusually impactful support.

Nate explicitly retains all five formulas, including X. His clarification supersedes the apparent veto of X. Enemy movement slowing and damage punishment are deliberate Shaman-specific exceptions to shared control rules. The ally and enemy functions are retained. Nate accepted the numerical retune and safeguards below as the current prototype. Balance remains untested.

#### Retuned syringe system

Fast, large, strongly team-colored projectiles with a short readable tracer that exposes the firing direction. Syringes intentionally collide with allied targets at all relevant ranges. Initial reload is 0.5 seconds under the latest timing decision. Each formula has one shared eight-second cooldown across chambers, beginning on firing. A miss spends it. The level 8 upgrade reduces this to seven seconds. The two chambers may fire different formulas together. Loading a second chamber does not bypass a formula cooldown. Exact binding, chamber refill behavior and physical projectile speed need testing.

Base allied healing and enemy damage on a direct hit are both 10. Extra H values add to this. A activates on terrain and supplies its area effect without an additional direct-hit bonus on terrain. Direct character-hit behavior for A remains open.

| Formula | Ally effect | Enemy or terrain effect | Base duration |
| --- | --- | --- | --- |
| M | 15% movement speed boost | 10% movement slow | 2 seconds |
| H | 15 extra instant healing, then 5 HP per second | 15 extra instant damage, then 5 damage per second | 3 seconds |
| L | Heal for 50% of actual enemy health damage dealt, capped at 60 healing per application | Take self damage equal to 25% of actual enemy health damage dealt, capped at 30 self damage per application | 2.5 seconds |
| A | Terrain zone heals allies at 8 HP per second | Same zone damages enemies at 8 HP per second | 5 seconds |
| X | Multiply positive score earned by 1.25 | Multiply positive score earned by 0.8 | 3 seconds |

Assistant safeguards: different formulas coexist. A repeated same-type effect refreshes rather than adds magnitude, and never stacks X multipliers. Refreshing L does not reset its total budget while the prior application remains active. No overheal. L excludes environmental damage, self damage, reflected damage, and object damage from its triggering basis. L self damage cannot trigger another reflection or lifesteal event. X does not change death deductions or score already awarded. X grants no extra utility credit merely for multiplying score. Support credit remains outcome based. For A, start at a two-meter radius, require line of sight from zone to occupant, and allow one active zone per Shaman. These prototype constraints were accepted by Nate.

A speed debuff should not instantly delete accumulated velocity on impact. Test reducing movement capability while retaining responsive steering and readable slowdown. Its existence remains a deliberate exception to the normal no-slows policy.

#### Retuned progression

| Level | Prototype upgrade |
| --- | --- |
| Baseline | Self injection using a loaded shot and the formula cooldown. Moving this from level 2 is an assistant choice under the simplification. |
| 3 | Earlier reload upgrade superseded by 0.5-second baseline. Proposed replacement is five-second passive regeneration delay. |
| 4 | Base direct healing and damage increase to 15 |
| 6 | Add 0.5 seconds to formula durations |
| 8 | Formula cooldowns become seven seconds |
| 9 | Second chamber allows two different formulas together. Shared formula cooldowns still apply. |
| 10 | Add another 0.5 seconds to durations, replacing the original three-second increase |

For stronger passive recovery, first candidate is 10 HP per second after seven seconds without damage, rather than the shared ten-second delay. Normal combat interactions reset the delay. This is a prototype choice.

H totals 40 at baseline and 50 at level 10. A totals 40 per occupant at baseline and 48 at level 10. Level 10 durations are M three seconds, H four seconds, L 3.5 seconds, A six seconds, and X four seconds. Formula cooldown exceeds maximum duration, leaving a natural coverage gap. Baseline simultaneous H plus L heals 50 immediately, then H can restore 15 and L at most 60, for up to 125 over the application. At level 10 that combination can restore up to 145. This strong combined save spends both chambers and both formula cooldowns, and still needs focus-fire testing.

The Behemoth's 100-damage shotgun grants 50 healing under allied L. A 125-damage Mercenary combo grants the capped 60. Under enemy L, that combo costs the attacker the capped 30 HP. Marked targets must be able to identify L through a compact status cue and sound within clarity constraints. Ability selection should preserve movement. Overall power and whether he becomes mandatory remain playtest questions.


## Detailed gameplay specification November 19

This section records Nate's answers to the implementation questionnaire and fills delegated gaps with assistant defaults. Explicit user decisions override earlier sections. Assistant defaults are reversible and untested. Where interpretation materially changes behavior it is labeled.

### Input and camera

User bindings: WASD movement, Space jump and repeated Space leap, Control crouch, Command slide, V quick melee, E guard for Behemoth or self injection for Shaman, R syringe selection, G ping, Tab scoreboard. Reload is automatic. All controls are rebindable. Crouch and guard support hold and toggle. Holding fire fires at the next available interval. Holding jump jumps on landing. Focus loss releases held actions.

User requests indefinite input availability and earliest received action precedence. Assistant implementation default: held actions persist while held; discrete one-shot actions buffer for 0.2 seconds only. Release or cancel clears pending actions. Menu interaction cannot cause an accidental shot on closing. Use locally responsive input with authoritative validation, preserving each player's input order rather than deciding cross-player outcomes by network arrival order. Simultaneous incompatible inputs use a deterministic priority to define during implementation. Ordinary firing, reloading, and movement can overlap where explicitly allowed.

A short, rate-limited unavailable-action sound replaces repeated empty spam. Default FOV is 90 with a 50 to 100 range. Horizontal FOV at 16:9 is the assistant convention. Ultrawide preserves vertical framing and exposes additional sides; viewmodels remain tied to a centered reference frame. HUD horizontal bounds are customizable. No ADS or movement accuracy penalty. Recoil is visual weapon motion only. Camera bob, shake, landing motion, and damage shake are off by default and optional.

Camera remains at the character's physical head position without decorative bob. Crouching must lower the camera with the head so camera and hitbox do not separate. Behemoth has higher sight lines. Viewmodels render separately from world geometry and support position, scale, handedness, animation-intensity, and hide controls. World shot obstruction must still be checked independently. Default crosshair is a small red outlined circle with extensive customization.

### Weapon timing and hit rules

User firing intervals: Mercenary shotgun 0.65 seconds, launcher 0.75, Behemoth shotgun 0.7, syringe gun 0.5. Deploy takes 0.25 seconds. Reloads start during recovery, with the later timer controlling availability. Ammunition becomes usable at the defined timer boundary. Completed loading is preserved on interruption. Quick melee and self injection have recovery; guard release and selection menu do not add a deploy penalty. Respawn resets weapon timing and loads ammunition as an assistant default.

Reload interpretation is unconfirmed: Nate gave 3.5 seconds for six Mercenary shells, four seconds for three grenades, two seconds for four Behemoth shells loaded in pairs, and 0.5 seconds per syringe. Assistant first-pass interpretation uses full-empty reload totals for the shotguns and launcher, giving one shell every 3.5/6 seconds, one grenade every 4/3 seconds, and one Behemoth pair each second. If Nate intended each insertion to take the listed time, these need changing. Both Mercenary weapons can reload simultaneously and independently. Firing one does not cancel the other's reload. No separate reload-priority input. Syringe chambers can load together in 0.5 seconds and fire independently with the shared 0.5-second firing interval; an intentional combined shot can discharge two different formulas together.

Shots originate at the camera. Assistant default checks muzzle-to-aim obstruction to prevent shooting through nearby cover. Starting inside a character counts as a hit except friendly overlap blocks ordinary weapon fire. Starting in a wall gives no hitscan result and triggers terrain behavior for projectiles. Same-tick combined damage and mutual kills are allowed. Projectiles persist after the shooter dies.

All shotgun patterns are deterministic plus signs with a center pellet. No damage falloff. Eight-pellet Mercenary level 10 pattern is unresolved because a symmetric centered plus needs equal arm counts. Current 97.5 maximum remains the damage target pending a pattern decision. Grenades are slower, heavier under gravity, and larger in collision than syringes. Both have a ten-second candidate lifetime. They pass through one another and pickups, and may fit through openings smaller than the visible model if their collision shape fits.

Explosions apply fixed damage within radius if world geometry allows line of sight from the blast to the target. No splash falloff and no through-wall damage. Use multiple target sample points as an assistant default to avoid an exposed limb being treated as full cover. Source surfaces need a small outward offset for blast queries. Grenade direct damage remains 50 total rather than 50 plus splash. Self damage equals the 15 splash damage, independent of distance inside the bubble. Mercenary reductions add to 65% at level 10. Source damage buffs scale damage and jump impulse together, while self-damage resistance reduces health cost only as an assistant interpretation. Internal health and damage preserve fractions, including 50% packs for odd health totals; display rounding must not change breakpoints.

### Movement and collision

Base speed order is Shaman slower than Behemoth, with Mercenary substantially faster than both. All use the same fast acceleration, braking, and direction reversal values. Diagonal speed is normalized. Jump holding preserves momentum and auto-hops. Grounded travel without hopping dissipates excess speed over about one second. Hops preserve rather than generate speed. No hard speed cap. Passive loops must not generate unlimited acceleration. Source of additional speed from leap and slide remains to tune.

Crouch halves target walking speed. Slide has its own input, no longer costs a jump, and can start stationary. It follows a committed ground direction, travels the same lateral distance as leap, and is faster. Leap is lower than jump, works sideways and backward, and remains steerable. Jump is shorter laterally than slide or leap and recovers faster. Camera turns do not redirect existing momentum. Voluntary cancel retains momentum. Damage does not interrupt movement.

Assistant slope default: follow walkable ground, climb steps up to a quarter of jump height, preserve tangent speed over small surface changes, and apply gravity on ramps without allowing a repeatable uphill acceleration exploit. Steep slopes count as airborne. No wall or enemy jump resets. No coyote-time grace. A 0.15-second prelanding jump buffer is a default. Jump resets on valid ground. Enemies can support players physically but do not refresh jump charges.

Slide and leap have a three-second initial recharge delay followed by three seconds to refill, six seconds total for the first charge. Recharge pauses airborne. Behemoth recharges sequentially. Assistant default for the second missing charge is another three seconds after the first refills; whether every charge needs six is open. A slide leaving a ledge becomes airborne travel, retaining momentum. Cancels and transitions can produce fast leap routes but must not duplicate impulse without spending a charge.

Allies pass through each other, enemies collide, and colliding into an enemy stops travel. Enemy body traps are allowed. Users can stand on enemies, not allies. Friendly hitboxes intercept ordinary fire close to the shooter. Assistant distance default is two meters from shot origin, using the whole hitbox. Inside this distance ordinary projectiles disappear with a compact fizzle, and outside it pass through allies. Shooting while overlapping an ally is blocked. Friendly fire does no damage. Enemy and support-hit targets use first valid collision, including overlapping Behemoth interception. Syringes intentionally collide with allies at all ranges.

### Recovery and statuses

Shared packs restore 50% of maximum health, cap at full health, and respawn five seconds after use. Full-health players cannot consume them, and packs do not cleanse. Proposed plus-one-level crystals on ten-second respawns remain an idea; assistant recommendation is to defer them because they change map incentives and score pacing.

Enemy and self damage reset passive recovery. Attacking does not. Damage-over-time ticks count as damage. Damage reduction stacks; assistant default is multiplicative with an overall 75% reduction limit, avoiding additive invulnerability. Status strength snapshots on application. Class switching removes statuses and ends effects sourced by the switching player as an assistant anti-exploit default. Spawn entry cleanses debuffs. Death does not shorten effects already cast on other players. Respawn itself is free of statuses.

Repeated doses add duration without increasing magnitude. Assistant duration cap is twice a single fully upgraded application, to prevent indefinitely banked effects. L's per-application budget does not reset just because duration is extended. Each status has a distinct subtle sheen; multiple status feedback must remain legible without covering the character or obscuring team color.

### Behemoth defaults and progression proposal

Guard prevents shooting, reloading, and quick melee. Raising takes 0.15 seconds before mitigation. Assistant release default is immediate. Guard covers the front of the whole body, using a 120-degree horizontal cone as a first-pass angle. Frontal explosion mitigation applies when the projectile hit the guard even if splash wraps behind it, per Nate. Source-based explosion guard checks still need exact implementation.

Assistant meter defaults: three-second capacity; refill begins 0.75 seconds after releasing a partly spent guard at one guard-second per real second. Full depletion waits 1.5 seconds, then fills at half that rate. No artificial minimum tap cost. Raising consumes actual time held, including raise time. Guard changes acceleration and target speed, preserves airborne momentum, cancels an active movement action, and returns grounded travel to its guarded speed target. No outgoing action resets or shortens firing recovery.

Replace lethal-save bookkeeping with actual damage mitigated. Remove movement-contact damage. Nate wants a small, purposeful redirection ability to protect teammates. Assistant candidate is a frontal shoulder check on right click, six-second grounded cooldown, one-meter reach, no stun or damage, and one horizontal impulse away from the Behemoth. It changes the approach direction while retaining speed up to the greater of incoming horizontal speed or base movement speed. It cannot add vertical height, acts on one target, does not refresh movement charges, and cannot be used while guarding. This is a prototype suggestion, not accepted kit.

Suggested Behemoth progression, replacing old fist-dependent upgrades: level 3 guard capacity 3.5 seconds; level 5 guard slow 50% instead of 75%; level 6 mitigating 75 actual enemy damage within two seconds grants 10% speed for two seconds, once per eight seconds; level 8 empty reload time 1.7 seconds instead of two; level 10 guard capacity four seconds and level 6 speed effect lasts three seconds. Level 4, 7, and 9 can remain empty. No automatic damage or fire-rate buffs in this first pass. Mercenary's obsolete recoil-removal level 6 needs replacement; assistant candidate reduces slide and leap refill stage from three to 2.5 seconds.

### Shaman specifics

Selection uses shortcuts without taking mouse aim. Formula choice survives shots, deaths and respawns. Self injection can use one chamber while the other fires outward. Both chambers share a forward trajectory when discharged together. A hitting a character only applies its base direct effect. Terrain zones can attach to walls, ceilings, slopes and payloads; line of sight and radius move with their attachment. One zone remains the limit.

L recoil can kill its recipient. It responds to actual enemy damage from shots, melee, and damage-over-time, including A. Self and reflected damage never qualify or recurse. Enemy guard reduces direct syringe damage but does not weaken or shorten its status. Ally guard does not weaken support. M affects acceleration and movement speed targets without deleting explosive momentum.

X evaluates positive score when awarded, including from projectiles fired before the status was applied. Different doses extend duration without multiplying percentages. Formula cooldown still starts at firing, and a second chamber does not bypass it.

The new 0.5-second syringe reload supersedes the old four-second value. The original level 3 half-second reload reduction would reach zero. Assistant replacement is passive regeneration delay five seconds rather than seven. Other accepted Shaman progression stays unless later tuned. This creates much faster delivery of different formulas and needs playtesting as a major pacing change.

### Score, class selection and HUD

Working assistant decision under Nate's suggestion: deaths no longer deduct score. Levels rise from contribution score and reset at a side swap or new match, carrying between same-side rounds. Level 10 surplus is discarded for progression. Upgrade thresholds from level 1 are increments of 2,2,3,3,4,4,4,4,4, giving cumulative 0,2,4,7,10,14,18,22,26,30. Level 10 therefore begins at 30, not 29. Level changes occur independently of actions. Assistant default preserves already-running weapon timers and applies new values on the next action. Increasing maximum health does not heal current health.

Assistant class-switch rule: switch only dead or in spawn, new class begins at zero score, old class score held in escrow for 15 seconds. Returning restores it with no transfer of score earned on the temporary class. Leaving spawn ends the undo grace. No simultaneous stored progression across multiple swaps. Team duplicate cap remains one unless Nate accepts the proposed change to two per team; two for the entire server is ambiguous.

Late join score is the mean of current teammates' progression score. Assist default is at least 15% of victim maximum health damaged within 15 seconds, or a support effect that measurably helped the killer within that window. Full recovery clears prior enemy damage contributions. Credit once per player per death, and track healing of self-inflicted damage separately so it cannot produce healing score. Useful speed credit can trigger if the ally damages an enemy, contests an objective, or receives enemy damage while buffed; award once per application with no score-recursive X credit. Apply X to the positive award after all kill bonuses are calculated. These remain assistant defaults.

HUD includes health, loaded ammo, buffs/debuffs, ability and movement resources, score, current and next upgrade, and objective state. Damage numbers and separate hit/kill feedback are enabled. Enemy health is hidden. Scoreboard lists kills, deaths, assists, K/D, damage, mitigation, score, level, and class, and remains accessible while moving. K/D at zero deaths displays a dash as a default.

### Objective and respawn defaults

KOTH enemy touch within five seconds of the owner's timer ending, neutral ownership, or partial capture triggers overtime. Untouched fully owned point wins when its clock ends. Assistant resolution: contest pauses the clock as before; last-five-second touch memory triggers overtime when it reaches zero later. In overtime, the expired-clock owner wins after clearing contest and securing full control for one second. If the opponent captures, their remaining ownership clock resumes. Capture alone does not erase their remaining timer. This is an assistant resolution of the still-open sudden-death question.

Capture default times for one through four players are eight, six, four, and three seconds; more players do not accelerate further. Airborne players contest within a ground-to-Behemoth-height volume. Payload defenders contest from a larger radius than escort range. Assistant initial radii are two meters escort and three meters contest, with line-of-sight checks. Speed candidates are 0.8, 1.1, and 1.4 meters per second. Rollback starts 15 seconds after last attacker touch, at 0.2 meters per second, and cannot cross a completed checkpoint. Payload healing is 10 HP per second within escort range, without the out-of-combat delay, as a first-pass interpretation.

Attack Defend compares both-point completion first, then completion time if both finish. If both fail, use completed points as tiebreaker, then a short mirrored tiebreaker on equal points. Capture A remains a lost attacking round if B is not captured, but completed A can rank tied failed attacks. This is a recommendation requiring confirmation because Nate previously described A as having no winning credit. Partial progress drains after a two-second absence at twice normal one-player capture speed as a default.

Grouped respawns use a two-second death clustering window and an eight-second target wait, never delaying anyone beyond ten seconds. A checkpoint relocates pending attackers to the new spawn. Spawn immunity is three seconds maximum outside the room, ends on offensive fire or enemy-affecting abilities, cannot be refreshed until a new respawn, and prevents self damage while leaving jump impulse intact. Immune players cannot capture, contest or escort until immunity ends. Ally support actions end exit immunity as well. Spawn boundary blocks all hostile effects inward and outgoing attacks from protected occupants. These are delegated assistant defaults.

## Playable prototype planning

Nate accepted the November 19 recommendations and authorized routine tuning of flagged issues. Reload times use full-empty totals, the syringe reload is 0.5 seconds, deaths deduct no score, the proposed Behemoth progression and shoulder check are accepted as prototype candidates, and the detailed delegated defaults form the current working specification. Values remain subject to testing rather than evidence of balance.

Nate wants to develop using Cursor. Unity 6.3 LTS with C# and URP is now the selected working stack. Exact patch and package versions remain to pin. No project or repository has been created. The assistant's preliminary recommendation is Unity with C# and URP for a desktop prototype. Godot with C# is a reasonable alternative, and Unreal with C++ and limited Blueprint setup is another option. Final choice should account for hardware, development experience and testers' operating systems.

Proposed scope: one graybox testing space, a small hill arena, simplified team-colored class bodies, temporary first-person weapons, and health/resource/status feedback. Start with the Mercenary movement and weapons offline, then prove a two-player server-authoritative combat path before building all class interactions. Grow to all three classes and a 3v3 private hill test. This tests character roles without requiring fourteen players. Full 7v7 remains the design target.

Recommended milestones: movement and damage sandbox; responsive two-player combat under simulated latency; Behemoth guard and Shaman formula tests; 3v3 hill round with group respawns; then score progression and level 10 tests. Use manual level controls during early tests. Bots and target dummies can test behavior mechanically but do not establish the quality of PvP combat.

Build gameplay numbers as editable configuration. Separate input, movement, weapon timing, hit resolution, statuses, score and game mode enough to tune them independently, without a generic framework for hypothetical future classes. Use an existing maintained transport/netcode solution after checking prediction and reconciliation support against the desired movement. Editor integration does not guarantee production-quality FPS networking.

Use short Cursor tasks with observable acceptance criteria, source control checkpoints, pinned engine/package versions, and project-specific rules. Prioritize low visual clutter, broad geometry, temporary meshes, and informative sound. Do not require finished graphic-novel materials or custom animation to validate movement and gunfeel. Internet hosting, discovery, matchmaker, ranked systems, final art, and additional modes can be scheduled after core private combat is proven.

### Retro 3D visual implementation brief November 19

Nate's latest direction favors low-poly character and environment models, visibly pixelated textures, and modern lighting and shading. DUSK is the main execution reference, with Quake and original Half-Life also informing the visual language. Watchmen remains an illustration and shape influence rather than requiring a complex illustrated rendering pipeline. Modern lighting should support clear silhouettes rather than conceal opponents. High FOV, small viewmodels, minimal HUD, small explosions, manageable status effects, and restrained post-processing remain priorities.

Ray tracing is an aspiration, not a selected technical requirement. Low geometry cost does not eliminate ray tracing cost. Assistant recommendation is native-resolution geometry edges and HUD, low-resolution textures, baked environmental lighting, probe lighting for moving characters, and limited real-time shadows. If Unity is chosen, URP remains the proposed baseline. Global pixelation, expensive bloom, and ray tracing are not required to establish this look.

- Behemoth visual direction: huge male frame built from large solid forms, a broad trench coat, slacks, worn boots, combat gloves, simple hair, and an enormous shotgun. Minimal outfit detail. The guard needs a readable arms-and-weapon pose compatible with its existing gameplay coverage.
- Shaman visual direction: one of the shortest characters, thin and spindly but physically capable. Woodsman, hunter, and druid influences in clothing and shape. Weapon is a mechanical crossbow firing syringes, replacing the modern-medical gun appearance. Existing formulas, projectile behavior, chamber logic, and timing remain the gameplay baseline. Supernatural magic is not implied by this clothing direction.
- Mercenary visual direction: around 18 years old, stocky and muscular, fast delivery-boy energy. Scarf, boots, cargo pants, and scrappy streetwear. Personal wear is now welcome for this character while avoiding excessive small costume details. Dual weapons remain his defining gameplay equipment.

Assistant first art test: build a small lighting scene, a corner of the hill map, and three simple silhouettes in both team palettes. Assess each character during run, jump, guard, reload, and status application at typical fighting ranges. Test snow, timber, metal, and stone backdrops with consistent exposure. Use restrained material roughness and texture wear. Large translucent status sheens need evaluation against the renewed clarity priority.

## Development decisions from hardware and first-build answers

Nate develops on a recent MacBook; exact model, chip, RAM and macOS version remain unknown. He has access to university gaming PCs with RTX 5090 GPUs. Those machines' software installation permissions remain unknown. The intended lower-end GPU target is RTX 2060 class. A specific baseline CPU is not established. Nate knows SQL, Python, Java, C#, and Git, has a little Unreal and Godot experience, uses paid Cursor, and has no project or repository yet. He can perform setup and editor changes, wants Cursor to do most implementation, and will supply extensive playtest feedback.

Engine selected by the assistant under Nate's authorization: Unity 6.3 LTS, C#, URP, native desktop development. Pin an exact supported stable patch and compatible packages during setup. The choice prioritizes existing C# knowledge, Mac development, editable gameplay, and the low-poly visual target. This is not a claim that an editor is installed or a project exists.

First Cursor deliverable is offline Mercenary control in a small graybox arena with HUD, temporary first-person viewmodels, movement chains, hitscan shotgun, grenade projectiles, reloads, damageable targets and basic feedback. Movement and weapon feel have priority. Basic geometry is accepted.

First multiplayer deliverable is two players and must include score-based leveling, relevant upgrades, kill/respawn flow and score display. King of the Hill is the first full mode. Simple manual-level tools can coexist with live progression for diagnostics. Networking package and internet join path remain undecided.

Before a friend-facing test, establish an actual representative visual treatment for an arena segment and Mercenary equipment. Final character artwork is not required, but an unstyled blockout alone does not meet Nate's stated sharing condition. Internal networking tests may occur before that art pass.

Proposed performance target to confirm is 1080p at 120 frames per second on RTX 2060 class hardware in a standalone build. Profile on the Mac separately, naming actual model and render resolution. Access to high-end GPUs does not establish minimum-spec performance. Next needed information is the Mac model/chip/memory and testers' platforms, count and connection requirements.

## First sandbox delivery and later LAN requirements

Nate explicitly prioritizes an offline, runnable first pass before multiplayer or outside testers. Use Mercenary only, a minimal menu, a small graybox test map, HUD, static temporary viewmodels, movement, hitscan fire, visible projectiles, automatic reloads, resets and basic target feedback. Custom 3D models and finished animations are not requirements. Placeholder audio, weapon models and UI may come from asset libraries after checking usable licenses and attribution. A simple custom HUD may be easier than adopting a complete unrelated UI framework. Temporary geometric weapons remain acceptable if a suitable licensed asset is unavailable. Work toward a coherent visual treatment before friend-facing tests.

Target Windows and macOS only initially. Keyboard and mouse defaults, windowed at 1920 by 1080, with an assistant default of fitting to the usable screen when that size is unavailable. Distribute platform-specific ZIPs. Windows package contains the executable and its required companion files. macOS package contains the application bundle. Engine source project is also a deliverable. Signing, macOS launch handling, and cross-platform build verification remain setup tasks.

Minimal initial menu: Play Sandbox, Settings and Quit. Save readable versioned JSON settings in the platform's per-user persistent data location outside the extracted game folder. Keep user preferences separate from balance definitions and temporary console overrides. Preserve settings across new builds, validate values, handle missing or malformed keys, and fall back to defaults. Scene reset clears temporary overrides unless explicitly saved to a development preset.

Developer console exists offline first and becomes server-authoritative in multiplayer. Initial commands should include help, kill, respawn/reset, set health, set outgoing damage multiplier, set level, spawn/reset/remove targets, and show movement statistics. Timed health/damage overrides should have explicit optional durations. Assistant syntax proposals include help, kill, respawn, reset, set health 150, set damage_multiplier 1.5 10, set level 8, target spawn, target clear, debug movement on. Reset restores baseline stats and resources at spawn; respawn follows ordinary death/respawn behavior. Advanced flight, noclip, and through-wall developer visualization are later developer tools for this project's own sandbox. In multiplayer only the host can enable cheats or grant permissions, with clear notification; remote clients cannot authorize changes themselves. Do not evaluate arbitrary code as console input.

Later multiplayer is initially LAN, up to six players, player-hosted with the host playing. First networking proof remains two players and includes score-based leveling. Invite code is preferred. Assistant implementation sequence starts with a simple LAN address join, then adds session-code convenience. Codes on one LAN can use local discovery or encode connection information; internet-wide codes require a separately chosen lookup, lobby, or relay path. No hosting expense needed for offline scope. Testers may configure port forwarding if later needed.

Allow mid-round joins. Use the earlier default of team-average progression for late joiners. Assistant reconnect default reserves a disconnected non-host player's team, class and progression for 60 seconds and restores them in spawn if reconnect identity is verified. No initial spectator requirement. Use exact compatible build/protocol checks during connection. Notify host of incompatibility, also give the joining client an actionable message, and reject the connection rather than silently running incompatible versions.

If the host leaves, the running session ends. Nate would like an explicit promote-to-host option later. Preserving a live session across promotion is a separate host-migration feature. Assistant scope keeps that feature deferred; a planned handoff can restart a fresh lobby with a new host until actual state transfer is implemented. Do not describe simple menu role changes as completed live host migration.

First-build acceptance: clean project opens, menu launches sandbox, character moves and aims, both static weapon models render, shots and grenades work, HUD updates, auto reload follows configured timing, damageable targets respond, kill/reset commands recover the player, settings survive relaunch, and a native standalone build launches. Windows and macOS builds must each be tested on their respective systems before claiming both work. No multiplayer functionality is required for this first milestone.

## Design risks and hypotheses

These are issues to investigate, not vetoes or agreed solutions.

- Progression can compound a lead. Losing score on death softens a reset but does not by itself prevent stronger players from gaining further advantage.
- A death penalty may encourage cautious play, disengagement, or avoiding objectives. Check whether progression supports the intended fast pace.
- Final blow scoring may reward kill stealing; solo bonuses may discourage cooperation. Attribution must be judged against the teamwork goal.
- Small hitboxes, evasive movement, and slow firing can combine into frequent misses and inconsistent encounter duration.
- Trap setup vulnerability only functions as a cost if maps and combat actually let opponents exploit it. Safe preparation positions may bypass the intended tradeoff.
- Fixed spawns can be contained even if players are protected inside them. Exit design matters alongside immunity.
- Weapon upgrades may change damage or reload breakpoints enough to make an otherwise fair matchup oppressive.
- Gunfeel, difficulty, and fairness require playable testing; written rules alone cannot establish them.

## Open questions

1. Baseline combat: tune firing intervals, health ranges, weapon switching, and recovery around the two-to-four-hit duel target. Baseline headshot bonuses are excluded.
2. Movement: shared controls versus class abilities; acceleration, braking, air control, momentum, and readability.
3. Score accounting: mutually exclusive kill categories, assist eligibility, objective and support credit, score floors and caps, and protection against farming.
4. Progression behavior: class switching, late joining, match boundaries, effect persistence details, and fair side swap comparisons with carryover. Falling below a threshold removes its upgrade, while existing timed effects survive caster death.
5. Upgrade design: statistical power versus new interactions, threshold spacing, visibility to opponents, and baseline viability against upgraded players.
6. Lethality: criteria for acceptable setup, execution, disable combinations, traps, and environmental kills.
7. Match structure: modes, objectives, team size, match duration, respawn timing, class selection conflicts, and class switching details. Duplicate classes are excluded per team.
8. Recovery and resources: healing timing, health pickups, self damage, and whether the payload interacts with loaded ammo. Unlimited reserves are confirmed.
9. World and roster: exact chronology, geography, technology limits, factions, team color placement, character designs, and silhouette differentiation.

The engine and initial prototype scope are selected in the latest development section. Development schedule, networking package, and release plan remain open. These should follow clarification of the intended gameplay.

## Decision history

| Revision | Change |
| --- | --- |
| Initial preferences | Established deliberate high damage weapons, broad mobility, class team combat, streak rewards, art influences, and exclusions. |
| First clarification | Allowed earned lethal setups and combinations; clarified silent contribution, protected spawns, varied accessibility, and generous ammunition reserves. |
| Latest clarification | Replaced consecutive killstreak progression with score gains and death deductions; specified vulnerability during trap preparation as an intended cost. |
| Version 0.2 | Recorded phase based progression, expected levels 8 to 9 with exceptional level 10, fractional points, solo reward rationale, passive play concerns, and the shotgun and grenade launcher glass cannon concept. |
| Version 0.3 | Confirmed round and side swap resets; updated level 10 expectations; recorded the proposed exceptional transformation and global cutscene; consolidated 7v7, respawn waves, and three core modes from intervening discussion. |
| Version 0.4 | Replaced mandatory cutscene with sound cue; recorded winnable matchups and no unkillable combatants, class specific exceptional upgrade proposal, and unresolved conflict between lethal examples and earlier exclusions. |

| Version 0.5 | Replaced the early dual-wield concept with the Mercenary's first kit, combat loop, and progression proposal. Recorded the resulting damage breakpoints and prototype recommendation. |

| Version 0.6 | Accepted the Mercenary prototype recommendations, including 50 total direct grenade damage, independent firing, and weaker outer pellets to preserve the single-click lethality limit. |

| Version 0.7 | Established the Watchmen-inspired art direction, Siberian and Southwest influences, early mechanical technology, character wardrobe and palette, muted green and purple teams, map references, and combat readability priorities. |

| Version 0.8 | Adopted no fall damage, unlimited reserves, full air strafing, minimal forced control, constrained healing, team recognition across play elements, and explicit visual clarity requirements. Recorded the goal of preventing overly defensive choke play. |

| Version 0.9 | Added the Behemoth as the second of three initial reference classes, excluded dedicated map-spanning attacks, and added a requirement for readable progression cues. |

| Version 0.10 | Adopted 250 HP, 50% frontal guard mitigation, and a dedicated hold-to-guard input. Preserved double left click for uppercut. |

| Version 0.11 | Recorded questionnaire answers 1 to 23, including duel targets, control rules, health and range direction, movement, support, class selection, and pings. Flagged open Mercenary tuning and switching semantics. |

| Version 0.12 | Adopted contribution-based scoring, shared earned level 2 floor, immediate upgrade loss on score reduction, persistent cast buffs, and scoreboard levels. Reversed round resets and removed the god-like tier. Added provisional scoring values. |

| Version 0.13 | Accepted full-recovery resets and meaningful support outcomes. Added the proposed level 8 to 10 progression outline and recommended visibility constraints. |

| Version 0.14 | Recorded objective rules, grouped respawns, spawn immunity, Payload path goals, and lethal-hit save qualification. Marked the Behemoth for a gun-based redesign. |

| Version 0.15 | Replaced the Behemoth fist weapon kit with the four-shell wide-spread shotgun proposal. Retained health, movement, and guard. Flagged its 100-damage one-tap conflict and open guard interactions. |

| Version 0.16 | Corrected Mercenary health to 150 HP and established 125 HP as the roster minimum. Withdrew the Behemoth 80-damage recommendation and cleared the 100-damage shotgun's one-tap conflict. |

| Version 0.17 | Added the Shaman proposal, 100 HP exception, five syringes, progression and stack rules. Calculated cumulative totals and flagged movement, healing, and score-system interactions. |

| Version 0.18 | Retained all five Shaman formulas after clarification, including X and enemy slow. Added an assistant numerical retune, smaller duration upgrades, shared chamber cooldowns, and effect safeguards for prototype review. |

| Version 0.19 | Recorded acceptance of the Shaman retune. Simplified the progression working rule to zero starting score and zero floor, removing level 2 retention and moving self injection to baseline. |

| Version 0.20 | Recorded detailed questionnaire decisions and delegated prototype defaults. Added timing, movement, collision, support, progression, objective, and spawn specifications plus Behemoth progression recommendations. |

| Version 0.21 | Recorded acceptance of the delegated gameplay defaults and initial Cursor-based prototype planning. Engine remains undecided. |

| Version 0.22 | Established the DUSK-inspired low-poly and pixel-texture execution direction with restrained modern lighting. Added concrete silhouettes and clothing for all three classes, including the syringe crossbow. |

| Version 0.23 | Recorded Mac development, C# experience, available gaming PCs, Mercenary-only offline scope, score progression in the first two-player build, and art before friend-facing tests. Selected Unity 6.3 LTS with C# and URP. |

| Version 0.24 | Established offline sandbox priority, minimal menu, static library assets, Windows/macOS ZIPs, persistent settings and developer commands. Recorded LAN six-player goals, compatible-build checks, and deferred host migration. |

When revising, replace superseded rules in the relevant section and briefly record substantive changes here. Preserve examples as examples rather than promoting them into requirements.

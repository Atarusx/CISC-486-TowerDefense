# CISC-486-TowerDefense

## Description / Game Type
**Game name not chosen yet**, is a **top-down co-op tower defense game** with **hero-shooter** aspects implemented. Two or more players control their own unique heros from a preset option to ward off incoming enemy waves. Included will be placing and uphrading towers + their own abilities to protect enemys from reaching the final location and losing.

## Gameplay
Defence: Stop continyous enemy waves from reaching the final location on a predetermined map.
Multiplayer: The ability to combine passive and active gameplay with tower placement and heros.
Hero Abilities: Each hero has a unique skillset that is activated upon a timer, with their own weapons and abilities + having an ultimate skill.
Upgrades: Credits will be rewarded with respect to the dificulty of eliminating an enemy unit plus rewards upon clearing a wave.
Respawn System: Heros if getting hit by enemy units and reaching 0 health points, will be downed and have a timer to respawn or have another hero revive them. A credit cost to instantly respawn will be available as well.
Win Condition: Defeating all enemy units in a wave to get to the next wave. Clearing all waves will complete the game and/or level.
Lose Condition: THe final location is breached by enemy units and will end the game.

## AI Plan
Enemy FSM 
Spawn → Initialize and select randomly from path
Advance → Follow along path reaching either placed towers in the way to get to the final location
Engage → Attack the nearest tower or hero when in range
Targeting → Attack target closest, prioritizing towers and final gate. If none attack final gate unless hero is in range along the path
Eliminated → Drop credits, visual effect

## Scripted Events
Boss Enemy Spawn: make towers stop working for selected amount of time
Unique Hero Ultimate Ability: Allow for attacks to pierce through enemies and increase attack speed
Unit Wave Spawn: If it takes too long to eliminate all the enemy units, the next wave will activate after a selected amount of time
Collection and Upgrading time in-between waves: select time to upgrade towers or upgrade heros.
Wave Difficulty Events: Implementation of night time and day time cycle, with a full moon. In the case of these random events, either more or less enemy units will spawn with varying difficulty to eliminate depending on daytime event.


## Multiplayer Plan / Player Setup
Co-op Mode: Minimum 2 players, can go up to 4 due to 4 hero selections
Player versus Player mode: 2 players or 4 players compete to see which team lasts to longest with infinite waves increasing in difficulty 

## Environment
Theme: Rustic, old-school, knights and devils/zombies
Map: Different paths for enemies to follow, chokepoints, blindspots and back entrances, build tiles for towers
Interactive Elements: Trees and obstacles can be destroyed (paying credits) to change pathways
Pathfinding: A dynamic navigating system to reroute enemies when interactive lane changes occur

## Assets
Heros: 4 hero models with unique abilities with archtypes that fit player playstyles.
Towers: Steampunk and oldschool elements with upgradeable variants.
Enemies: Zombies of varying sizes, boss units, range and melee units.
UI: HUD display, tower selection, ability hud with timers and use cost, credit counter, wave indicater, timer for in-between waves, enemy unit indicator.
VFX: particle effects for abilities around towers and heros, upgrade visual indicators, live display for lane changing of enemy waves.

## Physics
Rigidbody and colliders on all heros, towers, enemies.
Projectile physics for hero weapons, abilities, and towers.
Colliders and triggers on towers, build tiles, revives, and enemy breaches, tower destruction.

## Systems
Economy: Shared credit counter, tower costs, ability costs, independent hero upgrading credits.
Wave Manager: Controls spawn timing, difficulty scaling, scripted events, daytime events.
Upgrading: Handles tower upgrades and tiers, as well hero ability upgrades.
Reviving: Manages teammate revives and self-respawns under conditions. If not revived by teammate, will lose a tier from an upgraded ability.

## Mechanics
Hero Progression: Ability upgrades and ultimate ability unlock + upgrade.
Tower Placement: Select number of tiles to place towers on to upgrade.
Respawn: Timed-out until timer hits 0 to respawn or have a teamate revive for shorter duration. Optional credit payment to revive instantly.


## Controls
Action            Key
Move              Mouse
Aim/Shoot         Automatic aiming and shooting once in range
Ability 1         Q
Ability 2         W
Ultimate Ability  E
Interact          F
Menu              ESC

## Setup


## Deliverables

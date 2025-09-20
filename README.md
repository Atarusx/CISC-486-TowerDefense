# **CISC-486-TowerDefense**

## **Description / Game Type**
**Cordieval Tower Defense** is a top-down co-op tower defense game with hero-shooter aspects implemented.
Two or more players control their own unique heroes from a preset option to ward off incoming enemy waves. Includes tower placement and upgrading plus hero abilities to prevent enemies from reaching the final location and losing.



## **Gameplay**
**Defense:** Stop continuous enemy waves from reaching the final location on a predetermined map.  
**Multiplayer:** Combine passive and active gameplay with tower placement and heroes.  
**Hero Abilities:** Each hero has a unique skillset on a timer, with weapons and an ultimate skill.  
**Upgrades:** Credits rewarded based on enemy difficulty eliminations and wave clears.  
**Respawn System:** Heroes downed at 0 HP can self-respawn after a timer or be revived; instant respawn costs credits.  
**Win:** Defeat all enemy waves.  
**Lose:** Final location breached.

---

## **AI Plan**
### **Enemy FSM**
<ins>Spawn → Initialize and select path.</ins>  
<ins>Advance → Follow path toward the final location, attacking towers if needed.</ins>  
<ins>Engage → Attack nearest tower or hero when in range.</ins>  
<ins>Targeting → Prioritize towers, then gate; heroes if in range.</ins>  
<ins>Eliminated → Drop credits and show visual effects.</ins>



## **Scripted Events**
- Boss Enemy Spawn: Temporarily disable towers.  
- Unique Hero Ultimate: Piercing attacks + increased speed.  
- Unit Wave Auto-Spawn: Next wave starts after set time.  
- Upgrade Break: Time to upgrade towers/heroes between waves.  
- Wave Difficulty: Day/night cycle affects enemy difficulty and gives buffs.

---

## **Multiplayer Plan / Player Setup**
* Co-op: 2–4 players (4 hero selections).  
* PvP: Teams compete to survive endless waves.



## **Environment**
<ins>Theme:</ins> Rustic medieval, knights vs. zombies/devils.  
<ins>Map:</ins> Multiple paths, chokepoints, build tiles.  
<ins>Interactive:</ins> Pay credits to destroy trees/obstacles for new paths.  
<ins>Pathfinding:</ins> Dynamic navigation when lanes change.

---

## **Assets**
**Heroes:** 4 unique archetypes.  
**Towers:** Steampunk/medieval, upgradeable.  
**Enemies:** Varied zombies, ranged/melee, bosses.  
**UI:** HUD, timers, credit counter, wave indicator.  
**VFX:** Ability particles, upgrade indicators, lane-change visuals.



## **Physics**
* Rigidbody & colliders on heroes, towers, enemies, projectiles.  
* Projectile physics for weapons/abilities. 
* Colliders & triggers for towers, build tiles, revives, breaches.

---

## **Systems**
1. Economy: Shared credits, independent hero upgrades.  
2. Wave Manager: Controls spawns, difficulty, events.  
3. Upgrading: Tower/hero ability tiers.  
4. Reviving: Handles teammate revives and penalties.



## **Mechanics**
<ins>Hero Progression:</ins> Ability and ultimate upgrades.  
<ins>Tower Placement:</ins> Limited build tiles.  
<ins>Respawn:</ins> Timer or credit-based instant revive.

---

## **Controls**

| **Action** | **Key** |
|------------|--------|
| Move | Mouse |
| Aim/Shoot | Auto aim & shoot in range |
| Ability 1 | Q |
| Ability 2 | W |
| Ultimate | E |
| Interact | F |
| Menu | Esc |



## **Setup + Deliverables**
* Unity project & C# scripts for controllers/managers.  
* AI navigation system.  
* Animation controllers for shooting, upgrades, etc.  
* Physics for hit detection.  
* GitHub commits with progress updates.  
* In-game debugging for states, collisions, HUD.

---

# **CISC-486-TowerDefense**

## **Description / Game Type**
**Super Tower Engagement Warfare (S.T.E.W)** is a top-down co-op tower defense game with hero-shooter aspects implemented.
Two or more players control their own unique heroes from a preset option to ward off incoming enemy waves. Includes tower placement and upgrading plus hero attributes to prevent enemies from reaching the final location and losing.



## **Gameplay**
**Defense:** Stop continuous enemy waves from reaching the final location on a predetermined map.  
**Multiplayer:** Combine passive and active gameplay with tower placement and heroes.  
**Hero Abilities:** Each hero has a unique skillset that is upgraded with experience upon enemy elimination, with weapons and an ultimate skills.  
**Upgrades:** Credits and Experience rewarded based on enemy difficulty eliminations and wave clears.  
**Respawn System:** Heroes downed at 0 HP can self-respawn after a timer or be revived; instant respawn costs credits.  
**Win:** Defeat all enemy waves.  
**Lose:** Final Portal location breached.

---

## **AI Plan**
### **Enemy FSM**
<ins>Spawn → Initialize and select path.</ins>  
<ins>Advance → Follow path toward the final location, attacking towers if blocking path to main Portal.</ins>  
<ins>Engage → Attack nearest tower or hero when in range, otherwise seek the portal.</ins>  
<ins>Targeting → Prioritize portal, then towers; heroes if in range.</ins>  
<ins>Eliminated → Drop credits and show visual effects.</ins>



## **Scripted Events**
- Boss Enemy Spawn: Temporarily disable towers.  
- Unique Hero Ultimate: Piercing attacks + increased speed.  
- Unit Wave Auto-Spawn: Next wave starts after set time.  
- Upgrade Break: Time to upgrade towers/heroes between waves.  
- Wave Difficulty: Day/night cycle affects enemy difficulty and gives buffs.

---

## **Multiplayer Plan / Player Setup**
* Co-op: 2–4 players (4 hero selections). Local and P2P. 
* PvP: Teams compete to survive endless waves.



## **Environment**
<ins>Theme:</ins> Rustic medieval, knights vs. zombies/devils.  
<ins>Map:</ins> Multiple paths, chokepoints, build tiles.  
<ins>Interactive:</ins> Pay credits to destroy trees/obstacles for new paths.  
<ins>Pathfinding:</ins> Dynamic navigation when avoiding prop obstacles randomly generated in the map chunks.

---

## **Assets**
**Heroes:** 4 unique archetypes.  
**Towers:** Futuristic/Steampunk, upgradeable.  
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
<ins>Hero Progression:</ins> Turret and attribute upgrades.  
<ins>Tower Placement:</ins> Limited build tiles.  
<ins>Respawn:</ins> Timer or credit-based instant revive.

---

## **Controls**

| **Action** | **Key** |
|------------|--------|
| Move | WASD |
| Aim/Shoot | Auto aim |
| Ability 1 | Q |
| Ability 2 | E |
| Ultimate | R |
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

## **FSM Gameplay**

Enemy AI States: (*All detection ranges are changeable attributes*)  

EnemyIdle: Enemy will cycle through a short animation when the player is out of walking range and not detected.  

Transition: When the Player is in walking detection range AND NOT in running detection range, transition to EnemyWalking and move animation.  

EnemyWalking: Enemy will start to move towards the player at walking speed.  

Transition: When the Player is in walking detection range AND in running detection range, transition to EnemyRunning and run animation.  

EnemyRunning: Enemy will start to sprint towards the player at running speed.  

Gameplay found here: https://drive.google.com/file/d/1tT6EF35HOsqKMx35gLl0tcdy5Pd6bysm/view?usp=sharing  

![Image](https://github.com/user-attachments/assets/ca92ee23-f1bb-4d43-ad8d-0080f203178c)  


---


## **Decision-Making and Pathfinding**  

The enemy now has selective targeting, with decisions based on primary and secondary order targets. The enemy will follow

the player around and try to attack them when in targeting range. However, if the player is not in range, the main goal

of the enemy is to attack the central portal to end the game.  

The objective of protecting the portal is accomplished by using placeable turrets that shoot and prevent enemies from reaching it.

The enemy aims to destroy the portal. The enemy will now go out of its way to target turrets when in range if they are closer than

the distance to the portal. The enemy will decide between targeting either placed turrets or the player if the player gets too close 

to clear the path and get to the central portal.  

**Future Implementation**  

* Make the enemy avoid collision with props generated such as rocks to go around
* Limit chunk generation size
* Create an enemy wavemanager to allow waves of enemies to spawn rather than having a gameobject placed in the scene
* Implement Health and Damage to the turrets, portal, player, and enemy
* Create multiple enemies
* Create game states for winning, losing, and a HUD

The decision-making is marked with target debugging in the console window, as seen in the video below.  
Gameplay found here: https://drive.google.com/file/d/1siDTw7YQyLZx46sxqzI-r1w-bK95isNe/view?usp=sharing



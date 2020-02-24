# AstroMender - GGJ2020

AstroMender, an arcade space sim where energy management is everything.

This was created for Global Game Jam 2020 at the SCAD Atlanta site.  

This is my (Kevin O'Neil) local repo clone.

You can view the official GGJ Jam page [HERE](https://globalgamejam.org/2020/games/astromancer-8)

## Energy System
Most of what I personally contributed can be found under the Assets->Scripts->EnergySystem folder.  

The energy system has an interface with simple methods that all other parts of the game can call. For instance, if the player presses the shoot button, the player controller will just call the Shoot() function in the interface. The energy system will then evaluate if the "guns" are powered, if the ship has enough energy to fire, and if so, will deduct the energy cost and return a true value to let the calling function know the shot request is approved. The energy system does not actually fire the shot, it determines whether or not an action can be taken, alters the energy level accordingly, and returns its decision.  

## Randomized loot system  
I also developed a rouge like loot system. After a player had repaired all core systems, they could still level up by collecting energy and filling the bar. If this happens, the player would be prompted with 3 options for buff items. Examples would be a small boost to attack speed, reduced energy cost for boosting, damage reducing armor, etc. The chosen buff would be added to the ship. It would function like one of the core systems, where they could be damaged and repaired based on energy levels. This system would allow the player to author a "build" for each run.  
NOTE: This feature was fully coded, but was unfortunately disabled in the final Jam build due to us not having the time to balance the loot. We thought the base game was more enjoyable with a polished core system than by turning on an unbalanced loot system.


Full Team Credits:

Kartik Kini

Nicholas Shooter

Mitch McClellan

Larry Smith

Kevin O'Neil

James Lee

Alek Francescangeli

Tahri Turner

Jonathan Hunter

Nicholas Millett
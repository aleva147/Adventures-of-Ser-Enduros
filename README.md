# Adventures-of-Ser-Enduros
A 2D turn-based RPG where the hero must defeat five different monsters to beat the game. By defeating a monster, the hero inherits a new skill from that monster's moveset, which can then be used in future battles.

Video showcase: https://youtu.be/7w9sT8iZaZo</br>
Linux build: https://aleva147.itch.io/the-adventures-of-ser-enduros</br>

***
### Project Description
This game was developed in Unity Game Engine, and the project was divided into two separate sections representing server and client code. The server contains all configurations about the game (monster data, hero data, the order of the monsters, and all implemented AIs) and offers endpoints the client uses to set up a new game as well as to decide which moves monsters should make. The remaining in-game logic (the inventory system, the rest of the battle system...) is implemented on the client side. This way, game designers can easily experiment with different game configurations and bot behaviors without any dependency on the current version of the client build.

The client-side of the project was developed with a strong emphasis on creating a very professional, scalable, and maintainable code, so that teams of people can easily take over and build upon this code in the future. It is organized into Controllers, Views, and Services, properly separating logic and visuals. Observer pattern was used throughout the project to ensure mostly decoupled code that can be independently tested and modified. This, in combination with a very simple implementation of a State Machine Pattern for the Battle System, manages to completely avoid reliance on bloated Update methods.

All data is programmed as Scriptable Objects, allowing any non-programmer to modify all configuration parameters in the editor and to create new data with ease. Also, AIs are independent from concrete monsters, which ensures an even higher level of customization, as any monster can be assigned any AI in the editor. Because of this independence, and because the AIs are also Scriptable Objects, new AIs are incredibly easy to add and implement.

Unity version used: 6000.3.14f1

***
### Future Improvements
+ Because this project was very rushed (thought out, designed, and developed in just four days), it's still missing animations, VFXs, SFXs, and music.
+ The hero should have only two move slots unlocked at the beginning of the game and unlock extra slots by leveling up. This way the game will be more challenging and strategic.
+ The monsters should use AIs that match their personalities:
  + Goblin Warrior AI: Isn't very intelligent, always picks a random move. Skips every N-th turn because it only uses exhausting, intensive physical attacks.
  + Spider AI: Because of its great speed and reflexes, it's the only monster that plays before the player. When low on HP it slowly recovers but has to skip its turns by hiding/running away (hero's move have a higher percentage to miss).
  + Witch AI: Debuffs the player as much as possible with her spells and always heals herself when low on HP.  
  + Dragon AI: Only long range attacks work on it because it is in the air. It has no healing moves but is comfortable tanking damage because it has the most HP out of all monsters. Only the strongest attacks work against it because of its tough scales (the player has to replay previous battles and collect these moves).
  + Goblin Mage AI: Immune to magic attacks and reads the hero's mind (picks the best possible move by taking into account the hero's selected move).

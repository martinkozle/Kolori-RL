# Kolori - Game Jam Plus 2021 (RL Version)

This version of the game is a tweaked version of the original game made for reinforcement learning.

It is designed to be run with an `--rl` argument, which will disable user input and instead receive actions from an external agent through a TCP connection (using NetMQ). It also runs the game update loop as fast as it can while still simulating 30 TPS, this let's the agent train faster. It also disables drawing to the screen, unless requested by the agent, in which case the game will slow back down to real time.

***

A small game made as part of the 1st phase in Game Jam Plus 2021.
***
Goal of the game is for the player (blob) to survive for the longest time while avoiding the Erasers. Throughout the game the player can pick up buckets of various colors (purple, blue, green, yellow, and red) and gain power ups as well as change the players color and paint trail. Some of the bucket power ups can kill the Erasers that respawn over time. Paint bar depletes when the player takes damage, the player uses the current power up and over time for a small amount.

- **Blue bucket** - Projectile blast, player shoots blue at the direction of the curser, killing erasers
- **Purple bucket** - Teleportation, player teleports to the cursor's location
- **Red bucket** - bomb blast, player makes a red splash with a large AoE killing erasers around the player
- **Green bucket** - player can dash
- **Yellow bucket** - player slows down movement for erasers

## Game mechanics

Player movement is done using the [W, A, S, D] keys. All power ups can be activated multiple times at the press of [SPACE] key, for as long as there is paint left in the pain bar. Killing an eraser and picking up power ups increments the players score. Player can pause the game anytime with the [ESC] key.

Made with Monogame, Aseprite and FL Studio.

## Authors

LordDeatHunter
martinkozle
BananaMe
AnixDrone
zodiuxus

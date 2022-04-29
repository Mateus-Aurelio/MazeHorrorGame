# MazeHorrorGame
A horror game created for a class assignment, escape from the maze while a monster chases you.

## Objective:
* Collect all keys to escape the maze, but beware the monster!

## Controls:
* WASD to move.
* Mouse to look.

## Development:
* Created for a prototyping project: making a game around an emotion. This game's environment and gameplay revolve around invoking fear in the player. We researched many causes of fear and the human response to fear. Limited mobility, a confusing, dark environment, scary sounds are all meant to make the player feel scared, in moments of both low tension and high tension. 
* The monster AI is a finite state machine and utilizes a navigation mesh for pathfinding. In the first prototype, the monster would look for the player then chase after them, but after some feedback we decided to change the monster to make it blind, which made the game have much more tense moments. Now, the monster cannot see the player, but can hear the sound the player makes. It will run to the last sound it hears, then wander around that area looking for the player.
* Since noise attracts the monster, the player is put in tense situations where their own movements can put them in danger. Running is a good option for trying to escape the monster, but makes a lot of noise. Some keys are on graveled, rough terrain that makes even more noise, making them dangerous to pick up if the monster is near. 
* Inspired by Alien Isolation's AI, the monster's pathfinding is influenced by how long the player has experienced high tension or low tension. The monster will normally roam the maze randomly, but if the player has not been found in too long, the randomness be weighted slightly towards the player being found. On the other hand, if the player has been chased a lot in a short amount of time, the randomness will be weighted against the player being found. 
* The maze has five keys, all in small dead ends or open, vulnerable spaces. The walls of the maze are all identical to make the player feel lost and powerless, but small environment pieces can help the player find themselves somewhat. After collecting all five keys the player must find the exit of the maze to escape.

## Itch.io:
mateusaurelio.itch.io/maze-monster

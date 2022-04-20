# MazeHorrorGame
A horror game created for a class assignment, escape from the maze while a monster chases you.

## Objective:
* Collect all keys to escape the maze, but beware the monster!

## Controls:
* WASD to move.
* Mouse to look.

## Development:
* Created for a prototyping class project: making a game around an emotion. This game's environment and gameplay revolve around invoking fear in the player. Limited mobility, unclear vision, suspenseful music, and scary sounds are all meant to make the player feel scared, in moments of both low tension and high tension.
* The maze has five keys, all in small dead ends or open, vulnerable spaces. The walls of the maze are all identical to make the player feel lost and powerless, but small environment pieces can help the player find themselves somewhat.
* The enemy AI is a finite state machine and utilizes a navigation mesh for pathfinding and raycasts for vision.
* Inspired by Alien Isolation's AI, the monster's pathfinding is influenced by how long the player has experienced high tension or low tension. The monster will normally roam the maze randomly, but if the player has not been found in too long, the randomness be weighted slightly towards the player being found. On the other hand, if the player has been chased a lot in a short amount of time, the randomness will be weighted slightly against the player being found.

## Itch.io:
mateusaurelio.itch.io/maze-monster

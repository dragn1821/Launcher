# Launcher
This project is a video game launcher application that runs as a kiosk menu for Windows games.

# Features:
- Easy configuration through JSON files.
- Launcher monitors it's process to regain focus if it is lost.
- Launcher monitors a game that it launches to reset focus on the game if it is lost.
- Input is blocked when a game is selected to launch and reenabled after the game's process has exited.
- Each game has its own JSON metadata file that is read in on startup.
- Game play counts are tracked and updated when a game is launched.

# Controls:
Player 1
- Up / Down           = Cycle through the list of games.
- Button 1 / Button 2 = Selects the current game to play.

Player 2
- Up / Down           = Cycle through the list of games.
- Button 1 / Button 2 = Selects the current game to play.

# Configuration
settings.json
This file contains general launcher settings.
- GameDirectory    = The root folder of the location where all the games are stored. All sub-folders should be game folders.
- MetaDataFileName = This is the name of the JSON file found in each game folder that describes information about the game.
- ImageWidth       = The width of the game image to display.  The image is resized to this width and positioned 50 pixels from the right edge of the screen.
- ImageHeight      = The height of the game image to display.  The image is resized to this height and positioned 50 pixels from the bottom edge of the screen.
- ScreenWidth      = The width of the screen's resolution.
- ScreenHeight     = The height of the screen's resolution.
- CameraWidth      = The design-time width. Graphics are drawn to this resolution and then resized up to the screen resolution.
- CameraHeight     = The design-time height. Graphics are drawn to this resolution and then resized up to the screen resolution.

game-plays.json
This is the JSON file that contains the saved play count for each game.  When the launcher starts, this file is loaded and then merged with the list of games found in the game directory.  The file is only saved when a game is launched.
- GamePath  = The full path to the game's EXE file.  This is used to uniquly identify the game.
- PlayCount = How many times the game has been launched.

metadata.json
This file contains metadata on each game and is located in the game's folder.  The file name can change and is specified in the settings.json file.
- title       = The title to display for the game in the launcher.
- slug        = The folder name of the game.  This is used to build the full game's path.
- min_players = The minimum number of players, displayed in the launcher.
- max_players = The maximum number of players, displayed in the launcher.
- executable  = The executable file that launches the game.
- image_url   = The image to display for the game in the launcher.

# Versions:
- .NET 4.5
- Monogame 3.6
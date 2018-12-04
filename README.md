# Launcher
This project is a video game launcher application that runs as a kiosk menu for Windows games.

# Downloading this project
- Copy down the repo and open in Visual Studio.
- Right-Click on the Solution and select "Restore Nuget Packages".
- Make sure to change the configuration manager drop down from "Any CPU" to "x86".
- The "settings.json" file has a "GameDirectory" property that needs to point to a folder that contains the games.
- The "settings.json" file has a "MetaDataFileName" property that is the file name of the JSON file that each game must have to work with this launcher. If a game does not have this file in its folder, it will be skipped.

# Features:
- Easy configuration through JSON files.
- Launcher monitors it's process to regain focus if it is lost.
- Launcher monitors a game that it launches to reset focus on the game if it is lost.
- Input is blocked when a game is selected to launch and reenabled after the game's process has exited.
- Each game has its own JSON metadata file that is read in on startup.
- Game play counts are tracked and updated when a game is launched.
- Logging game selection changes, game launches and exits, and exceptions when launching games.

# Controls:
| Player | Change Game | Select Game   | Exit Launcher |
|--------|-------------|---------------|---------------|
| 1      | Up or Down  | . or / or ESC | End key       |
| 2      | W or S      | ` or 1 or ESC |               |

# Configuration
## settings.json
This file contains general launcher settings.
- GameDirectory    = The root folder of the location where all the games are stored. All sub-folders should be game folders.
- MetaDataFileName = This is the name of the JSON file found in each game folder that describes information about the game.
- LogPath          = The folder path to store log files.  This can be a relative path from the application directory.  Logs will include today's date in the file name, so a new log will be created each day.
- ImageWidth       = The width of the game image to display.  The image is resized to this width and positioned 50 pixels from the right edge of the screen.
- ImageHeight      = The height of the game image to display.  The image is resized to this height and positioned 50 pixels from the bottom edge of the screen.
- ScreenWidth      = The width of the screen's resolution.
- ScreenHeight     = The height of the screen's resolution.
- CameraWidth      = The design-time width. Graphics are drawn to this resolution and then resized up to the screen resolution.
- CameraHeight     = The design-time height. Graphics are drawn to this resolution and then resized up to the screen resolution.

## game-plays.json
This is the JSON file that contains the saved play count for each game.  When the launcher starts, this file is loaded and then merged with the list of games found in the game directory.  The file is only saved when a game is launched.  The launcher will manage the contents of this file.
- GamePath  = The full path to the game's EXE file.  This is used to uniquly identify the game.
- PlayCount = How many times the game has been launched.

## metadata.json
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

# Assets:
The following assets were used from the links below.  Both are Public Domain without any copyright by the creator.
- Music: Arcade Dash
- Sound Effect: https://opengameart.org/content/mouse-click
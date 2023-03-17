# MyRoomEditor

![MyRoom](https://github.com/BigJIU/RoomViewer/blob/main/imgs/MyRoom.png)

MyRoomEditor is a Unity plugin designed for interactive indoor scene generation and layout dataset visualization. It provides a user-friendly interface for generating scenes based on layout descriptions in JSON format.

## Features

![MyRoom](https://github.com/BigJIU/RoomViewer/blob/main/imgs/EditorMain.png)

- Import layout datasets into a 3D scene
- Generate a room with the desired scale and position
- Clear generated objects from the scene
- Export generated scenes for further use

## Installation

1. Clone or download this repository.
2. Open the MyRoomEditor project in Unity.
3. Import your layout datasets in JSON format.
4. Use the MyRoomEditor panel to generate scenes based on the imported layouts.

## Usage

### Importing Layout Datasets

MyRoomEditor provides many helpful approaches for loading the layout description file into a 3D scene. The sequence of importing layout datasets can be regulated to suit your needs.

### Generating Scenes

![MyRoom](https://github.com/BigJIU/RoomViewer/blob/main/imgs/EditorGenerate.png)

The MyRoomEditor's generation panel serves as the primary interface for generating scenes. To generate a room, follow these steps:

1. Choose the desired generation room using the RoomID of the JSON file or directly copying the JSON content into the input box.
2. Indicate the existence of the room's wall and floor.
3. Click the "Generate" button.

You can also generate a room with the scale and position of a specific floor. This feature is achieved by dragging the floor's game object from the Unity scene into the corresponding box provided by the generation panel.

### Clearing Generated Objects

MyRoomEditor provides a "Clear" button that removes all objects tagged by "generated" from the current scene. This feature eliminates the need for the manual deletion of objects.

### Exporting Generated Scenes

Generated scenes can be exported for further use in other Unity projects or external applications.

## Future Work

In the future, we plan to expand the dataset and create a faster generation model to improve the efficiency of scene generation.

## Credits

This project was created by [Your Name] as a part of [Your Institution/Company]. If you have any questions or suggestions, please contact [Your Email].
# KS: Framework

KS: Framework is a Unity Engine framework targeted to provide features that are common in a majority of AAA games that would be time consuming for Indie or Hobbyist developers. This version a early in development version that is not intended for production use, purely testing and feedback. 

Provided in this version:
* Localization
* Game manager
* File parsing to set OS locations
* Input manager (DS4 support)
* Config creation
* Splash Screen 
* Load Screen
* Saving and Loading
* In game Time Manager
* Day Night Cycle, Using Latitude and Longitude for real sun cycle, Star system
* Character controller 
* Water buoyancy 
* In game debug console
* Object Pooling
* Map system with minimap support
* Editor windows, For Input manager, Global settings, localization and setup
* And more

#### Survey:

As a test and feedback version there is a survey provided here: https://docs.google.com/forms/d/e/1FAIpQLSfDc3nHpwcRchPeSbLfaK3fGSg-Ts6fHgS7CsbrdIZXLQ0LSg/viewform

I am looking for feedback on this project such as bugs, suggestions and the bad points. Please comment or leave a survey response

# Setup:

Simply extract the Zip to your assets folder. Included with the Zip in KS_Framework/Extras:
* Unity Input manager settings, Replace over your projects InputManager to use the required inputs for input manager 
* Unity Tag Manager settings, Replace over your projects TagManager to use tags required by prefabs
* Documentation, Developer documentation of all classes

If using the input Manager you will need to set up axis used for Controller and Mouse (Case sensitive):

Name | Type | Sensitivity | Axis | Invert
--- | --- | --- | --- | ---
DS4 X-Axis | JoyStick Axis | 1 | X Axis | true
DS4 Y-Axis | JoyStick Axis | 1 | Y Axis | true
DS4 3rd Axis | JoyStick Axis | 1 | 3rd Axis | false
DS4 4th Axis | JoyStick Axis | 1 | 4th Axis | false
DS4 5th Axis | JoyStick Axis | 1 | 5th Axis | false
DS4 6th Axis | JoyStick Axis | 1 | 6th Axis | false
DS4 7th Axis | JoyStick Axis | 1 | 7th Axis | false
DS4 8th Axis | JoyStick Axis | 1 | 8th Axis | false
Mouse X | Mouse Movement | 1 | X axis | false
Mouse Y | Mouse Movement | 1 | Y axis | true
Mouse ScrollWheel | Mouse Movement | 1 | 3rd Axis | false

These are the Layers used for various parts of the framework:

Name | Description |
--- | --- |
Minimap |  Used to hide Minimap objects from player layer
NightCamera | Used with Day Night cycle to hide star system from player layer as rendered to specific depth 

These asset packs are used for the example project:

* Environment
* Particle Systems
* Prototyping
* Physics Materials

# First Use:

If installed correctly you should see a new menu on the tool bar "KS: Framework" in here you will find a setup button, this will add all the basic parts for running the framework and features to your scene aswell as create a new Game config. Other options in this menu are editing windows for localization database and Input database where you can add, edit or delete items. Lastly is an editor for the games global settings.

Also included is an example project running most the features provided, This can be found in "KS_Framework/Example Project/".

# Usage:

##### Game Manager:
This script once applied to an object will make this object and children not be destroyed on load, This script handles the current state of the game aswell as other useful functions and events for certain framework actions such as state change and loading a save. This is required to be in any scene for a number of scripts as it loads in other helper scripts such as KS_FileHelper.

##### Localization:
To use localization you need to have an active instance of KS_Localization in your scene, this can then be used by other scripts via KS_localisation.Instance for functions to fetch translations or change the language.

Included is a new UI element in the menu "GameObject/UI/Text Translate", this game object works exactly like the normal Text game object apart from it uses a "Line ID" box instead of "Text", in here you put the Line ID for the translation your want (More about that below) and this object will display the translation automatically in game, this will also update on language changed aswell.

To create translation strings go to the toolbar and "KS: Framework/Translation Manager" this will open an editor window with its own toolbar to create and open a database. Included is a translation database in "KS_Framework/KS_data/Translations.asset" or you can create a new one, once opened or created you can now create Languages and Strings via the add option on the toolbar.

##### Input Manager:
This is used similarly to Localization, first you need an active instance of KS_Input in your scene. Once in your scene you can use the static methods in KS_Input as you would use unitys own Input class.

To create or edit inputs go to the toolbar and "KS: Framework/Input Manager" This will open an editor window with its own toolbar to create or open a input database. Included is a input database in "KS_Framework/KS_data/Input.asset" or you can create a new one. Once opened you can create and edit inputs. There are three input types Keyboard, Mouse and Axis: Keyboard is simply for button press reading with options to use a DS4/Xbox controller button too (Hotswap); Mouse is for any Mouse click actions; Axis for axis readings from controllers or mouse aswell as positive and negative keyboard keys.

All inputs can be "Editable" via the toggle in the input manager, This means that the inputs are saved to an editable Config outside of the game where users can edit them aswell as marking editable in game too. Non editable inputs will always use the default.

##### Config Creation:
Configs are used by Input manager and Settings manager (Not fully implemented currently) these use it to save game settings or inputs to a .cfg file where its freely editable and loaded with the new settings on next launch. To start a new config use KS_IniParser (See documentation file for more details).

##### Splash Screen:
To use splash screens you can use the prefab in the example project or add KS_SplashScreen to an object. Add splash screens to this component with settings for each screen, Skip button and wait for input button use Input IDs as set in input manager. This script is designed to work along side KS_Manager and will start if the game state starts or is changed to "Intro".

##### Load Screen:
This script is designed to work when KS_Manager Load level function has been called, This will display a simply load screen while loading a level and will auto close it on level loaded or wait for player input before closing. This will also update the KS_manager to game state "Playing".

##### Saving and loading:
Saving And loading can be done from KS_SaveLoad if you are not using KS_Manager in your scene you will need to setup KS_manager.GameConfig before using any functions. On save a new KS_SaveGame object will be created an event "OnSave" will be called that parses a reference of the save object to any subscribers, This is to allow scripts to easily store data into the save file. 

During the save this will also look for any gameobject in the current scene with the script "KS_SavableObject" attached, These game objects will be stored via binary formatter to the save file including components (If you have unserializable components you will have to create a new Surrogate and add it with "KS_SaveLoad.AddSurrogate"). Any object with this script attached must also be a Prefab with the prefab stored in the "Resources" folder.

When you request to load a save you will get back the KS_SaveGame object that contains all the stored data from scripts, packed game objects and more. An event "OnLoad" which parses the KS_SaveGame object will be called on load aswell. When you want to unpack the stored game objects you have to parse each one through "KS_SaveLoad.RestoreGameObject". Objects will not be replaced by the save version and will need to be cleared first.

##### In Game Time manager:
This is a simple 24 hour cycle in game time tracker, The time scale is set by the amount of time (in seconds) per in game minute E.G. time scale of 2 is 30 in game minutes per real minute. This script provides in event call every time the time has changed aswell as functions to get the current time in different ways Such as formatted or in percent of the day.

##### Day Night Cycle:
See the prefab included for quick usage. This script adds a day night cycle to your scene using the In game time manager updates to update the environment. This script uses longitude and latitude to get the real sun position for the current day, time and world position. This script also handles environment changes during the day aswell for things such as ambient light levels and more. 

A star system is included using a seed based generation method, See the prefab on usage.

##### Character Controller: 
A simple character controller is provided that has controller and keyboard support, water buoyancy, Running and crouching. Add KS_CharacterController to your player object.

##### Water Buoyancy:
To use water buoyancy for objects you need to first add KS_Water to your water object, This requires a Colider which should fit your water volume inside (Used to tell when the player is in water) set to trigger. The players buoyancy is handled in the Character controller script where as other objects need to attach the "KS_Buoyancy" script.

##### In Game console:
This system adds a console screen into the game activated by the tidle key (See included prefab). This console allows commands to be run, See the script "ConsoleCommands" for usage on adding new commands. Unitys debug output can also be logged to the console.

#### Object Pooling:
To use this system KS_PoolManager must be used in the scene. Once this is active any object that is using the KS_IPoolObject interface can be added to the pool by calling the "AddToPool" function used with the interface. This requires a bit of setup to use where as you can inherit KS_PoolObject to any object that has this all setup apart from you need to override the PoolObjectSettings method E.G
````
public override PoolObjectSettings PoolSettings()
{
    return new PoolObjectSettings("tag", 10);
}
````
You can fetch objects from the pool manager KS_PoolManager.Get.

##### Map system:
Easily used by using the prefab included. This system provides a full map window in the style of Skyrim or Assassins creed origins. You can place map markers from the Map Marker prefab for interactable places on the map (Limited functionality currently).

Minimap functionality is provided as a separate prefab that uses the  minimapRenderTexture from the full map script.

# License:

MiT License - License file included with download

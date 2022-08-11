# MondayOFF SDK


### Requirements
---
- Faceboook App ID
- Facebook SDK (included in the package)
    - Please refer to https://developers.facebook.com/docs/unity/ for more information about Facebook SDK

### Installation
---
- Add **MondayOFFSDK.unitypackage** to your project.

### Usage
---
1. Select *Facebook > Edit Settings* on the menu
![FacebookSettings](./../res/FBSettingsMenu.png)
1. Add Facebook App Name and App ID in the inspector
![FacebookSettings](./../res/FBSettingsInspector.png)
1. Add Package Name and Class name from above to Facebook App page
![FacebookSettings](./../res/PlatformSettings.png)

1. Add **Assets/MondayOFF/Prefabs/MondayOFF.prefab** to your starting scene.
    - You can also create MondayOFF Game Object to current working scene by selecting *MondayOFF > Create MondayOFF Game Object* on the menu   
    Don't forget to save the scene!
            
    ### Note
    If you are initializing Facebook SDK on your own, select MondayOFF Game Object and uncheck **Also initialize Facebook SDK** from the inspector.


# RainbowTrollArmor
Troll Leather Armor set Generator for Valheim

https://www.nexusmods.com/valheim/mods/1309

Requires LitJson.dll to run



INSTALATION:
Drop the folder from the .rar inside Valheim/Bepinex/plugins/ 
done
Ps. You can do the same with the LitJson that is also needed to launch it (its already inclused inside).

Variables:
- prefabName - name of PrefabFIle for new Item
- inGameName-  ingame name that will be displayed
- sourcePrefabName - name of Source Prefab ( it needs to be 1 of 4 trollhide items others wont work)
- description - modify items description
-weight - wieght of item only in full kilos!
-speedMod - modify speed bonus INT - ex 120  equals to bonus 12% speed
-maxLevel - max level of item (you can make it level up more then 4 times !)
-armor - how much armor as base lvl 1 it will have
-armorPerLevel - how much armor it will gain per level up
-durability - how durable is the item
-durabilityPerLevel - how much durability it will gain per level
-enabled - is recipe for the item enabled
-ratio - value from 0 to 100 - describe the notation of color
- color - rgb values for new color (1- 255 range)
- recipe  - recipe for the item
- resist - add RESISTANCE to item - only 1 type . value string name of damage type  to resist!

RECIPE VALUES :
- enabled - bool true/false should recipe be enabled
- minStationLevel - int - min station level 
- res1 | res2 | res3 | res4 - resources for crafting anotation "PrefabName:Amount:AmountPerLevel:Recover" ex. "Wood:1:0:true"

All recipes are added to workbench !!!

Explemations:
If ratio is set to 0 the  item will be gray color no matter the input color
If ratio is set to 100 the items will have the highest possible value of the input color
Default value is set to 50 

Please always create new item using copy of existing ones to mitigate error of wrong source name as well as always double check  
PREFABNAME = ITEM ID

Atentions :
If either prefabName/ inGameName/ sourcePrefabNameare left empty mod wont work and create error
If error happens and user still will log to the world all new troll items will despawn from game
If error happens but User will see it at the main menu before entering world then fix the config error and restart the game items wont be lost

For those who did not yet use or have LitJson.dll  This is needed for RainbowTroll to work properly . File added to package


EXAMPLE CONFIG BELOW !

All items are stored in  BepInEx/plugins/RainbowTrollArmor/Configs/
 
1 json file per 1 item !

You can have as many troll item as you wish Hood, Cape,Chest,Pants as long as proper value are set
In downloaded .rar file there is folder that contains both example configs and the dll of mod they need to be put inside plugins folder

To disable the mod simply delete the .dll file 

Future plans:
- Ability to add your own texture for armor
- Ability to add your own icon
- Full compatibility with other mods
-Ability to attach custom status effect


Example Item config :
{
    "prefabName":"CapeVoidTrollHide",
    "inGameName":"Void Troll Leather Cape",
    "sourcePrefabName":"CapeTrollHide",
    "description": "Super Void",
    "weight": 1,
    "speedMod": 120,
    "maxLevel":6,
    "armor" : 1,
    "armorPerLevel":1,
    "maxDurability":500,
    "durabilityPerLevel":50,
    "enabled":true,
    "resist" : "Physical",
    "ratio" :30,
    "color":
    {
    "r":127,
    "g":17,
    "b":107
    },
        "recipe":{
            "enabled": true,
            "minStationLevel": "3",
            "res1": "Wood:1:0:true",
            "res2": "",
            "res3": "",
            "res4": ""
        }  
}

using BepInEx;
using HarmonyLib;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System.Linq;

namespace RainbowTrollArmor
{
    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]
    public class Launch : BaseUnityPlugin
    {
        private readonly Harmony harmony = new Harmony("balrond.astafaraios.RainbowTrollArmor");
        public const string PluginGUID = "balrond.astafaraios.RainbowTrollArmor";
        public const string PluginName = "RainbowTrollArmor";
        public const string PluginVersion = "1.0.0";
        public static AssetBundle assetBundle;

        public static List<GameObject> prefabs = new List<GameObject>();
        public static List<Recipe> recipes = new List<Recipe>();

        public static Texture2D chest;
        public static Texture2D pants;
        public static Texture2D secondaryLayer;

        public static Texture2D iconHead;
        public static Texture2D iconChest;
        public static Texture2D iconPants;
        public static Texture2D iconCape;

        public static CraftingStation station;

        public static GameObject RootObject;
        public static GameObject PrefabContainer;


        public static GameObject hood;
        public static GameObject back;
        public static GameObject legs;
        public static GameObject vest;


        public static JsonLoader jsonLoader = new JsonLoader();

        private void Awake()
        {
            createPrefabContainer();
            jsonLoader.loadJson();
            assetBundle = GetAssetBundleFromResources("rainbowtroll");
            loadAssets();
            harmony.PatchAll();
        }
        private void loadAssets()
        {
            string mainPath = "Assets/CustomItems/RAINBOWTROLL/";

            chest = assetBundle.LoadAsset<Texture2D>(mainPath + "TrollLeatherArmorChest_d.png");
            pants = assetBundle.LoadAsset<Texture2D>(mainPath + "TrollLeatherArmorLegs_d.png");
            secondaryLayer = assetBundle.LoadAsset<Texture2D>(mainPath + "TrollLeatherArmorLegs_m.png");

            iconHead = assetBundle.LoadAsset<Texture2D>(mainPath + "head_ico.png");
            iconChest = assetBundle.LoadAsset<Texture2D>(mainPath + "chest_ico.png");
            iconPants = assetBundle.LoadAsset<Texture2D>(mainPath + "pants_ico.png");
            iconCape = assetBundle.LoadAsset<Texture2D>(mainPath + "cape_ico.png");
        }

        void OnDestroy()
        {
            harmony.UnpatchSelf();
        }
        private static bool IsObjectDBValid()
        {
            return ObjectDB.instance != null && ObjectDB.instance.m_items.Count != 0 && ObjectDB.instance.m_recipes.Count != 0 && ObjectDB.instance.GetItemPrefab("Amber") != null;
        }

        public static AssetBundle GetAssetBundleFromResources(string filename)
        {
            var execAssembly = Assembly.GetExecutingAssembly();

            var resourceName = execAssembly.GetManifestResourceNames().Single(str => str.EndsWith(filename));

            using (var stream = execAssembly.GetManifestResourceStream(resourceName))
            {
                return AssetBundle.LoadFromStream(stream);
            }
        }

        public void createPrefabContainer()
        {
            RootObject = new GameObject("_ValheimReforgedRoot");
            UnityEngine.Object.DontDestroyOnLoad((UnityEngine.Object)RootObject);
            PrefabContainer = new GameObject("Prefabs");
            PrefabContainer.transform.parent = RootObject.transform;
            PrefabContainer.SetActive(false);
        }

        public static GameObject cloneMe(GameObject source, string name)
        {
            GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(source, PrefabContainer.transform);
            gameObject.name = name;
            fixMaterials(gameObject, source);
            gameObject.SetActive(true);
            return gameObject;
        }

        public static GameObject fixMaterials(GameObject clone, GameObject source)
        {
            foreach (MeshRenderer renderer in source.GetComponentsInChildren<MeshRenderer>())
            {
                foreach (MeshRenderer cloneRenderer in clone.GetComponentsInChildren<MeshRenderer>())
                {
                    if (renderer.name == cloneRenderer.name)
                    {
                        cloneRenderer.materials = renderer.sharedMaterials;
                        cloneRenderer.sharedMaterials = renderer.sharedMaterials;
                        break;
                    }
                }
            }
            return clone;
        }

        static GameObject FindItem(List<GameObject> list, string name)
        {
            GameObject item = list.Find((x) => x.name == name);
            if (item != null)
            {
                return item;
            }
            Debug.LogWarning("Item Not Found");
            return null;
        }

        static void setOriginals(List<GameObject> list)
        {
            vest = FindItem(list, "ArmorTrollLeatherChest");
            legs = FindItem(list, "ArmorTrollLeatherLegs");
            back = FindItem(list, "CapeTrollHide");
            hood = FindItem(list, "HelmetTrollLeather");
        }



        public static void getCraftingStation()
        {
            GameObject Hammer = ObjectDB.instance.m_items.Find((x) => x.name == "Hammer");
            ItemDrop hammerItemDrop = Hammer.GetComponent<ItemDrop>();

            GameObject craftingTable = hammerItemDrop.m_itemData.m_shared.m_buildPieces.m_pieces.Find((x) => x.name == "piece_workbench");
            station = craftingTable.GetComponent<CraftingStation>();
        }
        public static ArmorPieceType getPieceType(string name)
        {
           // Debug.LogWarning("Name Provided: " + name);
               switch(name)
            {
                case "ArmorTrollLeatherChest":
                    return ArmorPieceType.Chest;
                case "ArmorTrollLeatherLegs":
                    return ArmorPieceType.Legs;
                case "CapeTrollHide":
                    return ArmorPieceType.Back;
                case "HelmetTrollLeather":
                    return ArmorPieceType.Head;
            }

            Debug.LogWarning("Armor Piece Type Not recognized!");
            return ArmorPieceType.Other;
        }

        public static GameObject getSource(ArmorPieceType type)
        {
            switch (type)
            {
                case ArmorPieceType.Chest:
                    return vest;
                case ArmorPieceType.Legs:
                    return legs;
                case ArmorPieceType.Back:
                    return back;
                case ArmorPieceType.Head:
                    return hood;
                case ArmorPieceType.Other:
                    return null;
            }
            return null;
        }

        private static string checkTransformName(ArmorPieceType type)
        {
            switch (type)
            {
                case ArmorPieceType.Chest:
                case ArmorPieceType.Legs:
                case ArmorPieceType.Back:
                    return "log";
                case ArmorPieceType.Head:
                    return "hood";
                case ArmorPieceType.Other:
                    return null;
            }
            return null;
        }

        public static Texture2D pickProperIcon(ArmorPieceType type)
        {
            switch (type)
            {
                case ArmorPieceType.Chest:
                    return iconChest;
                case ArmorPieceType.Legs:
                    return iconPants;
                case ArmorPieceType.Back:
                    return iconCape;
                case ArmorPieceType.Head:
                    return iconHead;
                case ArmorPieceType.Other:
                    return null;
            }

            return null;
        }
        private static Material setProperPlayerArmorTexture(ItemDrop itemDrop, ArmorPieceData armorPieceData, ArmorPieceType type)
        {
            if (type == ArmorPieceType.Back)
            {
                return null;
            }

            Material currMat = itemDrop.m_itemData.m_shared.m_armorMaterial;
            Material armorMaterial = new Material(currMat.shader);
            armorMaterial.CopyPropertiesFromMaterial(currMat);
            armorMaterial.name = armorPieceData.prefabName + "_mat";

            if (type == ArmorPieceType.Chest)
            {
                armorMaterial = SetProperTexture("_ChestTex", armorMaterial, armorPieceData.color.getRgb(), armorPieceData.prefabName, armorPieceData.ratio);
            }
            if (type == ArmorPieceType.Legs)
            {
                armorMaterial = SetProperTexture("_LegsTex", armorMaterial, armorPieceData.color.getRgb(), armorPieceData.prefabName, armorPieceData.ratio, secondaryLayer);
            }
            Debug.Log("Armor Piece Material Created: " + armorPieceData.inGameName);
            return armorMaterial;
        }
        private static Material SetProperTexture(string texturePropertyName, Material armorMaterial, Color color, string name, int ratio, Texture2D sec = null)
        {
            Texture2D texu = (Texture2D)armorMaterial.GetTexture(texturePropertyName);
            if (texu.name == chest.name)
            {
                texu = chest;
            }
            if (texu.name == pants.name)
            {
                texu = pants;
            }
            SpriteTools tools = new SpriteTools();
            tools.setTint(color);
            tools.setRatio(ratio);
            if (sec != null)
            {
                tools.setSecondaryLayer(sec);
            }

            Texture2D newTex = tools.CreateTexture2D(texu, true);
            newTex.name = name + texturePropertyName;
            armorMaterial.SetTexture(texturePropertyName, (Texture)newTex);
            return armorMaterial;
        }

        public static void setItemValues(GameObject armor, ArmorPieceType type, ArmorPieceData armorPieceData)
        {
            ItemDrop itemDrop = armor.GetComponent<ItemDrop>();
            Material armorMaterial = null;
            Sprite newIcon = createIcon(type, armorPieceData);
            string transformName = checkTransformName(type);
            itemDrop.m_itemData.m_shared.m_name = armorPieceData.inGameName;
            if (itemDrop.m_itemData.m_shared.m_armorMaterial != null)
            {
                armorMaterial = setProperPlayerArmorTexture(itemDrop, armorPieceData, type);
                itemDrop.m_itemData.m_shared.m_armorMaterial = armorMaterial;
            }
            EditMaterialColor(armor, transformName, armorPieceData.color.getRgb(), armorMaterial, type);

            if (armorPieceData.maxLevel > 0)
            {
                itemDrop.m_itemData.m_shared.m_maxQuality = armorPieceData.maxLevel;
            }

            itemDrop.m_itemData.m_shared.m_icons[0] = newIcon;
            itemDrop.m_itemData.m_durability = armorPieceData.maxDurability;
            if(armorPieceData.weight != 0)
            {
                itemDrop.m_itemData.m_shared.m_weight = armorPieceData.weight;
            }
           
            itemDrop.m_itemData.m_shared.m_movementModifier = (float)((float)armorPieceData.speedMod/1000);
            itemDrop.m_itemData.m_shared.m_description = armorPieceData.description;
            itemDrop.m_itemData.m_shared.m_armor = armorPieceData.armor;

            itemDrop.m_itemData.m_shared.m_armorPerLevel = armorPieceData.armorPerLevel;
            itemDrop.m_itemData.m_shared.m_maxDurability = armorPieceData.maxDurability;
            itemDrop.m_itemData.m_shared.m_durabilityPerLevel = armorPieceData.durabilityPerLevel;

            if (armorPieceData.resist != "")
            {
                setItemResist(armorPieceData.resist, itemDrop);
            }
        }
        private static void setItemResist(string resistName , ItemDrop itemDrop)
        {
            //create resistance object and add it to item drop
            Debug.LogWarning("If resistance was made it be here");
            if (validateDamageType(resistName))
            {
                HitData.DamageModPair damageModPair = createDamageModif(resistName);
                itemDrop.m_itemData.m_shared.m_damageModifiers.Add(damageModPair);
            }     
        }

        private static bool validateDamageType(string name)
        {
            switch(name)
            {
                case "Fire":
                case "Frost":
                case "Poison":
                case "Spirit":
                case "Blunt":
                case "Slash":
                case "Pierce":
                case "Lighting":
                case "Physical":
                case "Elemental":
                case "Everything":
                case "Pickaxe":
                case "Chop":
                    return true;
                default:
                    return false;
            }
        }

        private static HitData.DamageModPair createDamageModif(string resistName)
        {
            HitData.DamageModPair damageModPair = new HitData.DamageModPair();

            try
            {
                HitData.DamageType type = (HitData.DamageType)Enum.Parse(typeof(HitData.DamageType), resistName);
                damageModPair.m_modifier = HitData.DamageModifier.Resistant;
                damageModPair.m_type = type;
                return damageModPair;
            }
            catch
            {
                Debug.LogWarning("Unrecognized Resistance name");
                return damageModPair;
            }
          
        }

        public static void EditMaterialColor(GameObject gameObject, string transformName, Color color, Material mat, ArmorPieceType type)
        {
            ItemDrop itemDrop = gameObject.GetComponent<ItemDrop>();

            if (transformName == null)
            {
                Debug.LogWarning("Transform name not given for: " + itemDrop.m_itemData.m_shared.m_name);
                return;
            }

            Transform foundElement = gameObject.transform.Find(transformName);
            if (foundElement == null)
            {
                Debug.LogWarning("Attach for transform not found: " + transformName);
                return;
            }

            MeshRenderer mesh = foundElement.GetComponent<MeshRenderer>();
            mesh.material.SetColor("_Color", color);


            Transform attach = null;
            Transform[] trs = gameObject.GetComponentsInChildren<Transform>(true);
            foreach (Transform t in trs)
            {
                if (t.name == "attach_skin")
                {
                    attach = t;
                    break;
                }
            }

            if(attach == null)
            {
                if(ArmorPieceType.Legs != type)
                {
                    Debug.LogWarning("Attach for transform not found: " + transformName);
                }
               
                return;
            }

            attach.gameObject.SetActive(true);
            string tName = null;
            switch (type)
            {
                case ArmorPieceType.Chest:
                    tName = "shorts";
                    break;
                case ArmorPieceType.Back:
                    tName = "cape2";
                    break;
                case ArmorPieceType.Head:
                    tName = "hood";
                    break;
                case ArmorPieceType.Legs:
                case ArmorPieceType.Other:
                    break;
            }
            if (tName != null)
            {
                Transform secondaryElement = attach.Find(tName);
                SkinnedMeshRenderer mesh2 = secondaryElement.GetComponent<SkinnedMeshRenderer>();
                if (mesh2 != null && (type == ArmorPieceType.Head || type == ArmorPieceType.Back))
                {
                    mesh2.material.SetColor("_Color", color);
                }
                if (mesh2 != null && type == ArmorPieceType.Chest)
                {
                    mesh2.material = mat;
                }
            }
            attach.gameObject.SetActive(false);
            Debug.Log("Armor Piece  Material Edited: " + itemDrop.m_itemData.m_shared.m_name);
        }

        public static Sprite createIcon(ArmorPieceType type, ArmorPieceData armorPieceData)
        {
            Texture2D icon = pickProperIcon(type);
            if (icon == null)
            {
                Debug.LogWarning("Armor Piece  Icon Not Found: " + armorPieceData.inGameName);
                return null;
            }
            SpriteTools tools = new SpriteTools();
            tools.setTint(armorPieceData.color.getRgb());
            tools.setRatio(armorPieceData.ratio);
            Debug.Log("Armor Piece  Icon Created: " + armorPieceData.inGameName);
            return tools.CreateSprite(icon, true);
        }

        public static void CreateArmor(ArmorPieceData armorPieceData)
        {
            ArmorPieceType armorPieceType = getPieceType(armorPieceData.sourcePrefabName);
            if (armorPieceType == ArmorPieceType.Other)
            {
                return;
            }
            GameObject source = getSource(armorPieceType);
            GameObject newArmor = cloneMe(source, armorPieceData.prefabName);

            setItemValues(newArmor, armorPieceType, armorPieceData);
            prefabs.Add(newArmor);
            armorPieceData.prefab = newArmor;
            armorPieceData.wasDone = true;
            Debug.Log("Armor Piece Created: " + armorPieceData.inGameName);
        }


        public static void createArmorPieces()
        {
            foreach (ArmorPieceData armorPieceData in jsonLoader.armorJson)
            {
                if (!armorPieceData.wasDone)
                {
                    CreateArmor(armorPieceData);
                }
            }
        }
        public static void createArmorRecipes()
        {
            foreach (ArmorPieceData armorPieceData in jsonLoader.armorJson)
            {
                if (armorPieceData.prefab != null)
                {
                    CreateRecipe(armorPieceData);
                }
            }
        }

        public static void CreateRecipe(ArmorPieceData armorPieceData)
        {
            if(armorPieceData.recipe == null)
            {
                Debug.LogWarning("No Recipe Data for: " + armorPieceData.inGameName);
                return;
            }

            Recipe recipe = ScriptableObject.CreateInstance<Recipe>();
            recipe.name = "Recipe_" + armorPieceData.prefabName;
            recipe.m_craftingStation = station;
            recipe.m_item = armorPieceData.prefab.GetComponent<ItemDrop>();
            recipe.m_minStationLevel = int.Parse(armorPieceData.recipe.minStationLevel);
            recipe.m_amount = 1;
            recipe.m_repairStation = station;
            recipe.m_resources = Requirements(armorPieceData.recipe);
            recipe.m_enabled =  armorPieceData.recipe.enabled;
            recipes.Add(recipe);
        }

        public static Piece.Requirement[] Requirements(RecipeRebalance recipeRebalance)
        {
            recipeRebalance.convertResToList();
            List<Piece.Requirement> requirements = new List<Piece.Requirement>();
            foreach (Resource resource in recipeRebalance.resources)
            {
                resource.setItemDrop(FindItem(ObjectDB.instance.m_items,resource.item));
                requirements.Add(resource.getPieceConfig());
            }
            return requirements.ToArray();
        }

        [HarmonyPatch(typeof(ZNetScene), "Awake")]
        public static class ZNetScene_Awake_Path
        {
            public static void Prefix(ZNetScene __instance)
            {
                if (__instance == null)
                {
                    return;
                }

                foreach (GameObject item in prefabs)
                {
                    __instance.m_prefabs.Add(item);
                }

            }
        }

        [HarmonyPatch(typeof(ObjectDB), "CopyOtherDB")]
        public static class Object_CopyOtherDB_Path
        {
            public static void Postfix()
            {
                if (!IsObjectDBValid())
                {
                    return;
                }
                setOriginals(ObjectDB.instance.m_items);
                getCraftingStation();
                createArmorPieces();
                Debug.LogWarning("Armor Pieces Created: " + prefabs.Count);
                createArmorRecipes();
                Debug.LogWarning("Recipe Created: " + recipes.Count);

                foreach (GameObject item in prefabs)
                {
                    AddItem(item);
                }

                foreach (Recipe recipe in recipes)
                {
                    AddRecipe(recipe);
                }
            }
        }

        [HarmonyPatch(typeof(ObjectDB), "Awake")]
        public static class Object_Awake_Path
        {
            public static void Postfix()
            {
                if (!IsObjectDBValid())
                {
                    return;
                }
                setOriginals(ObjectDB.instance.m_items);
                getCraftingStation();
                createArmorPieces();
                Debug.LogWarning("Armor Pieces Created: " + prefabs.Count);
                createArmorRecipes();
                Debug.LogWarning("Recipe Created: " + recipes.Count);

                foreach (GameObject item in prefabs)
                {
                    AddItem(item);
                }

                foreach(Recipe recipe in recipes)
                {
                    AddRecipe(recipe);
                }
            }
        }
        private static void AddRecipe(Recipe recipe)
        {
            if (!IsObjectDBValid())
            {
                return;
            }

          
            if (recipe != null)
            {
                if (ObjectDB.instance.GetRecipe(recipe.m_item.m_itemData) == null)
                {
                    ObjectDB.instance.m_recipes.Add(recipe);
               Debug.Log(recipe.name + " - Added to the Game");
                }
            }
            else
            {
                Debug.LogError(recipe.name + " - Recipe not found on prefab");
            }
        }

        private static void AddItem(GameObject newPrefab)
        {
            if (!IsObjectDBValid())
            {
                return;
            }

            var itemDrop = newPrefab.GetComponent<ItemDrop>();
            if (itemDrop != null)
            {
                if (ObjectDB.instance.GetItemPrefab(newPrefab.name) == null)
                {
                    ObjectDB.instance.m_items.Add(newPrefab);
                    Dictionary<int, GameObject> m_itemsByHash = (Dictionary<int, GameObject>)typeof(ObjectDB).GetField("m_itemByHash", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(ObjectDB.instance);
                    m_itemsByHash[newPrefab.name.GetHashCode()] = newPrefab;
                    //    Debug.Log(newPrefab.name + " - Added to the Game");
                }
            }
            else
            {
                Debug.LogError(newPrefab.name + " - ItemDrop not found on prefab");
            }
        }
    }
}

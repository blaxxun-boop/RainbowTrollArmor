using System;
using System.Collections.Generic;
using LitJson;
using System.IO;
using System.Linq;
using ServerSync;
using UnityEngine;

namespace RainbowTrollArmor
{
    public class JsonLoader
    {
        public List<ArmorPieceData> armorJson = new List<ArmorPieceData>();
        public CustomSyncedValue<Dictionary<string, string>> jsonData = new CustomSyncedValue<Dictionary<string, string>>(Launch.configSync, "armor jsons");

        public void loadJson()
        {
            LoadArmorFiles();
        }

        private string[] jsonFilePath(string folderName, string extension)
        {
            string path = Directory.GetCurrentDirectory() + Path.Combine(@"/BepInEx/", @"plugins/", @"RainbowTrollArmor/",  @"" + folderName + "/");
            string[] jsonFilePath = Directory.GetFiles(path, extension);
            Debug.Log(folderName + " Json Files Found: " + jsonFilePath.Length);
            return jsonFilePath;
        }

        void LoadArmorFiles()
        {
            jsonData.ValueChanged += LoadArmors;
            jsonData.AssignLocalValue(jsonFilePath("Configs", "*.json").ToDictionary(f => f, File.ReadAllText));
        }

        void LoadArmors()
        {
            armorJson.Clear();
            
            int counter = 0;
            foreach (KeyValuePair<string, string> jsonFile in jsonData.Value)
            {
                ArmorPieceData converting = JsonMapper.ToObject<ArmorPieceData>(jsonFile.Value);
                if (converting != null)
                {
                    armorJson.Add(converting);
                    counter++;
                }
                else
                {
                    Debug.LogError("Loading FAILED file: " + jsonFile.Key);
                }
            }
            Debug.Log("Armor JsonFiles Loaded: " + counter);
        }
    }
}

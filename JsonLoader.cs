using System;
using System.Collections.Generic;
using LitJson;
using System.IO;
using UnityEngine;

namespace RainbowTrollArmor
{
    public class JsonLoader
    {
        public List<ArmorPieceData> armorJson = new List<ArmorPieceData>();

        public void loadJson()
        {
            LoadArmors();
        }

        private string[] jsonFilePath(string folderName, string extension)
        {
            string path = Directory.GetCurrentDirectory() + Path.Combine(@"/BepInEx/", @"plugins/", @"RainbowTrollArmor/",  @"" + folderName + "/");
            string[] jsonFilePath = Directory.GetFiles(path, extension);
            Debug.Log(folderName + " Json Files Found: " + jsonFilePath.Length);
            return jsonFilePath;
        }

        void LoadArmors()
        {
            int counter = 0;
            foreach (string jsonFile in jsonFilePath("Configs", "*.json"))
            {
                string jsonString = File.ReadAllText(jsonFile);
                ArmorPieceData converting = JsonMapper.ToObject<ArmorPieceData>(jsonString);
                if (converting != null)
                {
                    armorJson.Add(converting);
                    counter++;
                }
                else
                {
                    Debug.LogError("Loading FAILED file: " + jsonFile);
                }
            }
            Debug.Log("Armor JsonFiles Loaded: " + counter);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace RainbowTrollArmor
{
    public class ArmorPieceData
    {
        public bool enabled;
        public string prefabName;
        public string inGameName;
        public string description;
        public string sourcePrefabName;
        public int speedMod = 100;
        public int weight = 0;
        public int maxLevel =0;
        public int armor =4;
        public int armorPerLevel =2;
        public int maxDurability = 500;
        public int durabilityPerLevel = 200;
        public string resist = "";
        public bool wasDone = false;
        public RecipeRebalance recipe;
        public int ratio = 50;
        public ColorObject color =  new ColorObject();
        public GameObject prefab = null;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace RainbowTrollArmor
{
    public class RecipeRebalance
    {
        public string name = "";
        public string item = "Wood";
        public string amount = "1";
        public string craftingStation = "";
        public string minStationLevel = "1";
        public bool enabled = true;
        public string repairStation = "";
        public string res1 = "";
        public string res2 = "";
        public string res3 = "";
        public string res4 = "";
        public string res5 = "";
        public string res6 = "";
        public List<Resource> resources = new List<Resource>();

        public bool wasLoaded = false;

        public void convertResToList()
        {
            int counter = 0;

            if (resources.Count == 0)
            {
                counter += stringArrayToResource(splitString(res1));
                counter += stringArrayToResource(splitString(res2));
                counter += stringArrayToResource(splitString(res3));
                counter += stringArrayToResource(splitString(res4));
                counter += stringArrayToResource(splitString(res5));
                counter += stringArrayToResource(splitString(res6));
                if (counter == 6)
                {
                    Debug.LogWarning("No resources found for recipe: " + name);
                }
            }
        }
        string[] splitString(string res)
        {
            if (res != "")
            {
                return res.Split(':');
            }
            return null;
        }

        int stringArrayToResource(string[] array)
        {
            if (array != null)
            {
                if (array.Length == 4)
                {
                    Resource resource = new Resource();
                    resource.item = array[0];
                    resource.amount = Convert.ToInt32(array[1]);
                    resource.amountPerLevel = Convert.ToInt32(array[2]);
                    resource.recovery = Convert.ToBoolean(array[3]);
                    resources.Add(resource);
                    return 0;
                }
                Debug.LogWarning("Incomplete array resource length: " + array.Length);
                return 1;

            }
            return 1;
        }

        public string ObjectToString()
        {
            string line = "Name:" + name + " Item:" + item + " Amount:" + amount + " Table:" + craftingStation + " Level:" + minStationLevel + " Enabled:" + enabled + " Repair:" + repairStation + " Resources:" + ResourcesToString();

            return line;
        }
        public string ResourcesToString()
        {
            string line = "";
            foreach (Resource elem in resources)
            {
                line += " Name:" + elem.item + " Amount:" + elem.amount + " PerLevel:" + elem.amountPerLevel + " Recover:" + elem.recovery + ",";
            }
            return line;
        }
    }
}

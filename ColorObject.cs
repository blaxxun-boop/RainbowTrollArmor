using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace RainbowTrollArmor
{
    public class ColorObject
    {
        public int r;
        public int g;
        public int b;


        public Color getRgb()
        {
            Color newColor = new Color((float)((float)r / 255), (float)((float)g / 255), (float)((float)b / 255), 1);
            return newColor;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace RainbowTrollArmor
{
    public class Resource
    {
        public int amount = 1;
        public int amountPerLevel = 0;
        public bool recovery = true;
        public ItemDrop itemDrop;
        public Piece.Requirement pieceConfig;
        public string item = "Wood";

        public Resource() : base()
        {

        }

        public Resource(string item, int amount, int amountPerLevel = 0, bool recovery = true) : base()
        {
            this.item = item;
            this.amount = amount;
            this.amountPerLevel = amountPerLevel;
            this.recovery = recovery;
        }
        public void setItemDrop(GameObject prefab)
        {
            if (prefab.GetComponent<ItemDrop>() != null)
            {
                this.itemDrop = prefab.GetComponent<ItemDrop>();
            }
            else
            {
                Debug.LogWarning("NO ITEM DROP FOUND on prefab name: " + prefab.name);
            }
        }

        public Piece.Requirement getPieceConfig()
        {
            return pieceConfig = new Piece.Requirement()
            {
                m_resItem = itemDrop,
                m_amount = amount,
                m_amountPerLevel = amountPerLevel,
                m_recover = recovery
            };

        }
    }
}

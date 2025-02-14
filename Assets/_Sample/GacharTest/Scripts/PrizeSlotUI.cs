using System;
using TMPro;
using UnityEngine;

namespace MySampleEx
{
    [Serializable]
    public class Prize
    {
        public string name;
        public string item;
    }

    public class PrizeSlotUI : MonoBehaviour
    {
        public TMP_Text nameText;
        public TMP_Text itemText;

        public void SetNameText(string _name)
        { 
            nameText.text = _name;
            itemText.text = "";
        }

        public void SetItemText(string _item)
        {
            itemText.text = _item;
        }
    }
}
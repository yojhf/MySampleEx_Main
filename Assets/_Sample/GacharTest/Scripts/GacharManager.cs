using System.Collections.Generic;
using UnityEngine;

namespace MySampleEx
{
    public class GacharManager : Singleton<GacharManager>
    {
        private GacharData gacharData = null;

        public GacharUI gacharUI_Name;
        public GacharUI gacharUI_Item;

        // 수상자 목록
        public Transform prizePar;
        public GameObject prizePrefab;

        public List<PrizeSlotUI> prizeSlots = new List<PrizeSlotUI>();
        private int winnerIndex = -1;


        protected override void Awake()
        {
            base.Awake();

            //가차 데이터 가져오기
            if (gacharData == null)
            {
                gacharData = ScriptableObject.CreateInstance<GacharData>();
                //gacharData.gacharItemList = gacharData.LoadData("Gachar/GacharItem");
                //gacharData.gacharNameList = gacharData.LoadData("Gachar/GacharName");
            }
        }

        //가차 데이터 가져오기
        public GacharData GetGacharData()
        {
            if (gacharData == null)
            {
                gacharData = ScriptableObject.CreateInstance<GacharData>();
                //gacharData.gacharItemList = gacharData.LoadData("Gachar/GacharItem");
                //gacharData.gacharNameList = gacharData.LoadData("Gachar/GacharName");
            }

            return gacharData;
        }


        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            gacharUI_Name.SetGachar(true);
            gacharUI_Item.SetGachar(false);
        }
        
        public void NameNextGachar()
        {
            gacharUI_Name.nextBtn.SetActive(false);
            gacharUI_Item.SetGachar(true);

            CreatePrizeSlot();

            prizeSlots[winnerIndex].SetNameText(gacharUI_Name.gacharList.gacharItems[gacharUI_Name.gacharIndex].name);
        }

        public void ItemNextGachar()
        {
            gacharUI_Item.nextBtn.SetActive(false);

            gacharUI_Name.SetGachar(true);
            gacharUI_Item.SetGachar(false);

            prizeSlots[winnerIndex].SetNameText(gacharUI_Item.gacharList.gacharItems[gacharUI_Item.gacharIndex].name);
        }

        void CreatePrizeSlot()
        { 
            GameObject slotGo = Instantiate(prizePrefab, prizePar);
            PrizeSlotUI slotUI = slotGo.GetComponent<PrizeSlotUI>();

            prizeSlots.Add(slotUI);
            winnerIndex++;

        }
    }
}
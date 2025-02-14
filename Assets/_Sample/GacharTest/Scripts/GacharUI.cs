using System.Security.Cryptography;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

namespace MySampleEx
{
    // 가차 뽑기 진행 상태
    public enum GacharState
    { 
        Ready,      // 시작 버튼을 기다리는 상태
        Scroll01,   // 스타트 버튼을 누르면 애니01 플레이, 아이템은 무작위, 스톱 버튼을 기다림
        Scroll02,   // 스톱 버튼을 누르면 애니02 플레이, 가차 아이템이 뽑힌 상태, 공회전 (3번)
        Scroll03,   // 일정 시간이 지나면 정답을 보여준다, 랜덤하게 1 ~ 3 스크롤을 해준다
        Result      // 정답을 보여줌
    }

    public class GacharUI : MonoBehaviour
    {
        public GacharItems gacharList;
        public string dataPath;

        public TMP_Text realName;
        public TMP_Text fakeName;
        
        public TMP_Text sentence;

        public GameObject startBtn;
        public GameObject stopBtn;
        public GameObject nextBtn;

        // 뽑기
        private int nowIndex = 0;
        // scroll2 3회전 공회전
        private int dummyNum = 3;
        // 랜덤 스크롤 횟수
        private int fakeNum = 0;
        private int scrollCount = 0;

        // 가차에 뽑힌 아이템 인덱스
        public int gacharIndex = 0;

        [SerializeField] private string nameText = "누구?";

        Animator animator;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            animator = GetComponent<Animator>();
            gacharList = GacharManager.Instance.GetGacharData().LoadData(dataPath);
        }

        public void SetGachar(bool _isReady)
        {
            if (_isReady)
            {
                realName.text = nameText;
                fakeName.text = nameText;
                sentence.text = "시작 버튼을 누르세요";

                startBtn.SetActive(true);
            }
            else
            {
                realName.text = "";
                fakeName.text = "";
                sentence.text = "";

                startBtn.SetActive(false);
            }
        }

        public void StartGachar()
        {
            animator.Play("NameScroll01");

            startBtn.SetActive(false);
            stopBtn.SetActive(true);

            sentence.text = "스톱 버튼을 누르세요";

            // 뽑기 데이터 초기화
            scrollCount = 0;
            nowIndex = 1;
            realName.text = gacharList.gacharItems[nowIndex].name;
            fakeName.text = gacharList.gacharItems[nowIndex - 1].name;
        }

        public void StopGachar()
        {
            // 뽑기
            gacharIndex = GetCharItem();
            gacharList.gacharItems[gacharIndex].rate = 0;

            Debug.Log(gacharList.gacharItems[gacharIndex].name);

            fakeNum = Random.Range(1, 4);
            nowIndex = gacharIndex - (dummyNum + fakeNum) + 1;
            
            GetCharItem();

            animator.Play("NameScroll02");
            scrollCount = 0;
            stopBtn.SetActive(false);
            sentence.text = "";
        }

        int GetCharItem()
        {
            int result = 0;

            // 랜덤 범위 총합 구하기
            int total = 0;

            for (int i = 0; i < gacharList.gacharItems.Count; i++)
            {
                total += gacharList.gacharItems[i].rate;
            }

            int randomNum = Random.Range(0, total);
            int subToal = 0;

            for (int i = 0; i < gacharList.gacharItems.Count; i++)
            {
                if (gacharList.gacharItems[i].rate == 0)
                {
                    continue;
                }

                subToal += gacharList.gacharItems[i].rate;

                if (randomNum < subToal)
                { 
                    result = i;

                    break;
                }
            }

            return result;
        }

        void GotoScroll03()
        {
            animator.Play("NameScroll03");
            scrollCount = 0;
        }

        void GotoResult()
        {
            animator.Play("Empty");
            nextBtn.SetActive(true);
            sentence.text = $"{gacharList.gacharItems[gacharIndex].name} 뽑혔습니다.";

            
        }

        void SetGacharName()
        {
            if (nowIndex < 0)
            {
                nowIndex += gacharList.gacharItems.Count;
            }

            if(nowIndex >= gacharList.gacharItems.Count)
            {
                nowIndex -= gacharList.gacharItems.Count;
            }

            if (nowIndex == 0)
            {
                realName.text = gacharList.gacharItems[nowIndex].name;
                fakeName.text = gacharList.gacharItems[gacharList.gacharItems.Count - 1].name;
            }
            else
            {
                realName.text = gacharList.gacharItems[nowIndex].name;
                fakeName.text = gacharList.gacharItems[nowIndex - 1].name;

            }

        }

        void PlayScroll01()
        { 
            nowIndex++;
            SetGacharName();

            //animator.Play("NameScroll01");
        }

        void PlayScroll02()
        {
            scrollCount++;

            nowIndex++;
            SetGacharName();

            if (scrollCount >= dummyNum)
            {
                GotoScroll03();
            }
        }
        void PlayScroll03()
        {
            scrollCount++;

            nowIndex++;
            SetGacharName();

            if (scrollCount >= fakeNum)
            {
                GotoResult();
            }
        }



    }
}
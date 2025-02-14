using System.Security.Cryptography;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

namespace MySampleEx
{
    // ���� �̱� ���� ����
    public enum GacharState
    { 
        Ready,      // ���� ��ư�� ��ٸ��� ����
        Scroll01,   // ��ŸƮ ��ư�� ������ �ִ�01 �÷���, �������� ������, ���� ��ư�� ��ٸ�
        Scroll02,   // ���� ��ư�� ������ �ִ�02 �÷���, ���� �������� ���� ����, ��ȸ�� (3��)
        Scroll03,   // ���� �ð��� ������ ������ �����ش�, �����ϰ� 1 ~ 3 ��ũ���� ���ش�
        Result      // ������ ������
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

        // �̱�
        private int nowIndex = 0;
        // scroll2 3ȸ�� ��ȸ��
        private int dummyNum = 3;
        // ���� ��ũ�� Ƚ��
        private int fakeNum = 0;
        private int scrollCount = 0;

        // ������ ���� ������ �ε���
        public int gacharIndex = 0;

        [SerializeField] private string nameText = "����?";

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
                sentence.text = "���� ��ư�� ��������";

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

            sentence.text = "���� ��ư�� ��������";

            // �̱� ������ �ʱ�ȭ
            scrollCount = 0;
            nowIndex = 1;
            realName.text = gacharList.gacharItems[nowIndex].name;
            fakeName.text = gacharList.gacharItems[nowIndex - 1].name;
        }

        public void StopGachar()
        {
            // �̱�
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

            // ���� ���� ���� ���ϱ�
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
            sentence.text = $"{gacharList.gacharItems[gacharIndex].name} �������ϴ�.";

            
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
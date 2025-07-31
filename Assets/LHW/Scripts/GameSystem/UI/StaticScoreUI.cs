using DG.Tweening;
using UnityEngine;

/// <summary>
/// ���� �÷��� ���� ���������� �����Ǵ� ���� UI
/// </summary>
public class StaticScoreUI : MonoBehaviour
{
    [SerializeField] GameObject[] leftWinImages;
    [SerializeField] GameObject[] rightWinImages;

    [Header("Offset")]
    [Tooltip("���ھ� ȹ�� �ִϸ��̼� ���� �� ���� ���� �̹��� �ݿ� ������, ���ھ� ȹ�� �ִϸ��̼� ��ü ���̺��� �ణ ��� �������ּ���")]
    [SerializeField] float scoreObtainDelay = 2f;

    private void OnEnable()
    {
        Init();
        TestIngameManager.OnRoundOver += RoundScoreChange;
        TestIngameManager.OnGameSetOver += GameScoreChange;
    }

    private void OnDisable()
    {
        TestIngameManager.OnRoundOver -= RoundScoreChange;
        TestIngameManager.OnGameSetOver -= GameScoreChange;
    }

    private void Init()
    {
        for (int i = 0; i < leftWinImages.Length; i++)
        {
            leftWinImages[i].SetActive(false);
            rightWinImages[i].SetActive(false);
        }
    }

    /// <summary>
    /// �� ���帶�� Ư�� �÷��̾ 1���� �� �ÿ� UI�� ǥ��. ������ ���� �¸��ڰ� ���� �� �й��ڰ� 1���� ���� ��� �ش� UI�� ��Ȱ��ȭ
    /// </summary>
    private void RoundScoreChange()
    {
        string currentWinner = TestIngameManager.Instance.ReadScore(out int left, out int right);
        if (currentWinner == "Left" && left == 1)
        {
            for (int i = 0; i < leftWinImages.Length; i++)
            {
                if (leftWinImages[i].activeSelf) continue;
                if (!leftWinImages[i].activeSelf)
                {
                    leftWinImages[i].SetActive(true);

                    break;
                }
            }
        }
        else if (currentWinner == "Right" && right == 1)
        {
            for (int i = 0; i < leftWinImages.Length; i++)
            {
                if (rightWinImages[i].activeSelf) continue;
                if (!rightWinImages[i].activeSelf)
                {
                    rightWinImages[i].SetActive(true);

                    break;
                }
            }
        }
    }

    /// <summary>
    /// Ư�� ������ ���� �¸��ڸ� UI�� �ݿ�
    /// </summary>
    private void GameScoreChange()
    {
        string currentWinner = TestIngameManager.Instance.ReadRoundScore(out int leftWinNum, out int rightWinNum);
        currentWinner = TestIngameManager.Instance.ReadScore(out int leftRoundNum, out int rightRoundNum);

        if (currentWinner == "Left")
        {
            leftWinImages[leftWinNum - 1].transform.DOScale(new Vector3(2.5f, 2.5f, 2.5f), 0.1f).SetDelay(scoreObtainDelay);
            if (rightRoundNum == 1 && rightWinImages[rightRoundNum - 1].activeSelf)
            {
                rightWinImages[rightRoundNum - 1].SetActive(false);
            }
        }
        else if (currentWinner == "Right")
        {
            rightWinImages[rightWinNum - 1].transform.DOScale(new Vector3(2.5f, 2.5f, 2.5f), 0.1f).SetDelay(scoreObtainDelay);
            if (leftRoundNum == 1 && leftWinImages[leftWinNum - 1].activeSelf)
            {
                leftWinImages[leftWinNum - 1].SetActive(false);
            }
        }

        if (leftWinNum != 3 && rightWinNum != 3)
        {
            TestIngameManager.Instance.SceneChange();
        }
    }
}
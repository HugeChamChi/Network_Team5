using UnityEngine;
using UnityEngine.UI;

public class GameRestartPanelController : MonoBehaviour
{
    [SerializeField] Button yesButton;
    [SerializeField] Button noButton;

    private void Awake()
    {
        yesButton.onClick.AddListener(RestartGame);
        noButton.onClick.AddListener(EndGame);
    }

    private void RestartGame()
    {
        TestIngameManager.Instance.GameStart();
        Debug.Log("���� �����");
        // TODO : ī�� ���� ȭ������ �̵�
    }

    private void EndGame()
    {
        Debug.Log("���� ����");
        // TODO : ���� ȭ������ �̵�
    }
}
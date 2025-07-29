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
        // TODO : ī�� ���� ȭ������ �̵�
    }

    private void EndGame()
    {
        // TODO : ���� ȭ������ �̵�
    }
}
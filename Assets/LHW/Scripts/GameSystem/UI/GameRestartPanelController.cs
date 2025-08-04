using Photon.Pun;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ���� ����� �г� ����
/// </summary>
public class GameRestartPanelController : MonoBehaviourPun
{
    [SerializeField] Button yesButton;
    [SerializeField] Button noButton;

    Coroutine restartPanelCoroutine;

    private void Awake()
    {
        yesButton.onClick.AddListener(RestartGame);
        noButton.onClick.AddListener(EndGame);
        gameObject.SetActive(false);
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

    [PunRPC]
    public void GameRestartPanelActivate(bool activation)
    {
        gameObject.SetActive(activation);
    }
}
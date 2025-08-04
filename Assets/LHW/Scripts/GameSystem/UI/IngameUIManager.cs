using Photon.Pun;
using System.Collections;
using UnityEngine;

/// <summary>
/// ���� ���� �гΰ� ���� ����� �г��� ������
/// </summary>
public class IngameUIManager : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] RandomMapPresetCreator creator;

    [Header("Panels")]
    [SerializeField] GameObject cardSelectPanel;
    [SerializeField] GameObject roundOverPanel;
    [SerializeField] GameObject gameRestartPanel;

    [Header("Offset")]
    [Tooltip("���� ���� �г� ���� �ð�")]
    [SerializeField] private float roundOverPanelDuration = 3.5f;
    public float RoundOverPanelDuration { get { return roundOverPanelDuration; } }
    [Tooltip("���� ���� �� ����� �г� Ȱ��ȭ ������")]
    [SerializeField] private float restartPanelShowDelay = 3.5f;

    Coroutine ROPanelCoroutine;
    Coroutine restartPanelCoroutine;

    private void OnEnable()
    {
        TestIngameManager.OnRoundOver += RoundOverPanelShow;
        TestIngameManager.OnGameOver += RestartPanelShow;
        TestIngameManager.onCardSelectEnd += HideCardSelectPanel;
    }

    private void OnDisable()
    {
        TestIngameManager.OnRoundOver -= RoundOverPanelShow;
        TestIngameManager.OnGameOver -= RestartPanelShow;
        TestIngameManager.onCardSelectEnd -= HideCardSelectPanel;
    }

    private void RoundOverPanelShow()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            ROPanelCoroutine = StartCoroutine(RoundOverPanelCoroutine());
        }
    }

    public void HideRoundOverPanel()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            roundOverPanel.SetActive(false);
        }
    }

    public void RestartPanelShow()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            restartPanelCoroutine = StartCoroutine(RestartPanelCoroutine());
        }
    }

    public void HideRestartPanel()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            gameRestartPanel.SetActive(false);
        }
    }

    private void HideCardSelectPanel()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonView cardSelectPanelView = cardSelectPanel.GetComponent<PhotonView>();
            cardSelectPanelView.RPC(nameof(CardSelectUIPanelController.CardSelectUIActivate), RpcTarget.AllBuffered, false);
        }
    }

    /// <summary>
    /// ���� ���� �г��� Ȱ��ȭ�ϰ� ���ӽð���ŭ ������ ���� �ٽ� ��Ȱ��ȭ�ϴ� �ڷ�ƾ
    /// </summary>
    /// <returns></returns>
    IEnumerator RoundOverPanelCoroutine()
    {
        WaitForSeconds delay = new WaitForSeconds(roundOverPanelDuration);
        PhotonView roundOverPanelView = roundOverPanel.GetComponent<PhotonView>();
        roundOverPanelView.RPC(nameof(RoundOverPanelController.RoundOverPanelActivate), RpcTarget.All, true);

        yield return delay;
        roundOverPanelView.RPC(nameof(RoundOverPanelController.RoundOverPanelActivate), RpcTarget.All, false);

        TestIngameManager.Instance.RoundStart();
        if (TestIngameManager.Instance.IsGameSetOver)
        {
            Debug.Log("�� ��Ʈ ����");
            creator.MapUpdate(TestIngameManager.Instance.CurrentGameRound);
            TestIngameManager.Instance.GameSetStart();
            if (!TestIngameManager.Instance.IsGameOver)
            {
                PhotonView cardSelectPanelView = cardSelectPanel.GetComponent<PhotonView>();
                cardSelectPanelView.RPC(nameof(CardSelectUIPanelController.CardSelectUIActivate), RpcTarget.AllBuffered, true);
            }
        }

        ROPanelCoroutine = null;
    }

    IEnumerator RestartPanelCoroutine()
    {
        yield return new WaitForSeconds(restartPanelShowDelay);

        PhotonView restartPanelView = gameRestartPanel.GetComponent<PhotonView>();
        restartPanelView.RPC(nameof(GameRestartPanelController.GameRestartPanelActivate), RpcTarget.AllBuffered, true);

        restartPanelCoroutine = null;
    }
}
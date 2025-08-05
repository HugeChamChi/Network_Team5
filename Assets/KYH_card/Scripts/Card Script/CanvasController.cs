using DG.Tweening;
using ExitGames.Client.Photon;
using Photon.Pun;
using UnityEngine;
public class CanvasController : MonoBehaviourPunCallbacks
{
    [SerializeField] private Canvas MasterCanvas;
    [SerializeField] private Canvas ClientCanvas;
    [SerializeField] private CardSelectManager cardSelectManager;
    FlipCard flipCard;
    private bool isMyTurn = false;
    private bool alreadyStarted = false;
    void Start()
    {
        MasterCanvas.gameObject.SetActive(false);
        ClientCanvas.gameObject.SetActive(false);

        Debug.Log("�����");

        if (PhotonNetwork.IsMasterClient)
        {
            // ���� ������ ����
            int firstSelectorActorNum = Random.Range(0, 2) == 0
                ? PhotonNetwork.PlayerList[0].ActorNumber
                : PhotonNetwork.PlayerList[1].ActorNumber;

            ExitGames.Client.Photon.Hashtable props = new();
            props["IsFirstSelector"] = firstSelectorActorNum;
            PhotonNetwork.CurrentRoom.SetCustomProperties(props);
        }

        // ������/Ŭ���̾�Ʈ ���� ���� �ʱ�ȭ �õ�
       // TryStartCardSelection();
    }



    public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
    {
        if (propertiesThatChanged.ContainsKey("IsFirstSelector"))
        {
            Debug.Log("�·�������Ƽ������Ʈ ����");
            TryStartCardSelection();
        }
    }

    public void TryStartCardSelection()
    {
        Debug.Log($"[TryStartCardSelection] ȣ��� | alreadyStarted={alreadyStarted}");

        alreadyStarted = false;

        Debug.Log("ī�� ���� ������ ���� �˸�");

        if (alreadyStarted)
        {
            Debug.LogWarning("[TryStartCardSelection] �̹� ���۵� �� �ߴ�");
            return;
        }
        if (!PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue("IsFirstSelector", out object selectorObj)) return;

        alreadyStarted = true;

        int selectorActorNum = (int)selectorObj;

        isMyTurn = (PhotonNetwork.LocalPlayer.ActorNumber == selectorActorNum);

        if (isMyTurn)
        {
            Debug.Log("���� ���� ������ �� ī�� ���� ����");

            if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("isWinner", out object isWinnerObj)
                && (bool)isWinnerObj == true)
            {
                Debug.Log("���� ���� �� ī�� ���� ����");

                ExitGames.Client.Photon.Hashtable props = new();
                props["Select"] = true;
                PhotonNetwork.LocalPlayer.SetCustomProperties(props);

                DOVirtual.DelayedCall(2f, () =>
                {
                    photonView.RPC(nameof(RPC_SwitchTurnToOther), RpcTarget.All);
                });
                return;
            }

            // ���� ���� �����ϴ� ��� (���� or ù ����)
            int[] selectedIndexes = cardSelectManager.GetRandomCardIndexes().ToArray();
            photonView.RPC(nameof(RPC_SyncMasterCanvas), RpcTarget.All, selectedIndexes);
        }
        else
        {
            Debug.Log("������ ���� ������ �� ���� ���");
        }
    }

    [PunRPC]
    public void RPC_SyncMasterCanvas(int[] indexes)
    {
        MasterCanvas.gameObject.SetActive(true);
        ClientCanvas.gameObject.SetActive(false);
        cardSelectManager.UpdateCharacterVisibility();
        bool canInteract = PhotonNetwork.LocalPlayer.ActorNumber ==
                           (int)PhotonNetwork.CurrentRoom.CustomProperties["IsFirstSelector"];

        cardSelectManager.SpawnCardsFromIndexes(indexes, canInteract);
    }

    [PunRPC]
    public void RPC_SwitchTurnToOther()
    {
        // if (cardSelectManager.HasSelected()) //  �̹� ������ ����� �� �Ѿ�� �ƹ��͵� ���� ����
        // {
        //     Debug.Log("�̹� ������ �÷��̾�� ����");
        //     return;
        // }

        Debug.Log("���� �ݴ� �÷��̾�� ��ȯ��");

        MasterCanvas.gameObject.SetActive(false);
        ClientCanvas.gameObject.SetActive(true);

        // �� ����
        // isMyTurn = !isMyTurn; //  ���� �� ��ü ����

        if (!isMyTurn)
        {
            // ���� �� ���ʰ� �Ǿ��� �� ���� ����
            if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("isWinner", out object isWinnerObj)
                && (bool)isWinnerObj == true)
            {
                Debug.Log("���� ���� �� ī�� ���� ����");

                ExitGames.Client.Photon.Hashtable props = new();
                props["Select"] = true;
                PhotonNetwork.LocalPlayer.SetCustomProperties(props);
                return;
            }

            // ���� ���ڰų� ù �� �� ���� ����
            int[] selectedIndexes = cardSelectManager.GetRandomCardIndexes().ToArray();
            photonView.RPC(nameof(RPC_SyncClientCanvas), RpcTarget.All, selectedIndexes);
        }
    }

    [PunRPC]
    public void RPC_SyncClientCanvas(int[] indexes)
    {
        MasterCanvas.gameObject.SetActive(false);
        ClientCanvas.gameObject.SetActive(true);
        cardSelectManager.UpdateCharacterVisibility();
        bool canInteract = !isMyTurn; // �� ��° ������ ����� ���� ����
        cardSelectManager.SpawnClientCardsFromIndexes(indexes, canInteract);
    }

    public bool IsMyTurn()
    {
        return isMyTurn;
    }

    public bool IsMasterCanvasActive()
    {
        return MasterCanvas != null && MasterCanvas.gameObject.activeSelf;
    }

    public bool IsClientCanvasActive()
    {
        return ClientCanvas != null && ClientCanvas.gameObject.activeSelf;
    }

    public void ResetCardSelectionState()
    {
        Debug.Log("ĵ���� ��Ʈ�ѷ��� ���� ī�弱�� �ʱ�ȭ");
        alreadyStarted = false;
        isMyTurn = false;

        MasterCanvas.gameObject.SetActive(false);
        ClientCanvas.gameObject.SetActive(false);


    }

    public void DecideNextSelector()
    {
        Debug.Log("�ӽ� �׽�Ʈ�� ��,�� ���� ���� �ʱ�ȭ");
        int nextSelector = Random.Range(0, 2) == 0
            ? PhotonNetwork.PlayerList[0].ActorNumber
            : PhotonNetwork.PlayerList[1].ActorNumber;

        ExitGames.Client.Photon.Hashtable props = new();
        props["IsFirstSelector"] = nextSelector;
        PhotonNetwork.CurrentRoom.SetCustomProperties(props);
    }
}

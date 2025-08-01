using Photon.Pun;
using UnityEngine;

public class CanvasController : MonoBehaviourPun
{
    [SerializeField] private Canvas MasterCanvas;
    [SerializeField] private Canvas ClientCanvas;
    [SerializeField] private CardSelectManager cardSelectManager;

    void Start()
    {
        MasterCanvas.gameObject.SetActive(true);
        ClientCanvas.gameObject.SetActive(false);

        //  if (PhotonNetwork.IsMasterClient)
        //  { // ī�� ����
        //      cardSelectManager.SpawnRandomCards1(photonView.IsMine); // �����͸� ��ȣ�ۿ� ����
        //  }

        // RPC�� �����ڿ��� ����ȭ
        if (PhotonNetwork.IsMasterClient)
        {
            
            Debug.Log("������ ī�� ������ ����");
            int[] selectedIndexes = cardSelectManager.GetRandomCardIndexes().ToArray();
            photonView.RPC(nameof(RPC_SyncMasterCanvas), RpcTarget.All);

        }
    }

    [PunRPC]
    void RPC_SyncMasterCanvas(int[] indexes)
    {
        MasterCanvas.gameObject.SetActive(true);
        ClientCanvas.gameObject.SetActive(false);
        Debug.Log("������ ī�� ������ ����");
        bool canInteract = PhotonNetwork.IsMasterClient;
        cardSelectManager.SpawnCardsFromIndexes(indexes, canInteract); // ������ ������ (���� �Ұ�)
    }

    [PunRPC]
    public void RPC_SwitchToClientCanvas()
    {
        Debug.Log("ĵ���� ����Ī ����");
        MasterCanvas.gameObject.SetActive(false);
        ClientCanvas.gameObject.SetActive(true);

        // if (PhotonNetwork.IsMasterClient)
        // {
        //     cardSelectManager.SpawnRandomCards1(false); // �����ʹ� ����
        // }
        // else
        // {
        //     cardSelectManager.SpawnRandomCards1(true); // �����ڴ� ��ȣ�ۿ� ����
        // }

        if (PhotonNetwork.IsMasterClient == false) // �����ڸ� �ε��� ������
        {
            Debug.Log("������ ī�� ������ ����");
            int[] selectedIndexes = cardSelectManager.GetRandomCardIndexes().ToArray();
            photonView.RPC(nameof(RPC_SyncClientCanvas), RpcTarget.All, selectedIndexes);
        }
    }

    [PunRPC]
    public void RPC_SyncClientCanvas(int[] indexes)
    {
        MasterCanvas.gameObject.SetActive(false);
        ClientCanvas.gameObject.SetActive(true);
        Debug.Log("������ ī�� ������ ����");
        bool canInteract = !PhotonNetwork.IsMasterClient; // �����ڸ� ��ȣ�ۿ� ����
        cardSelectManager.SpawnClientCardsFromIndexes(indexes, canInteract);
    }
}

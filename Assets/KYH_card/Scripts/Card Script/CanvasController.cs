using DG.Tweening;
using Photon.Pun;
using UnityEngine;

public class CanvasController : MonoBehaviourPun
{
    [SerializeField] private Canvas MasterCanvas;
    [SerializeField] private Canvas ClientCanvas;
    [SerializeField] private CardSelectManager cardSelectManager;

    void OnEnable()
    {
        MasterCanvas.gameObject.SetActive(true);
        ClientCanvas.gameObject.SetActive(false);

        Debug.Log("�� �ο��̺� �����");
        //  if (PhotonNetwork.IsMasterClient)
        //  { // ī�� ����
        //      cardSelectManager.SpawnRandomCards1(photonView.IsMine); // �����͸� ��ȣ�ۿ� ����
        //  }

        // RPC�� �����ڿ��� ����ȭ
        // if (PhotonNetwork.IsMasterClient)
        // {
        //     
        //     Debug.Log("������ ī�� ������ ����");
        //     int[] selectedIndexes = cardSelectManager.GetRandomCardIndexes().ToArray();
        //     photonView.RPC(nameof(RPC_SyncMasterCanvas), RpcTarget.All, selectedIndexes);
        //
        // }

        if (PhotonNetwork.IsMasterClient)
        {
            if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("isWinner", out object isWinnerObj))
            {
                bool isWinner = (bool)isWinnerObj;

                if (isWinner)
                {
                    Debug.Log("�����Ͱ� ���� �� ī�� ���� �ǳʶ�");

                    ExitGames.Client.Photon.Hashtable props = new();
                    props["Select"] = true;
                    PhotonNetwork.LocalPlayer.SetCustomProperties(props);

                    DOVirtual.DelayedCall(2f, () =>
                    {
                        // ���⼭ RPC_SwitchToClientCanvas ���� �� Ŭ���̾�Ʈ���Ը� ���� ��ȸ ��� ��
                        photonView.RPC(nameof(RPC_SwitchToClientCanvas), RpcTarget.All);
                    });

                    return;
                }

                // �����Ͱ� ���� �� ī�� ���� ����
                Debug.Log("������ ī�� ������ ���� (����)");
            }
            else
            {
                // ù ���� �� ���� ����
                Debug.Log("ù ī�� ���� �� ī�� ���� ����");
            }

            int[] selectedIndexes = cardSelectManager.GetRandomCardIndexes().ToArray();
            photonView.RPC(nameof(RPC_SyncMasterCanvas), RpcTarget.All, selectedIndexes);
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

        if (!PhotonNetwork.IsMasterClient)
        {
            // Ŭ���̾�Ʈ�� ������ ��� �� ���� ����
            if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("isWinner", out object isWinnerObj)
                && (bool)isWinnerObj == true)
            {
                Debug.Log("Ŭ���̾�Ʈ�� ���� �� ī�� ���� ����");

                ExitGames.Client.Photon.Hashtable props = new();
                props["Select"] = true;
                PhotonNetwork.LocalPlayer.SetCustomProperties(props);
                return; // ���� �г� �������� ����
            }

            // Ŭ���̾�Ʈ�� ���� �� ���� ����
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

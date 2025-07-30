using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasController : MonoBehaviourPun
{
    [SerializeField] private Canvas MasterCanvas;
    [SerializeField] private Canvas ClientCanvas;
    [SerializeField] private CardSelectManager cardSelectManager;

    void Start()
    {
            MasterCanvas.gameObject.SetActive(true);
            ClientCanvas.gameObject.SetActive(false);

        if (PhotonNetwork.IsMasterClient)
        { // ī�� ����
            cardSelectManager.SpawnRandomCards1(true); // �����͸� ��ȣ�ۿ� ����
        }
        
            // RPC�� �����ڿ��� ����ȭ
            photonView.RPC(nameof(RPC_SyncMasterCanvas), RpcTarget.All);

    }

    [PunRPC]
    void RPC_SyncMasterCanvas()
    {
        MasterCanvas.gameObject.SetActive(true);
        ClientCanvas.gameObject.SetActive(false);

        cardSelectManager.SpawnRandomCards1(false); // ������ ������ (���� �Ұ�)
    }

    [PunRPC]
    public void RPC_SwitchToClientCanvas()
    {
        MasterCanvas.gameObject.SetActive(false);
        ClientCanvas.gameObject.SetActive(true);

        if (PhotonNetwork.IsMasterClient)
        {
            cardSelectManager.SpawnRandomCards1(false); // �����ʹ� ����
        }
        else
        {
            cardSelectManager.SpawnRandomCards1(true); // �����ڴ� ��ȣ�ۿ� ����
        }
    }
}

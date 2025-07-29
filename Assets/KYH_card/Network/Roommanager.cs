using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Roommanager : MonoBehaviour
{
    [SerializeField] private Button startButton;
    [SerializeField] private Button leaveButton;

    [SerializeField] private GameObject playerPanelItemPrefabs;
    [SerializeField] private Transform PlayerPanelContent;

    public Dictionary<int, PlayerPanelItem> playerPanels = new Dictionary<int, PlayerPanelItem>();

    private void Start()
    {
        startButton.onClick.AddListener(GameStart);
        leaveButton.onClick.AddListener(LeaveRoom);
    }
    public void PlayerPanelSpawn(Player player)
    {
        if(playerPanels.TryGetValue(player.ActorNumber, out PlayerPanelItem panel))
        {
            startButton.interactable = true;
            panel.Init(player);
            return;
        }

        // ���� �÷��̾ ���ο� �÷��̾� ���� �� ȣ��
        GameObject obj = Instantiate(playerPanelItemPrefabs);
        obj.transform.SetParent(PlayerPanelContent);
        PlayerPanelItem item = obj.GetComponent<PlayerPanelItem>();
        // �ʱ�ȭ
        item.Init(player);
        playerPanels.Add(player.ActorNumber, item);
    }

    public void PlayerPanelSpawn()
    {
        PhotonNetwork.AutomaticallySyncScene = true;

        if (!PhotonNetwork.IsMasterClient)
        {
            startButton.interactable = false;
        }

        // ���� ���� ���� ���� �� ȣ��
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            GameObject obj = Instantiate(playerPanelItemPrefabs);
            obj.transform.SetParent(PlayerPanelContent);
            PlayerPanelItem item = obj.GetComponent<PlayerPanelItem>();
            // �ʱ�ȭ
            item.Init(player);
            playerPanels.Add(player.ActorNumber, item);
        }
    }


    private bool isSceneLoading = false;
    public void GameStart()
    {
        if (isSceneLoading) return;

        if (PhotonNetwork.IsMasterClient && AllPlayerReadyCheck())
        {
            Debug.Log("[GameStart] �����Ͱ� CardTest �� �ε� ����");
            isSceneLoading = true;
            PhotonNetwork.LoadLevel("CardTest");
        }
    }

    public bool AllPlayerReadyCheck()
    {
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            if (!player.CustomProperties.TryGetValue("Ready", out object value) || !(bool)value)
            {
                return false;
            }
        }
        Debug.Log("��� �÷��̾ �غ� �Ϸ� �Ǿ���");
        return true;
    }


    

    public void PlayerPanelDestroy(Player player)
    {
        if (playerPanels.TryGetValue(player.ActorNumber, out PlayerPanelItem panel))
        {
            Destroy(panel.gameObject);
            playerPanels.Remove(player.ActorNumber);
        }
        else
        {
            Debug.LogError("�г��� �������� �ʽ��ϴ�.");
        }
    }


    public void LeaveRoom()
    {
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            Destroy(playerPanels[player.ActorNumber].gameObject);
        }

        playerPanels.Clear();

        PhotonNetwork.LeaveRoom();
    }
}

using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class NetworkManager : MonoBehaviourPunCallbacks
{
    [Header("�ε� ����")]
    [SerializeField] private GameObject loadingPanel;
    [SerializeField] private TextMeshProUGUI StateText;

    [Header("�г��� ����")]
    [SerializeField] private GameObject nicknamePanel;
    [SerializeField] private Button nicknameAdmitButton;
    [SerializeField] private TMP_InputField nicknameField;

    [Header("�κ� ����")]
    [SerializeField] private GameObject lobbyPanel;
    [SerializeField] private TMP_InputField roomNameField;
    [SerializeField] private Button roomNameAdmitButton;
    [SerializeField] private GameObject roomListItemPrefabs;
    [SerializeField] private Transform roomListContent;

    [SerializeField] private GameObject roomPanel;


    private Dictionary<string, GameObject> roomListItems = new Dictionary<string, GameObject>();

    [SerializeField] private Roommanager Roommanager;
    [SerializeField] private CardSelectManager cardSelectManager;
    private void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
        loadingPanel.SetActive(true);
        nicknamePanel.SetActive(false);
        lobbyPanel.SetActive(false);
        roomPanel.SetActive(false);
        nicknameAdmitButton.onClick.AddListener(NicknameAdmit);
        roomNameAdmitButton.onClick.AddListener(CreateRoom);
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        Debug.Log("������ ���� �����");
        if (loadingPanel.activeSelf)
        {
            loadingPanel.SetActive(false);
        }
        else
        {
            PhotonNetwork.JoinLobby();
        }
            nicknamePanel.SetActive(true);
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);
        Debug.Log($"{cause} �� ���� ������ ������");
        PhotonNetwork.ConnectUsingSettings();
        Debug.Log("�翬����");
    }

    public void NicknameAdmit()
    {
        if (string.IsNullOrWhiteSpace(nicknameField.text))
        {
            Debug.Log("�г��� �Է� �� ����");
            return;
        }

        PhotonNetwork.NickName = nicknameField.text;
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        base.OnJoinedLobby();
        Debug.Log("�κ� ���� �Ϸ��");
        nicknamePanel.SetActive(false);
        roomPanel.SetActive(false);
        lobbyPanel.SetActive(true);

    }

    public void CreateRoom()
    {
        if (string.IsNullOrEmpty(roomNameField.text))
        {
            Debug.Log("�� �̸��� ������ �� �� �����ϴ�.");
            return;
        }

        roomNameAdmitButton.interactable = false;

        RoomOptions options = new RoomOptions { MaxPlayers = 2 };
        PhotonNetwork.CreateRoom(roomNameField.text, options);
        roomNameField.text = null;
    }

    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        roomNameAdmitButton.interactable = true;
        Debug.Log($"{roomNameField.text} �� ���� ��");
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        lobbyPanel.SetActive(false);
        roomPanel.SetActive(true);
        Debug.Log($"{roomNameField.text} �� ���� �Ϸ�");
        Roommanager.PlayerPanelSpawn();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        if (newPlayer != PhotonNetwork.LocalPlayer)
        {
            Roommanager.PlayerPanelSpawn(newPlayer);
        }
    }


    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);

        if (otherPlayer != PhotonNetwork.LocalPlayer)
        {
            Roommanager.PlayerPanelDestroy(otherPlayer);
        }
    }
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        base.OnRoomListUpdate(roomList);
        foreach (RoomInfo info in roomList)
        {
            if (info.RemovedFromList)
            {
                if (roomListItems.TryGetValue(info.Name, out GameObject obj))
                {
                    Destroy(obj);
                    roomListItems.Remove(info.Name);
                }

                continue;
            }

            if (roomListItems.ContainsKey(info.Name))
            {
                roomListItems[info.Name].GetComponent<RoomListItem>().Init(info);
            }
            else
            {
                GameObject roomListItem = Instantiate(roomListItemPrefabs);
                roomListItem.transform.SetParent(roomListContent);
                roomListItem.GetComponent<RoomListItem>().Init(info);
                roomListItems.Add(info.Name, roomListItem);
            }
        }
    }

    public override void OnPlayerPropertiesUpdate(Player target, ExitGames.Client.Photon.Hashtable propertiesThatChanged)
    {
        base.OnPlayerPropertiesUpdate(target, propertiesThatChanged);

        // ���� �غ� üũ ����
        Roommanager.playerPanels[target.ActorNumber].ReadyCheck(target);

        
    }

    public override void OnMasterClientSwitched(Player newClientPlayer)
    {
        base.OnMasterClientSwitched(newClientPlayer);
        Roommanager.PlayerPanelSpawn(newClientPlayer);
    }
    private void Update()
    {
        StateText.text = $"Current State : {PhotonNetwork.NetworkClientState}";
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
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

    private Dictionary<string, GameObject> roomListItems = new Dictionary<string, GameObject>();

    private void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
        loadingPanel.SetActive(true);
        nicknamePanel.SetActive(false);
        lobbyPanel.SetActive(false);
        nicknameAdmitButton.onClick.AddListener(NicknameAdmit);
        roomNameAdmitButton.onClick.AddListener(CreateRoom);
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        Debug.Log("������ ���� �����");
        loadingPanel.SetActive(false);
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
        if(string.IsNullOrWhiteSpace(nicknameField.text))
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
        lobbyPanel.SetActive(true);
    }

    public void CreateRoom()
    {
        if(string.IsNullOrEmpty(roomNameField.text))
        {
            Debug.Log("�� �̸��� ������ �� �� �����ϴ�.");
            return;
        }

        roomNameAdmitButton.interactable = false;

        RoomOptions options = new RoomOptions { MaxPlayers = 2};
        PhotonNetwork.CreateRoom(roomNameField.text,options);
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
        Debug.Log($"{roomNameField.text} �� ���� �Ϸ�");
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
    private void Update()
    {
        StateText.text = $"Current State : {PhotonNetwork.NetworkClientState}";
    }
}

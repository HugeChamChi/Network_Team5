using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Realtime;
using Photon.Pun;
public class RoomListItem : MonoBehaviour
{
    // �� �̸�
    [SerializeField] private TextMeshProUGUI roomNameText;
    // ���� �ο�
    [SerializeField] private TextMeshProUGUI playerCountText;
    // ��ư�� ������ �� ����
    [SerializeField] private Button joinButton;

    private string roomName;
    public void Init(RoomInfo info)
    {
        roomName = info.Name;
        roomNameText.text = $"Room Name : {roomName}";
        playerCountText.text = $"{info.PlayerCount} / {info.MaxPlayers}";

        joinButton.onClick.AddListener(JoinRoom);
    }

    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom(roomName);

        joinButton.onClick.RemoveListener(JoinRoom);
    }
}

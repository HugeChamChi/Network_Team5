using System;
using System.Collections;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerDisconnectedPopup : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject popupPanel;

    [SerializeField] private Button confirmButton;
    
 
    private void OnDisable()
    {
        Debug.Log("OnDisable");
        InGameManager.OnPlayerDisconnected -= ShowDisconnectedPopup;
    }

    private void Awake()
    {
        InGameManager.OnPlayerDisconnected += ShowDisconnectedPopup;
        confirmButton.onClick.AddListener(OnConfirmClick);
        popupPanel.SetActive(false);
    }

    void ShowDisconnectedPopup()
    {
        popupPanel.SetActive(true);
        
        // 게임 일시정지 
        Time.timeScale = 0;
    }

    void OnConfirmClick()
    {
        Debug.Log("OnConfirmClick");
        Time.timeScale = 1;
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        Debug.Log("OnLeftRoom");
        SceneManager.LoadScene("USW/LobbyScene/LobbyScene");
    }
}

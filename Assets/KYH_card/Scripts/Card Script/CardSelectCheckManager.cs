using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class CardSelectCheckManager : MonoBehaviourPunCallbacks
{
    private bool hasSelected = false;
    [SerializeField] private TextMeshProUGUI NicknameText;
    [SerializeField] private TextMeshProUGUI currentSelect;
    void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public void Init(Player player)
    {
        NicknameText.text = player.NickName;
       // currentSelect.text = 
    }

    public void OnCardSelected()
    {
        if (hasSelected) return;

        hasSelected = true;
        Debug.Log("�� ī�� ���� �Ϸ��");

        CardSelectCheckUpdate();
    }

    public void CardSelectCheckUpdate()
    {
        ExitGames.Client.Photon.Hashtable selectProperty = new ExitGames.Client.Photon.Hashtable
        {
            { "hasSelect", hasSelected }
        };

        PhotonNetwork.LocalPlayer.SetCustomProperties(selectProperty);
    }

    public bool AllPlayerCardSelectCheck()
    {
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            if (!player.CustomProperties.TryGetValue("hasSelect", out object value) || !(bool)value)
            {
                Debug.Log($"{player.NickName} ���� ���� ����");
                return false;
            }
        }

        return true;
    }

    public override void OnPlayerPropertiesUpdate(Player target, ExitGames.Client.Photon.Hashtable propertiesThatChanged)
    {
        base.OnPlayerPropertiesUpdate(target, propertiesThatChanged);

        if (propertiesThatChanged.ContainsKey("hasSelect"))
        {
            if (PhotonNetwork.IsMasterClient && AllPlayerCardSelectCheck())
            {
                Debug.Log(" ��� �÷��̾� ī�� ���� �Ϸ� �� Game Scene �ε�");
                PhotonNetwork.LoadLevel("Game Scene");
            }
        }
    }
}


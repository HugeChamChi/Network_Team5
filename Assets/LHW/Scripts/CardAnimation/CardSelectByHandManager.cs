using Photon.Pun;
using UnityEngine;

public class CardSelectByHandManager : MonoBehaviourPun
{
    [SerializeField] GameObject[] cards;
    [SerializeField] CardSceneArmController armController;
    
    
   
    private int selectedIndex = -1;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            SelectRightCard();
            if (selectedIndex >= 0 && selectedIndex < cards.Length)
            {
                Debug.Log("������");
                cards[selectedIndex].GetComponent<FlipCard>().PlayFlipAnimation();

                // RPC�� �� ������ ��� ����
                photonView.RPC(nameof(RPC_SelectCardArm), RpcTarget.All, selectedIndex);

                CardAnimaitonPlay();
            }
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            SelectLeftCard();
            if (selectedIndex >= 0 && selectedIndex < cards.Length)
            {
                Debug.Log("������");
                cards[selectedIndex].GetComponent<FlipCard>().PlayFlipAnimation();

                photonView.RPC(nameof(RPC_SelectCardArm), RpcTarget.All, selectedIndex);

                if (cards[selectedIndex].GetComponent<FlipCard>().IsFlipped)
                {
                    CardAnimaitonPlay();
                }
            }
        }
    }

    // �� ����ȭ�� ���� RPC
    [PunRPC]
    void RPC_SelectCardArm(int cardIndex)
    {
        armController.SelectCard(cardIndex);
    }

    private void SelectRightCard()
    {
        if (selectedIndex >= cards.Length - 1) return;

        selectedIndex++;
    }

    private void SelectLeftCard()
    {
        if (selectedIndex <= 0) return;

        selectedIndex--;
    }

    private void CardAnimaitonPlay()
    {
        for (int i = 0; i < cards.Length; i++)
        {
            if (i == selectedIndex)
            {
                cards[selectedIndex].GetComponentInChildren<CardAnimator>().RestartAnimation();
                continue;
            }
            else if (cards[i].GetComponent<FlipCard>().IsFlipped)
            {
                cards[i].GetComponentInChildren<CardAnimator>().StopAnimation();
            }
        }
    }
    
}
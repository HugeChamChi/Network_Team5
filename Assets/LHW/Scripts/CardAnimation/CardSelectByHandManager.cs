using UnityEngine;

public class CardSelectByHandManager : MonoBehaviour
{
    [SerializeField] GameObject[] cards;
    [SerializeField] CardSceneArmController armController;
    
    // ��ũ��Ʈ �浹 ������ �ӽ� �ּ�ó���߽��ϴ�. ��ȣ���� �ø�ī�� ��ũ��Ʈ�� ���� �ʿ�
    /*
    private int selectedIndex = -1;
    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.D))
        {
            SelectRightCard();
            if (selectedIndex >= 0 && selectedIndex < cards.Length)
            {
                cards[selectedIndex].GetComponent<LHWFlipCard>().PlayFlipAnimation();
                armController.SelectCard(selectedIndex);
                CardAnimaitonPlay();
            }
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            SelectLeftCard();
            if (selectedIndex >= 0 && selectedIndex < cards.Length)
            {
                cards[selectedIndex].GetComponent<LHWFlipCard>().PlayFlipAnimation();
                armController.SelectCard(selectedIndex);
                if (cards[selectedIndex].GetComponent<LHWFlipCard>().IsFlipped)
                {
                    CardAnimaitonPlay();
                }
            }
        }
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
            else if (cards[i].GetComponent<LHWFlipCard>().IsFlipped)
            {
                cards[i].GetComponentInChildren<CardAnimator>().StopAnimation();
            }
        }
    }
    */
}
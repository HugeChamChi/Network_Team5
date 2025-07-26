using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CardUI : MonoBehaviour
{
    public GameObject frontImage;     // frontImage ������Ʈ
    public GameObject backImage;      // BackImage ������Ʈ

    public Text cardNameText;         // Cardname �ؽ�Ʈ
    public Text cardDescriptionText;  // Carddiscription �ؽ�Ʈ

 //  private CardEffectSO cardData;
 //  private PlayerStats target;
 //  private bool isFront = false;
 //  private bool isChosen = false;
 //
 //  public void Setup(CardEffectSO card, PlayerStats player)
 //  {
 //      cardData = card;
 //      target = player;
 //
 //      // �ؽ�Ʈ ä���
 //      cardNameText.text = card.cardName;
 //      cardDescriptionText.text = card.description;
 //
 //      // �ʱ����: �޸�
 //      frontImage.SetActive(false);
 //      backImage.SetActive(true);
 //      isFront = false;
 //
 //      // ���� ����
 //      transform.localScale = Vector3.zero;
 //      transform.DOScale(1f, 0.4f).SetEase(Ease.OutBack);
 //  }
 //
 //  public void OnClickCard()
 //  {
 //      if (isChosen) return;
 //
 //      if (!isFront)
 //      {
 //          // �޸� �� �ո� ȸ��
 //          isFront = true;
 //          Sequence seq = DOTween.Sequence();
 //          seq.Append(transform.DORotate(new Vector3(0, 90, 0), 0.25f))
 //             .AppendCallback(() =>
 //             {
 //                 backImage.SetActive(false);
 //                 frontImage.SetActive(true);
 //             })
 //             .Append(transform.DORotate(new Vector3(0, 180, 0), 0.25f));
 //      }
 //      else
 //      {
 //          // �ո� ������ �� ���� ó��
 //          isChosen = true;
 //          cardData.ApplyEffect(target);
 //          CardManager.Instance.RemoveOtherCards(this);
 //      }
 //  }
 //
 //  public void DestroyCard()
 //  {
 //      transform.DOScale(0f, 0.3f).SetEase(Ease.InBack)
 //          .OnComplete(() => Destroy(gameObject));
 //  }
}//

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;
public class FlipCard : MonoBehaviour
{
    private bool isFlipped = false;      // �޸� �� �ո����� �������°�?
    private bool isSelected = false;     // ���õǾ��°�?

    [Header("��/�޸� ��Ʈ ������Ʈ")]
    public GameObject frontRoot; // frontImage
    public GameObject backRoot;  // BackImage

    [Header("����")]
    public float flipDuration = 0.25f;

    private CardSelectManager manager;

    public void SetManager(CardSelectManager mgr)
    {
        manager = mgr;
    }

    private void Start()
    {
        isFlipped = false;
        isSelected = false;

        transform.localRotation = Quaternion.Euler(0f, 180f, 0f);

        if (frontRoot != null) frontRoot.SetActive(false);
        if (backRoot != null) backRoot.SetActive(true);
    }

    public void OnClickCard()
    {
        // ���� ������ ���� ���¸� �� ȸ���ؼ� �ո�����
        if (!isFlipped)
        {
            isFlipped = true;

            transform.DORotate(new Vector3(0, 0, 0), flipDuration)
                .SetEase(Ease.InOutSine)
                .OnUpdate(() =>
                {
                    float yRot = transform.localEulerAngles.y;
                    if (yRot > 180f) yRot -= 360f;
                    bool showFront = Mathf.Abs(yRot) <= 90f;

                    frontRoot.SetActive(showFront);
                    backRoot.SetActive(!showFront);
                })
                .OnComplete(() =>
                {
                    frontRoot.SetActive(true);
                    backRoot.SetActive(false);
                });
        }
        // �̹� �ո��̰� ���� ���õ��� �ʾҴٸ� �� ���� ó��
        else if (!isSelected)
        {
            isSelected = true;
            manager?.OnCardSelected(gameObject);
        }
    }
}
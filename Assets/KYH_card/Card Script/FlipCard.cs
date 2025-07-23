using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;
public class FlipCard : MonoBehaviour
{
    private bool CardFlip = false; // false = �޸�, true = �ո�

    [Header("Canvas to control")]
    public GameObject frontCanvas; // �ո� Canvas
    public GameObject backCanvas;  // �޸� Canvas

    [Header("����")]
    public float flipDuration = 0.25f;

    private void Start()
    {
        // ī�� �޸� ���·� ����
        transform.localRotation = Quaternion.Euler(0f, 180f, 0f);
        CardFlip = false;

        if (frontCanvas != null) frontCanvas.SetActive(false);
        if (backCanvas != null) backCanvas.SetActive(true);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Flip();
        }
    }

    public void Flip()
    {
        CardFlip = !CardFlip;
        float targetY = CardFlip ? 0f : 180f;

        // �̸� �ո� �Ѱ� �޸� �� (ȸ�� �� ���¿� ���� �ٽ� �ٲ� �� ����)
        if (frontCanvas != null) frontCanvas.SetActive(true);
        if (backCanvas != null) backCanvas.SetActive(true);

        transform.DORotate(new Vector3(0, targetY, 0), flipDuration)
            .SetEase(Ease.InOutSine)
            .OnUpdate(() =>
            {
                float yRot = transform.localEulerAngles.y;
                if (yRot > 180f) yRot -= 360f;

                // �ո� ���̴� ����: -90 ~ 90
                bool showFront = Mathf.Abs(yRot) <= 90f;

                if (frontCanvas != null) frontCanvas.SetActive(showFront);
                if (backCanvas != null) backCanvas.SetActive(!showFront);
            })
            .OnComplete(() =>
            {
                // ���� ���� ����
                if (frontCanvas != null) frontCanvas.SetActive(CardFlip);
                if (backCanvas != null) backCanvas.SetActive(!CardFlip);
            });
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CardAnimator : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Sprite[] clips;               // ������ ������
    public Image card;                   // �ִϸ��̼� ���� ��� �̹���
    public float wait = 0.1f;            // ������ �� �ð�

    public int numberOfClips;
    private int stage = 0;
    private float timer = 0f;

    private bool isHovered = false;

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovered = true;
        timer = 0f; // Ÿ�̸ӵ� �ʱ�ȭ�ϸ� ��� ù ������ ��� ����
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovered = false;
        stage = 0;                         // �ִϸ��̼� ù ���������� ����
        timer = 0f;
        card.sprite = clips[0];           // �̹����� �ʱ�ȭ
    }

    private void Start()
    {
        if (clips != null && clips.Length > 0)
        {
            numberOfClips = clips.Length;
            card.sprite = clips[0];       // ó�� ���������� ����
        }
    }

    private void Update()
    {
        if (!isHovered || clips.Length == 0) return;

        timer += Time.deltaTime;

        if (timer >= wait)
        {
            timer = 0f;
            stage = (stage + 1) % numberOfClips;
            card.sprite = clips[stage];
        }
    }
}

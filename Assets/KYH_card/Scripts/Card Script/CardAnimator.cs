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
    private bool isPlaying = false;

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovered = true;
        timer = 0f;

        // ȣ�� �ÿ��� ��� ����
        isPlaying = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovered = false;
        stage = 0;
        timer = 0f;
        isPlaying = false;

        if (clips != null && clips.Length > 0)
            card.sprite = clips[0]; // �ʱ� ���������� ����
    }

    private void Start()
    {
        if (clips != null && clips.Length > 0)
        {
            numberOfClips = clips.Length;
            card.sprite = clips[0];
        }
    }

    private void Update()
    {
        if ((!isHovered && !isPlaying) || clips.Length == 0) return;

        timer += Time.deltaTime;

        if (timer >= wait)
        {
            timer = 0f;
            stage = (stage + 1) % numberOfClips;
            card.sprite = clips[stage];
        }
    }

    /// <summary>
    /// �ܺο��� �ִϸ��̼� ����� (ó������)
    /// </summary>
    public void RestartAnimation()
    {
        stage = 0;
        timer = 0f;
        isPlaying = true;

        if (card != null && clips.Length > 0)
            card.sprite = clips[0];
    }

    /// <summary>
    /// �ܺο��� �ִϸ��̼� ���� ����
    /// </summary>
    public void StopAnimation()
    {
        isPlaying = false;
        stage = 0;
        timer = 0f;

        if (card != null && clips.Length > 0)
            card.sprite = clips[0];
    }
}

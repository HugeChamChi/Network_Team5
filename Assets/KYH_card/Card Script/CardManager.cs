using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSelectManager : MonoBehaviour
{
    [Header("��ü ī�� ������ ����Ʈ")]
    public List<GameObject> allCardPrefabs;

    [Header("�θ� ���̾ƿ� �׷�")]
    public Transform cardSpawnParent;

    [Header("����� ī�� ����")]
    public int cardCountToShow = 3;

    private List<GameObject> currentCards = new();
    private bool hasSelected = false;

    void Start()
    {
        SpawnRandomCards();
    }

    public void SpawnRandomCards()
    {
        if (allCardPrefabs.Count < cardCountToShow)
        {
            Debug.LogError("ī�� �������� �����մϴ�.");
            return;
        }

        List<int> selectedIndexes = new();

        while (selectedIndexes.Count < cardCountToShow)
        {
            int rand = Random.Range(0, allCardPrefabs.Count);
            if (!selectedIndexes.Contains(rand))
                selectedIndexes.Add(rand);
        }

        // �߾� ���� ī�� ���� ����
        float spacing = 500f; // ī�� �� ���� (�ȼ� ����, Canvas ����)
        float centerX = 0f;
        float startX = centerX - (spacing * (cardCountToShow - 1) / 2f);

        for (int i = 0; i < cardCountToShow; i++)
        {
            GameObject card = Instantiate(allCardPrefabs[selectedIndexes[i]], cardSpawnParent);

            RectTransform rt = card.GetComponent<RectTransform>();
            if (rt != null)
            {
                rt.anchoredPosition = new Vector2(startX + spacing * i, -600f); // ���� ��ġ �Ʒ�
                rt.DOAnchorPosY(0f, 0.5f).SetEase(Ease.OutBack); // ���� Ƣ������� �ִϸ��̼�
            }

            FlipCard flip = card.GetComponent<FlipCard>();
            if (flip != null)
                flip.SetManager(this);

            currentCards.Add(card);
        }
    }

    public void OnCardSelected(GameObject selected)
    {
        if (hasSelected) return;
        hasSelected = true;

        foreach (GameObject card in currentCards)
        {
            if (card != selected)
                Destroy(card, 0.3f);
        }

        Debug.Log("���õ� ī��: " + selected.name);
    }
}

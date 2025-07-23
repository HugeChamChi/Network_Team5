using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSelectManager : MonoBehaviour
{
    [Header("��ü ī�� ������ ����Ʈ")]
    public List<GameObject> allCardPrefabs; // ���ӿ��� ����� ��ü ī�� ������ ���

    [Header("�θ� ���̾ƿ� �׷�")]
    public Transform cardSpawnParent; // ������ ī�尡 ���� �θ�(ĵ���� �� ��ġ �����̳�)

    [Header("����� ī�� ����")]
    [SerializeField] public int cardCountToShow = 3; // �� ���� ������ ī�� ����

    private List<GameObject> currentCards = new(); // ���� ȭ�鿡 ǥ�� ���� ī�� ���
    private bool hasSelected = false; // �÷��̾ ī�带 �����ߴ��� ����

    void Start()
    {
        SpawnRandomCards(); // ���� �� ī����� �����ϰ� ����
    }

    // ���� ī�� ���� �� ȭ�鿡 ���
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

        float radiusX = 550f;   // �¿� ���� ���� (Ŭ���� �� �а�)
        float radiusY = 300f;   // ���� ��ġ�� ������ ���� (�������� �� ���� �ö�)
        float totalAngle = 100f;
        float yOffset = 200f;  // ���ϴ� ��ŭ ���� �ø� �� ( ���ϴ� ������ ���� ����)
        Vector2 center = Vector2.zero;

        for (int i = 0; i < cardCountToShow; i++)
        {
            GameObject card = Instantiate(allCardPrefabs[selectedIndexes[i]], cardSpawnParent);
            RectTransform rt = card.GetComponent<RectTransform>();
            CanvasGroup cg = card.GetComponent<CanvasGroup>();
            if (cg == null) cg = card.AddComponent<CanvasGroup>();

            float angle = -totalAngle / 2f + (totalAngle / (cardCountToShow - 1)) * i;
            float rad = angle * Mathf.Deg2Rad;

            // �� ���� ��ġ ���
            float targetX = Mathf.Sin(rad) * radiusX;
            float targetY = -Mathf.Abs(Mathf.Sin(rad)) * radiusY + yOffset;  // �Ʒ������� ��������

            float startY = -600f;

            if (rt != null)
            {
                rt.anchoredPosition = new Vector2(targetX, startY);
                cg.alpha = 0f;

                Sequence seq = DOTween.Sequence();
                seq.Append(rt.DOAnchorPosY(targetY, 0.6f).SetEase(Ease.OutCubic));
                seq.Join(cg.DOFade(1f, 0.6f));
                seq.SetAutoKill(true);
            }

            FlipCard flip = card.GetComponent<FlipCard>();
            if (flip != null)
                flip.SetManager(this);

            currentCards.Add(card);
        }
    }

    // ī�� �ϳ��� ���õǾ��� �� ȣ���
    public void OnCardSelected(GameObject selected)
    {
        if (hasSelected) return; // �̹� �����ߴٸ� ����
        hasSelected = true; // ���� �÷��� ����

        foreach (GameObject card in currentCards)
        {
            if (card == null) continue;

            if (card == selected)
            {
                // ���õ� ī�� �� ���̵�ƿ�
                CanvasGroup cg = card.GetComponent<CanvasGroup>();
                if (cg == null) cg = card.AddComponent<CanvasGroup>();

                cg.DOFade(0f, 0.5f)
                  .SetEase(Ease.InOutSine)
                  .OnComplete(() => Destroy(card)); // �ִϸ��̼� ������ ����
            }
            else
            {
                // ���õ��� ���� ī�� �� �Ʒ��� �̵��ϸ� �����
                RectTransform rt = card.GetComponent<RectTransform>();
                if (rt != null)
                {
                    rt.DOAnchorPosY(rt.anchoredPosition.y - 300f, 0.5f)
                      .SetEase(Ease.InBack)
                      .OnComplete(() => Destroy(card));
                }
                else
                {
                    // UI ������Ʈ�� �ƴ϶�� �Ϲ� Transform �̵����� ��ü
                    card.transform.DOMoveY(card.transform.position.y - 3f, 0.5f)
                        .SetEase(Ease.InBack)
                        .OnComplete(() => Destroy(card));
                }
            }
        }

        Debug.Log("���õ� ī��: " + selected.name);
    }
}

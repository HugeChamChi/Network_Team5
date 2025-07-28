using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class CardSelectManager : MonoBehaviourPunCallbacks
{
    [Header("��ü ī�� ������ ����Ʈ")]
    public List<GameObject> allCardPrefabs; // ���ӿ��� ����� ��ü ī�� ������ ���

    [Header("�θ� ���̾ƿ� �׷�")]
    public Transform cardSpawnParent; // ������ ī�尡 ���� �θ�(ĵ���� �� ��ġ �����̳�)

    [Header("����� ī�� ����")]
    public int cardCountToShow = 3; // �� ���� ������ ī�� ����

    [Header("��ä�� ��ġ ����")]
    public float xSpacing = 300f; // ī�� �� X ���� ������
    public float curveHeight = 150f; // Y ��ġ�� �ó�� �ֱ� ���� ��
    public float maxAngle = 60f; // ȸ�� �ð� ����
    public float appearYOffset = -600f;


    private List<GameObject> currentCards = new(); // ���� ȭ�鿡 ǥ�� ���� ī�� ���
    private bool hasSelected = false; // �÷��̾ ī�带 �����ߴ��� ����

    void Start()
    {
        SpawnRandomCards(); // ���� �� ī����� �����ϰ� ����
        SceneLoadingManager.Instance.LoadSceneAsync("Game Scene");
        Debug.Log("���� �� ���� �Ѿ�� ���� �ε� ����");
    }

    // ���� ī�� ���� �� ȭ�鿡 ���
    public void SpawnRandomCards()
    {
        if (allCardPrefabs.Count < cardCountToShow)
        {
            Debug.LogError("ī�� �������� �����մϴ�.");
            return;
        }

        // ���� ī�� ����
        List<int> selectedIndexes = new();
        while (selectedIndexes.Count < cardCountToShow)
        {
            int rand = Random.Range(0, allCardPrefabs.Count);
            if (!selectedIndexes.Contains(rand))
                selectedIndexes.Add(rand);
        }

        float centerIndex = (cardCountToShow - 1) / 2f;

        for (int i = 0; i < cardCountToShow; i++)
        {
            GameObject card = Instantiate(allCardPrefabs[selectedIndexes[i]], cardSpawnParent);
            RectTransform rt = card.GetComponent<RectTransform>();
            CanvasGroup cg = card.GetComponent<CanvasGroup>();
            if (cg == null) cg = card.AddComponent<CanvasGroup>();

            float offset = i - centerIndex;

            //  X ��ǥ�� ������� ����
            float x = offset * xSpacing;

            //  Y�� �ε巯�� ��� ���� ���� ��¦
            float y = -Mathf.Abs(offset) * curveHeight + curveHeight;

            //  ȸ���� ������ŭ ��ä��ó�� �ο�
            float rotZ = offset * 5f;

            if (rt != null)
            {
                rt.anchoredPosition = new Vector2(x, appearYOffset);
                rt.localRotation = Quaternion.Euler(0, 0, rotZ); 
                cg.alpha = 0f;

                Sequence seq = DOTween.Sequence();
                seq.Append(rt.DOAnchorPos(new Vector2(x, y), 0.6f).SetEase(Ease.OutCubic));
                seq.Join(cg.DOFade(1f, 0.6f));
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
        hasSelected = true;

        

        // ī�� ȿ�� ����
        CardEffect effect = selected.GetComponent<CardEffect>();
        if ( effect != null)
        {
            GameObject player = GameObject.FindWithTag("Player");

            if (player != null)
            {
                effect.ApplyEffect(player);
            }
        }

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
                // ���õ��� ���� ī�� �� ȸ�� �������� �̵� + ��� + ���̵�ƿ�
                RectTransform rt = card.GetComponent<RectTransform>();
                CanvasGroup cg = card.GetComponent<CanvasGroup>();
                if (cg == null) cg = card.AddComponent<CanvasGroup>();

                float angleZ = rt.localEulerAngles.z; // rotZ
                Vector2 direction = Quaternion.Euler(0, 0, angleZ) * Vector2.up; // ȸ�� ���� ���� ����
                Vector2 targetPos = rt.anchoredPosition + direction * 400f;

                Sequence seq = DOTween.Sequence();
                seq.Join(rt.DOAnchorPos(targetPos, 2f).SetEase(Ease.InCubic)); // ���ư��� �̵�
                seq.Join(rt.DOScale(0.1f, 2f).SetEase(Ease.InCubic));           // �۾�����
                seq.Join(cg.DOFade(0f, 2f));
                seq.Join(rt.DOLocalRotate(new Vector3(180f, 180f, angleZ), 2f, RotateMode.FastBeyond360));
                seq.OnComplete(() => Destroy(card));
            }
        }

        Debug.Log("���õ� ī��: " + selected.name);
        Debug.Log("���� �� ���� �Ѿ�� ���� �ε� ����");


        // DOVirtual.DelayedCall(2f, () => SceneLoadingManager.Instance.AllowSceneActivation());

    }

    
}

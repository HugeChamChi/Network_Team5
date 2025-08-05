using DG.Tweening;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;

public class CardSelectManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private CanvasController canvasController;
    [SerializeField] private GameObject canvasActivation;
    [SerializeField] private CardSelectCheckManager cardSelectCheckManager;
    [SerializeField] private CardSelectPanelItem cardSelectPanelItem;
    FlipCard flipCard;

    [Header("��ü ī�� ������ ����Ʈ")]
    public List<GameObject> allCardPrefabs; // ���ӿ��� ����� ��ü ī�� ������ ���

    [Header("�θ� ���̾ƿ� �׷�")]
    public Transform cardSpawnParent1; // ������ ī�尡 ���� �θ�(ĵ���� �� ��ġ �����̳�)
    public Transform cardSpawnParent2;

    [Header("����� ī�� ����")]
    public int cardCountToShow = 3; // �� ���� ������ ī�� ����

    [Header("��ä�� ��ġ ����")]
    public float xSpacing = 300f; // ī�� �� X ���� ������
    public float curveHeight = 150f; // Y ��ġ�� �ó�� �ֱ� ���� ��
    public float maxAngle = 60f; // ȸ�� �ð� ����
    public float appearYOffset = -600f;

    private CardSceneArmController armController;

    [SerializeField] private GameObject masterCharacter;
    [SerializeField] private GameObject clientCharacter;

    [SerializeField] private CardSceneArmController masterArmController;
    [SerializeField] private CardSceneArmController clientArmController;

    [SerializeField] private CharacterShrinkEffect masterShrinkEffect;
    [SerializeField] private CharacterShrinkEffect clientShrinkEffect;

    [Header("�ɸ��� ũ�� ����")]
    [SerializeField] private CharacterShrinkEffect shrinkEffect;


    private List<GameObject> currentCards = new(); // ���� ȭ�鿡 ǥ�� ���� ī�� ���
    [SerializeField] private bool hasSelect = false; // �÷��̾ ī�带 �����ߴ��� ����


    void Start()
    {
        

        cardSelectCheckManager.cardSelectPanelSpawn();
        cardSelectCheckManager.CardSelectPanelSpawn(PhotonNetwork.LocalPlayer);

        UpdateCharacterVisibility();
        // SceneLoadingManager.Instance.LoadSceneAsync("Game Scene");
        // Debug.Log("���� �� ���� �Ѿ�� ���� �ε� ����");

        // if (PhotonNetwork.IsMasterClient)
        // {
        //     List<int> selectedMasterIndexes = GetRandomCardIndexes();
        //     photonView.RPC(nameof(RPC_SpawnCardsWithIndexes), RpcTarget.All, selectedMasterIndexes.ToArray());
        // }

    }
    public void UpdateCharacterVisibility()
    {
        bool isMaster = PhotonNetwork.IsMasterClient;
        bool masterCanvasActive = canvasController.IsMasterCanvasActive();
        bool clientCanvasActive = canvasController.IsClientCanvasActive();

        if (masterCanvasActive)
        {
            if (canvasController.IsMyTurn())
            {
                if (isMaster) ActivateMasterCharacter();     // ������ �� ������
                else ActivateClientCharacter();              // ������ �� ������
            }
            else
            {
                if (isMaster) ActivateClientCharacter();     // ������ �� ������
                else ActivateMasterCharacter();              // ������ �� ������
            }
        }
        else if (clientCanvasActive)
        {
            if (canvasController.IsMyTurn())
            {
                if (isMaster) ActivateClientCharacter();     // ������ �� ������
                else ActivateMasterCharacter();              // ������ �� ������
            }
            else
            {
                if (isMaster) ActivateMasterCharacter();     // ������ �� ������
                else ActivateClientCharacter();              // ������ �� ������
            }
        }
        else
        {
            masterCharacter.SetActive(false);
            clientCharacter.SetActive(false);
        }
    }
      public void ActivateMasterCharacter()
      {
          masterCharacter.SetActive(true);
          clientCharacter.SetActive(false);
    
          armController = masterArmController;
          shrinkEffect = masterShrinkEffect;
      }
    
      public void ActivateClientCharacter()
      {
          masterCharacter.SetActive(false);
          clientCharacter.SetActive(true);
    
          armController = clientArmController;
          shrinkEffect = clientShrinkEffect;
      }

    // private void Awake()
    // {
    //     PhotonNetwork.AutomaticallySyncScene = true;
    // }

    public CardSceneArmController GetArmController()
    {
        return armController;
    }

    [PunRPC]
    public void RPC_SelectCardArm(int index)
    {
        Debug.Log($"[CardSelectManager] ����Ʈ ī�� �� index = {index} ȣ���");
        armController.SelectCard(index);
    }

    // ���� ī�� ���� �� ȭ�鿡 ���
    public List<int> GetRandomCardIndexes()
    {
        List<int> indexes = new();
        while (indexes.Count < cardCountToShow)
        {
            int rand = Random.Range(0, allCardPrefabs.Count);
            if (!indexes.Contains(rand))
                indexes.Add(rand);
        }
        return indexes;
    }

   // [PunRPC]
   // public void RPC_SpawnCardsWithIndexes(int[] indexes)
   // {
   //     SpawnCardsFromIndexes(indexes, canvasController.IsMyTurn());
   // }

    public void SpawnCardsFromIndexes(int[] indexes, bool canInteract)
    {
      //  if (hasSelect)
      //  {
      //      Debug.Log("�̹� ī�� ���� �Ϸ� ���� �� ī�� ���� ��ŵ");
      //      return; // ������ �����ٸ� ī�� �ٽ� ����� ����
      //  }

        Debug.Log("ī�� ���� ����");
        currentCards.Clear();

        float centerIndex = (indexes.Length - 1) / 2f;

        for (int i = 0; i < indexes.Length; i++)
        {
            Debug.Log("�� �� ������ ������");
            GameObject card = Instantiate(allCardPrefabs[indexes[i]], cardSpawnParent1);
            RectTransform rt = card.GetComponent<RectTransform>();
            CanvasGroup cg = card.GetComponent<CanvasGroup>();
            if (cg == null) cg = card.AddComponent<CanvasGroup>();

            float offset = i - centerIndex;
            float x = offset * xSpacing;
            float y = -Mathf.Abs(offset) * curveHeight + curveHeight;
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
            {
                flip.SetManager(this);
                flip.SetCardIndex(i); // �ε��� ��� ����ȭ��
                flip.SetInteractable(canInteract);
            }

            currentCards.Add(card);
        }

    }

    public void SpawnClientCardsFromIndexes(int[] indexes, bool canInteract)
    {
       // if (hasSelect)
       // {
       //     Debug.Log("�̹� ī�� ���� �Ϸ� ���� �� ī�� ���� ��ŵ");
       //     return; // ������ �����ٸ� ī�� �ٽ� ����� ����
       // }

        Debug.Log("ī�� ���� ����");
        currentCards.Clear();

        float centerIndex = (indexes.Length - 1) / 2f;

        for (int i = 0; i < indexes.Length; i++)
        {
            Debug.Log("�� �� ������ ������");
            GameObject card = Instantiate(allCardPrefabs[indexes[i]], cardSpawnParent2);
            RectTransform rt = card.GetComponent<RectTransform>();
            CanvasGroup cg = card.GetComponent<CanvasGroup>();
            if (cg == null) cg = card.AddComponent<CanvasGroup>();

            float offset = i - centerIndex;
            float x = offset * xSpacing;
            float y = -Mathf.Abs(offset) * curveHeight + curveHeight;
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
            {
                flip.SetManager(this);
                flip.SetCardIndex(i); // �ε��� ��� ����ȭ��
                flip.SetInteractable(canInteract);
            }

            currentCards.Add(card);
        }

    }

    public List<GameObject> GetCurrentCards() => currentCards;

    [PunRPC]
    public void RPC_FlipCardByIndex(int index)
    {
        if (index >= 0 && index < currentCards.Count)
        {
            FlipCard flip = currentCards[index].GetComponent<FlipCard>();
            if (flip != null)
            {
                Debug.Log("ī�� ������ �ִϸ��̼� ����");
                flip.PlayFlipAnimation();
            }
        }
    }

    [PunRPC]
    public void RPC_HighlightCardByIndex(int index)
    {
        if (index < 0 || index >= currentCards.Count) return;

        GameObject card = currentCards[index];
        if (card == null) return;

        FlipCard flip = card.GetComponent<FlipCard>();
        if (flip == null) return;

        flip.PlayHighlight(); // Ȯ��� ����� �޼���
    }

    [PunRPC]
    public void RPC_UnhighlightCardByIndex(int index)
    {
        if (index < 0 || index >= currentCards.Count) return;

        GameObject card = currentCards[index];
        if (card == null) return;

        FlipCard flip = card.GetComponent<FlipCard>();
        if (flip == null) return;

        flip.PlayUnhighlight();
    }


    // ī�� �ϳ��� ���õǾ��� �� ȣ���
    public void OnCardSelected(GameObject selected)
    {
        Debug.Log($"[OnCardSelected] called | hasSelect: {hasSelect}");
        if (hasSelect) return;

        hasSelect = true;

        Debug.Log("�� ī�� ���� �Ϸ��");

        PhotonNetwork.AutomaticallySyncScene = true;

        if (cardSelectCheckManager.cardSelectPanels.TryGetValue(PhotonNetwork.LocalPlayer.ActorNumber, out CardSelectPanelItem panel))
        {
            panel.OnCardSelected();
            panel.SelectCheck(PhotonNetwork.LocalPlayer);
        }

        // ī�� ȿ�� ����
        CardEffect effect = selected.GetComponent<CardEffect>();
        if (effect != null)
        {
            GameObject playerObj = GameObject.FindWithTag("Player");
            if (playerObj != null)
            {
                PlayerStats playerStats = playerObj.GetComponent<PlayerStats>();
                if (playerStats != null)
                {
                    effect.ApplyShotEffect(playerStats);
                    effect.ApplyStatusEffect(playerStats);
                    Debug.Log($"[ī�� ����] {effect.cardName} ȿ���� ����Ǿ����ϴ�.");
                }
            }
        }

        // ���õ� ī�� �ε����� ���ؼ� RPC ȣ��
        int selectedIndex = currentCards.IndexOf(selected);
        photonView.RPC(nameof(RPC_PlayCardSelectionAnimation), RpcTarget.All, selectedIndex);

        Debug.Log("���õ� ī��: " + selected.name);
       // Debug.Log("���� ������ �Ѿ�� ���� �ε� ����");

        if (canvasController.IsMyTurn())
        {
            DOVirtual.DelayedCall(1f, () =>
            {
                if (cardSelectCheckManager.AllPlayerCardSelectCheck())
                {
                    if (PhotonNetwork.IsMasterClient) // �����͸� �� ��ȯ
                    {
                        Debug.Log("��� �÷��̾� ���� �Ϸ� �� Game Scene ��ȯ");
                        PhotonNetwork.LoadLevel("Game Scene");
                    }
                }
                else
                {
                    canvasController.photonView.RPC("RPC_SwitchTurnToOther", RpcTarget.All);
                }
            });
        }
    }

    [PunRPC]
    public void RPC_PlayCardSelectionAnimation(int selectedIndex)
    {
        for (int i = 0; i < currentCards.Count; i++)
        {
            GameObject card = currentCards[i];
            if (card == null) continue;

            RectTransform rt = card.GetComponent<RectTransform>();
            CanvasGroup cg = card.GetComponent<CanvasGroup>();
            if (cg == null) cg = card.AddComponent<CanvasGroup>();

            if (i == selectedIndex)
            {
                // ���õ� ī�� �� ���̵�ƿ� �� ����
                cg.DOFade(0f, 0.5f)
                  .SetEase(Ease.InOutSine)
                  .OnComplete(() => Destroy(card));
            }
            else
            {
                // ������ ī�� �� �־����鼭 ��� �� ����
                float angleZ = rt.localEulerAngles.z;
                Vector2 direction = Quaternion.Euler(0, 0, angleZ) * Vector2.up;
                Vector2 targetPos = rt.anchoredPosition + direction * 400f;

                Sequence seq = DOTween.Sequence();
                seq.Join(rt.DOAnchorPos(targetPos, 1f).SetEase(Ease.InCubic));
                seq.Join(rt.DOScale(0.1f, 1f).SetEase(Ease.InCubic));
                seq.Join(cg.DOFade(0f, 1f));
                seq.Join(rt.DOLocalRotate(new Vector3(180f, 180f, angleZ), 1f, RotateMode.FastBeyond360));
                seq.OnComplete(() => Destroy(card));
            }
        }

        shrinkEffect.RequestShrinkEffect();
    }

     public override void OnPlayerPropertiesUpdate(Player target, ExitGames.Client.Photon.Hashtable propertiesThatChanged)
     {
         base.OnPlayerPropertiesUpdate(target, propertiesThatChanged);
    
         if (propertiesThatChanged.ContainsKey("Select"))
         {
             cardSelectCheckManager.cardSelectPanels[target.ActorNumber].SelectCheck(target);
    
             // ��� �÷��̾� ī�� ���� �Ϸ� �� ���� �� ��ȯ (2�� ��)
             if (cardSelectCheckManager.AllPlayerCardSelectCheck())
             {
                 Debug.Log("��� �÷��̾� ī�� ���� �Ϸ� ");
    
                 DOVirtual.DelayedCall(1f, () =>
                 {
                     Debug.Log("���� ����� �� ���� ī�� ���� �غ�");

                     ResetCardSelectionState();

                     

                     // 1. CanvasController ���� ����
                     canvasController.ResetCardSelectionState();

                     DOVirtual.DelayedCall(0.2f, () => {
                         canvasController.DecideNextSelector();
                     });


                 });
             }
         }
     }

    public bool HasSelected()
    {
        return hasSelect;
    }


    public void ResetCardSelectionState()
    {
        Debug.Log("ī�弱�û�Ȳ �ʱ�ȭ");
        hasSelect = false;

        // ���� Canvas�� �ڽ� ī�� ������Ʈ ����
        foreach (Transform t in cardSpawnParent1)
            Destroy(t.gameObject);

        foreach (Transform t in cardSpawnParent2)
            Destroy(t.gameObject);

        ExitGames.Client.Photon.Hashtable props = new();
        props["Select"] = false;
        PhotonNetwork.LocalPlayer.SetCustomProperties(props);

        // ĳ���� ���α�
        masterCharacter.SetActive(false);
        clientCharacter.SetActive(false);

        foreach (var panel in cardSelectCheckManager.cardSelectPanels.Values)
        {
            panel.ResethasSelected();
        }
    }

}

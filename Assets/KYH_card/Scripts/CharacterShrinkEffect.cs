using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using DG.Tweening;

public class CharacterShrinkEffect : MonoBehaviourPun
{
    [SerializeField] private Transform characterTransform;

    private Vector3 originalScale;

    [Header("�ִϸ��̼� Ŀ��")]
    [SerializeField] private AnimationCurve shrinkCurve;   // 1�ʰ� ���


    private void Awake()
    {
        if (characterTransform == null)
        {
            Debug.LogError("Character Transform�� ������� �ʾҽ��ϴ�.");
            return;
        }

        originalScale = characterTransform.localScale;
    }

    private void OnEnable()
    {
        // ĳ���Ͱ� �ٽ� ������ �� ������ ����
        characterTransform.localScale = originalScale;
    }

    [PunRPC]
    public void RPC_PlayShrinkAnimation()
    {
        Vector3 shrinkScale = Vector3.one * 0.2f;

        Sequence seq = DOTween.Sequence();

        // Ŀ��Shrink�� 1�ʰ� �پ��
        seq.Append(characterTransform.DOScale(shrinkScale, 1f).SetEase(shrinkCurve));

        
    }

    public void RequestShrinkEffect()
    {
        photonView.RPC(nameof(RPC_PlayShrinkAnimation), RpcTarget.All);
    }
}


using DG.Tweening;
using Photon.Pun;
using System.Collections;
using UnityEngine;

/// <summary>
/// ������ ���� 3���� ���� �� ���� ���� �� �̵��ϰ� �ϴ� ��ũ��Ʈ
/// </summary>
public class MapController : MonoBehaviour
{
    [Header("Offset")]
    [Tooltip("�� ��ȯ ���� ������")]
    [SerializeField] private float mapChangeDelay = 0.8f;

    [SerializeField] private GameObject[] rounds;
    public float MapChangeDelay { get { return mapChangeDelay; } }

    private Coroutine moveCoroutine;

    private void OnEnable()
    {
        InGameManager.OnRoundStart += GoToNextStage;
    }

    private void OnDisable()
    {
        InGameManager.OnRoundStart -= GoToNextStage;
    }

    public void GoToNextStage()
    {
        MapShake();
        MapMove();
    }

    /// <summary>
    /// ���� ���� �� ���� �� �� ��鸲
    /// </summary>
    private void MapShake()
    {
        gameObject.transform.DOShakePosition(0.5f, 1, 10, 90);
    }

    /// <summary>
    /// ������ �ð� ���� ���� �������� ���۵�
    /// </summary>
    private void MapMove()
    {
        if (!PhotonNetwork.IsMasterClient) return;

        moveCoroutine = StartCoroutine(MovementCoroutine());

    }

    IEnumerator MovementCoroutine()
    {
        WaitForSeconds delay = new WaitForSeconds(mapChangeDelay);

        MapDynamicMovement[] movements = rounds[InGameManager.Instance.CurrentMatch].GetComponentsInChildren<MapDynamicMovement>();

        for (int i = 0; i < movements.Length; i++)
        {
            if (movements[i] != null)
            {

                movements[i].DynamicMove();
                yield return delay;
            }
        }

        moveCoroutine = null;
    }
}
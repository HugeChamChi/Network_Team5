using UnityEngine;
using DG.Tweening;
using System.Collections;

/// <summary>
/// ������ ���� 3���� ���� �� ���� ���� �� �̵��ϰ� �ϴ� ��ũ��Ʈ
/// </summary>
public class MapController : MonoBehaviour
{
    [Header("Offset")]
    [Tooltip("�� ��ȯ ���� ������")]
    [SerializeField] private float mapChangeDelay = 0.8f;
    public float MapChangeDelay { get { return mapChangeDelay; } }

    private Coroutine moveCoroutine;

    private void OnEnable()
    {
        TestIngameManager.OnRoundOver += GoToNextStage;
    }

    private void OnDisable()
    {
        TestIngameManager.OnRoundOver -= GoToNextStage;
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
        Debug.Log("����");
        moveCoroutine = StartCoroutine(MovementCoroutine());
    }

    IEnumerator MovementCoroutine()
    {
        WaitForSeconds delay = new WaitForSeconds(mapChangeDelay);

        MapDynamicMovement[] movements = GetComponentsInChildren<MapDynamicMovement>();
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
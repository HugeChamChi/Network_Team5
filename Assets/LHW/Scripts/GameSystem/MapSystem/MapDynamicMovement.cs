using DG.Tweening;
using UnityEngine;

/// <summary>
/// ���� ���� �� ���� �̵���ų ��, �� �������� ǥ���ϴ� ��ũ��Ʈ
/// </summary>
public class MapDynamicMovement : MonoBehaviour
{
    private MapController mapController;
    private RandomMapPresetCreator randomMapPresetCreator;

    [SerializeField] GameObject[] mapComponents;

    // ù ��° �÷����� �����̱� �����ϴ� ����(������)
    [SerializeField] float moveDelay = 1f;
    // �� �÷����� �̵��ϱ� �����ϴ� ����
    [SerializeField] float moveDurationOffset = 0.2f;

    private void OnEnable()
    {
        mapController = GetComponentInParent<MapController>();
        randomMapPresetCreator = GetComponentInParent<RandomMapPresetCreator>();
    }

    public void DynamicMove()
    {
        for(int i = 0; i < mapComponents.Length; i++)
        {
            float duration = moveDelay + (i * moveDurationOffset);
            mapComponents[i].transform.DOMove(mapComponents[i].transform.position + new Vector3(-randomMapPresetCreator.MapTransformOffset, 0, 0), duration)
                .SetDelay(mapController.MapChangeDelay).SetEase(Ease.InOutCirc);
        }
    }
}
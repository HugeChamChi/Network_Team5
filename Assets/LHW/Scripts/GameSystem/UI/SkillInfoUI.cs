using UnityEngine;
using UnityEngine.EventSystems;

// ����ȭ�鿡���� ǥ���ϸ� �Ǵ� UI�̹Ƿ� ����ȭ�� �ʿ伺�� ����
/// <summary>
/// ���콺�� ���ٴ��� �� �ڽ� �� ����� ��ų ������ ����ϴ� UI
/// </summary>
public class SkillInfoUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{ 
    public void OnPointerEnter(PointerEventData eventData)
    {
        // TODO : ī�� UI ǥ��
        Debug.Log("Info Activate");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // TODO : ī�� UI ��Ȱ��ȭ
        Debug.Log("Info Inactivate");
    }
}

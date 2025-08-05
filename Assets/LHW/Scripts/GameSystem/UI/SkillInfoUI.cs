using Photon.Pun;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// ���콺�� ���ٴ��� �� �ڽ� �� ����� ��ų ������ ����ϴ� UI
/// </summary>
public class SkillInfoUI : MonoBehaviourPun, IPointerEnterHandler, IPointerExitHandler
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

    [PunRPC]
    public void SetParentToPanel(int parentViewID)
    {
        Transform parent = PhotonView.Find(parentViewID).transform;
        transform.SetParent(parent);
    }
}

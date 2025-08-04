using UnityEngine;

public class CardSceneArmController : MonoBehaviour
{
    [SerializeField] CardSceneCharacterRightArm right;
    [SerializeField] CardSceneCharacterLeftArm left;

    public void SelectCard(int num)
    {
        Debug.Log($"[ArmController] �� ������ ȣ�� index = {num}");
        right.SelectCard(num);
        left.SelectCard(num);
    }
}

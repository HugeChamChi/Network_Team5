using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticCardInfoUI : MonoBehaviour
{
    [SerializeField] GameObject UIImagePrefab;

    [SerializeField] Transform leftPanelContent;
    [SerializeField] Transform rightPanelContent;
    [SerializeField] List<GameObject> leftUIImage = new List<GameObject>();
    [SerializeField] List<GameObject> rightUIImage = new List<GameObject>();

    string[] leftCardList;
    string[] rightCardList;

    private void OnEnable()
    {
        TestIngameManager.OnSkillObtained += ShowCardList;
    }

    private void OnDisable()
    {
        TestIngameManager.OnSkillObtained -= ShowCardList;
    }

    private void ShowCardList()
    {
        leftCardList = TestIngameManager.Instance.GetSkillInfo("Left");
        rightCardList = TestIngameManager.Instance.GetSkillInfo("Right");

        if(leftCardList.Length > leftUIImage.Count)
        {
            for(int i = leftUIImage.Count; i < leftCardList.Length; i++)
            {
                GameObject skillInfo = Instantiate(UIImagePrefab);
                skillInfo.transform.SetParent(leftPanelContent);
                // TODO : ī�� ���� �Է�
                leftUIImage.Add(skillInfo);
            }
        }

        if(rightCardList.Length > rightUIImage.Count)
        {
            for (int i = rightUIImage.Count; i < rightCardList.Length; i++)
            {
                GameObject skillInfo = Instantiate(UIImagePrefab);
                skillInfo.transform.SetParent(rightPanelContent);
                // TODO : ī�� ���� �Է�
                rightUIImage.Add(skillInfo);
            }
        }
    }
}

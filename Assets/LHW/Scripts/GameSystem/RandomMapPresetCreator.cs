using Photon.Pun;
using System.Security.Cryptography;
using UnityEngine;

public class RandomMapPresetCreator : MonoBehaviour
{
    // ���� Resources�� ������ �Ÿ� �ش� ������� ���� �ʿ�
    [SerializeField] GameObject[] mapResources;

    // �� ��ġ ������
    [SerializeField] float mapTransformOffset = 35;    

    // ���� ���� ��
    [SerializeField] int gameCycleNum = 3;

    private WeightedRandom<GameObject> mapWeightedRandom = new WeightedRandom<GameObject>();

    private void OnEnable()
    {
        RandomInit();
        RandomMapSelect();
    }

    private void RandomInit()
    {
        for(int i = 0; i < mapResources.Length; i++)
        {
            mapWeightedRandom.Add(mapResources[i], 1);
        }
    }

    private void RandomMapSelect()
    {
        for(int i = 0; i < gameCycleNum; i++)
        {
            GameObject selectedMap = mapWeightedRandom.GetRandomItemBySub();
            Vector3 selectedMapPosition = new Vector3(i * mapTransformOffset, 0, 5);
            //PhotonNetwork.Instantiate(selectedMap.name, selectedMapPosition, Quaternion.identity);
            Instantiate(selectedMap, selectedMapPosition, Quaternion.identity);
        }
    }

    public float GetTransformOffset()
    {
        return mapTransformOffset;
    }
}
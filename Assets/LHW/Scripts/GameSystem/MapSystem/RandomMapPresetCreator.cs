using UnityEngine;
using Photon.Pun;

public class RandomMapPresetCreator : MonoBehaviour
{
    // ���� Resources�� ������ �Ÿ� �ش� ������� ���� �ʿ�
    [SerializeField] GameObject[] mapResources;

    // �� ��ġ ������
    [SerializeField] private float mapTransformOffset = 35;
    public float MapTransformOffset { get { return mapTransformOffset; } }

    // ���� ���� ��
    [SerializeField] int gameCycleNum = 3;

    [SerializeField] Transform[] mapListTransform;

    private WeightedRandom<GameObject> mapWeightedRandom = new WeightedRandom<GameObject>();

    private void Awake()
    {
        for (int i = 0; i < mapListTransform.Length; i++)
        {
            RandomInit();
            RandomMapSelect(i);
        }
    }

    /// <summary>
    /// ���� Ȯ�� �ʱ� ����
    /// </summary>
    private void RandomInit()
    {
        for(int i = 0; i < mapResources.Length; i++)
        {
            mapWeightedRandom.Add(mapResources[i], 1);
        }
    }

    /// <summary>
    /// ���� �� ���� - �� �� ������ ���� ���� Ȯ������ �ƿ� ���ܵǹǷ� ���� �ߺ����� �ʰ� ��
    /// </summary>
    private void RandomMapSelect(int round)
    {
        for(int i = 0; i < gameCycleNum; i++)
        {
            GameObject selectedMap = mapWeightedRandom.GetRandomItemBySub();
            Vector3 selectedMapPosition = new Vector3((i + 1) * mapTransformOffset, 0, 5);
            //GameObject map =  PhotonNetwork.Instantiate(selectedMap.name, selectedMapPosition, Quaternion.identity);
            GameObject map = Instantiate(selectedMap, selectedMapPosition, Quaternion.identity);
            map.transform.SetParent(mapListTransform[round]);
        }
    }

    public void MapUpdate(int round)
    {
        Debug.Log(round);
        mapListTransform[round - 1].gameObject.SetActive(false);
        mapListTransform[round].gameObject.SetActive(true);
    }
}
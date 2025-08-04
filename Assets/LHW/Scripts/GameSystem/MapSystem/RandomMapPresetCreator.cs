using Photon.Pun;
using UnityEngine;

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

    private void OnEnable()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            for (int i = 0; i < mapListTransform.Length; i++)
            {
                RandomInit();
                RandomMapSelect(i);
            }
        }
    }

    /// <summary>
    /// ���� Ȯ�� �ʱ� ����
    /// </summary>
    private void RandomInit()
    {
        for (int i = 0; i < mapResources.Length; i++)
        {
            mapWeightedRandom.Add(mapResources[i], 1);
        }
    }

    /// <summary>
    /// ���� �� ���� - �� �� ������ ���� ���� Ȯ������ �ƿ� ���ܵǹǷ� ���� �ߺ����� �ʰ� ��
    /// </summary>
    private void RandomMapSelect(int round)
    {
        for (int i = 0; i < gameCycleNum; i++)
        {
            GameObject selectedMap = mapWeightedRandom.GetRandomItemBySub();
            Vector3 selectedMapPosition = new Vector3((i + 1) * mapTransformOffset, 0, 5);
            GameObject map = PhotonNetwork.Instantiate(selectedMap.name, selectedMapPosition, Quaternion.identity);
            //GameObject map = Instantiate(selectedMap, selectedMapPosition, Quaternion.identity);
            map.transform.SetParent(mapListTransform[round]);

            PhotonView mapView = map.GetComponent<PhotonView>();
            mapView.RPC("SetParentToRound", RpcTarget.OthersBuffered, round);            
        }
        MapUpdate(TestIngameManager.Instance.CurrentGameRound);
    }

    public Transform GetRoundTransform(int round)
    {
        return mapListTransform[round];
    }

    public void MapUpdate(int round)
    {
        if (TestIngameManager.Instance.IsGameOver) return;
        for(int i =0; i < mapListTransform.Length; i++)
        {
            PhotonView MapView = mapListTransform[i].GetComponent<PhotonView>();
            if(round == i)
            {
                MapView.RPC("RoundActivate", RpcTarget.AllBuffered, true);
            }
            else
            {
                MapView.RPC("RoundActivate", RpcTarget.AllBuffered, false);
            }
        }
    }
}
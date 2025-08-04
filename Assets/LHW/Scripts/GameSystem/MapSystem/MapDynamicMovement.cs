using DG.Tweening;
using Photon.Pun;
using UnityEngine;

/// <summary>
/// ���� ���� �� ���� �̵���ų ��, �� �������� ǥ���ϴ� ��ũ��Ʈ
/// </summary>
public class MapDynamicMovement : MonoBehaviourPun, IPunObservable
{
    private MapController mapController;
    private RandomMapPresetCreator randomMapPresetCreator;

    [SerializeField] GameObject[] mapComponents;

    // ù ��° �÷����� �����̱� �����ϴ� ����(������)
    [SerializeField] float moveDelay = 1f;
    // �� �÷����� �̵��ϱ� �����ϴ� ����
    [SerializeField] float moveDurationOffset = 0.2f;

    private Vector3 networkPos;
    private Quaternion networkRot;
    private bool networkActiveSelf;

    private void Start()
    {
        mapController = GetComponentInParent<MapController>();
        randomMapPresetCreator = GetComponentInParent<RandomMapPresetCreator>();
    }

    private void Update()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            transform.position = Vector3.Lerp(transform.position, networkPos, Time.deltaTime * 10f);
        }
    }

    public void DynamicMove()
    {
        photonView.RPC(nameof(RPC_DynamicMove), RpcTarget.All);
    }

    [PunRPC]
    public void RPC_DynamicMove()
    {
        for (int i = 0; i < mapComponents.Length; i++)
        {
            float duration = moveDelay + (i * moveDurationOffset);
            mapComponents[i].transform.DOMove(mapComponents[i].transform.position + new Vector3(-randomMapPresetCreator.MapTransformOffset, 0, 0), duration)
                .SetDelay(mapController.MapChangeDelay).SetEase(Ease.InOutCirc);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting && PhotonNetwork.IsMasterClient)
        {
            stream.SendNext(transform.position);
        }
        else if (stream.IsReading)
        {
            networkPos = (Vector3)stream.ReceiveNext();
        }
    }

    [PunRPC]
    public void SetParentToRound(int round)
    {
        Transform roundParent = FindObjectOfType<RandomMapPresetCreator>().GetRoundTransform(round);
        transform.SetParent(roundParent);
    }
}
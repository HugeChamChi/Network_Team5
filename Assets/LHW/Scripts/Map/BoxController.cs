using Photon.Pun;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// �Ѿ˿� �ǰݵǴ� �ڽ� �÷����� �ൿ�� ������. (���� Joint�� ���)
/// </summary>
public class BoxController : MonoBehaviourPun, IPunObservable
{
    FixedJoint2D fixedJoint;

    private Rigidbody2D rigid;

    // ��Ʈ��ũ ����ȭ�� ����
    private bool isPhysicsEnabled = false;
    private bool networkPhysicsEnabled = false;
    private Vector3 networkPos;
    private Quaternion networkRot;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        fixedJoint = GetComponent<FixedJoint2D>();
    }

    private void Update()
    {
        // �ڽŰ� ����� Joint (�ڽź��� �Ʒ��� �ִ� ������Ʈ) �� �ı��Ǿ��� ��� ���� Ȱ��ȭ
        if (photonView.IsMine &&
            (fixedJoint != null && fixedJoint.connectedBody != null && fixedJoint.connectedBody.bodyType == RigidbodyType2D.Dynamic))
        {
            EnablePhysics();
        }
        if (!photonView.IsMine)
        {
            if (isPhysicsEnabled)
            {
                transform.position = Vector3.Lerp(transform.position, networkPos, Time.deltaTime * 10f);
                transform.rotation = Quaternion.Lerp(transform.rotation, networkRot, Time.deltaTime * 10f);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // �Ѿ� Ȥ�� �ظӿ� �ǰݵǾ��� �� ���� Ȱ��ȭ
        if (photonView.IsMine && (collision.gameObject.CompareTag("Bullet") || collision.gameObject.CompareTag("Hammer")))
        {
            EnablePhysics();
        }
    }

    /// <summary>
    /// ���� Ȱ��ȭ
    /// </summary>
    private void EnablePhysics()
    {
        networkPhysicsEnabled = true;

        // ���� ������ ������ Ŭ���̾�Ʈ�� �ϸ�,
        // ��� �÷��̾�� ���� ���¸� Kinematic���� ������ ä ���� ��ȭ�� �о���� ������� ����ȭ
        // �÷��̾ ���� ���� ������ �ϴ� ���� �����ϱ� ����
        if (PhotonNetwork.IsMasterClient)
        {
            rigid.bodyType = RigidbodyType2D.Dynamic;
            rigid.mass = 0.1f;
            rigid.gravityScale = 0.3f;
            isPhysicsEnabled = true;
        }
    }

    
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
            stream.SendNext(isPhysicsEnabled);
            if (isPhysicsEnabled)
            {
                stream.SendNext(rigid.velocity);
                stream.SendNext(rigid.angularVelocity);
            }
        }
        else if (stream.IsReading)
        {
            networkPos = (Vector3)stream.ReceiveNext();
            networkRot = (Quaternion)stream.ReceiveNext();
            networkPhysicsEnabled = (bool)stream.ReceiveNext();
            // ���� ��Ʈ��ũ�� ���� Ȱ��ȭ�� �����Ǿ��� ��� �� �� �� üũ
            if (networkPhysicsEnabled != isPhysicsEnabled)
            {
                isPhysicsEnabled = networkPhysicsEnabled;
            }

            if (isPhysicsEnabled)
            {
                Vector2 networkVelocity = (Vector2)stream.ReceiveNext();
                float networkAngularVelocity = (float)stream.ReceiveNext();

                if (!photonView.IsMine)
                {
                    rigid.velocity = networkVelocity;
                    rigid.angularVelocity = networkAngularVelocity;
                }
            }
        }
    }
}
using UnityEngine;

public class RopeCreator : MonoBehaviour
{
    [SerializeField] Rigidbody2D hook;
    [SerializeField] GameObject linkPrefab;
    [SerializeField] RopeTiedObject objectWeight;
    [SerializeField] int links = 7;

    private GameObject link;
    private bool IsCreated = false;

    private void Awake()
    {
        GenerateRope();
    }

    private void OnEnable()
    {
        if(IsCreated == false)
        {
            GenerateRope();
        }
    }

    /// <summary>
    /// ���� ��ũ �������� �ν����Ϳ� �Է��� Links ����ŭ �����ϰ� Hinge Joint�� ����
    /// </summary>
    private void GenerateRope()
    {
        Rigidbody2D previousRB = hook;

        for(int i = 0; i < links; i++)
        {
            link = Instantiate(linkPrefab, transform);
            HingeJoint2D joint = link.GetComponent<HingeJoint2D>();
            joint.connectedBody = previousRB;
            previousRB = link.GetComponent<Rigidbody2D>();
        }
        objectWeight.ConnectRopeEnd(link.GetComponent<Rigidbody2D>());
        IsCreated = true;
    }
}
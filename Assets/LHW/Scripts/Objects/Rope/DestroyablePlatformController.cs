using UnityEngine;

public class DestroyablePlatformController : MonoBehaviour
{
    [SerializeField] RopeController rope;
    [SerializeField] RopeTiedObject tiedObject;

    /// <summary>
    /// �Ѿ˿� ���� �� ������Ʈ�� �ı��Ǿ��� �� ���� ��� �۵�
    /// ������ ���� ������Ʈ�� ������ Ȱ��ȭ�ϰ� �����ϰ� �ִ� ������ ��Ȱ��ȭ��
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            gameObject.SetActive(false);
            tiedObject.EnablePhysics();
            rope?.RopeDestroy();
        }
    }
}
using UnityEngine;

public class OutArea : MonoBehaviour
{
    /// <summary>
    /// �÷��̾ �� �ٱ����� ������ ��� �������� ��
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // TODO : �÷��̾� ������
        }
    }
}
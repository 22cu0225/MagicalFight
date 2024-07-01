using UnityEngine;

public class Swinging : MonoBehaviour
{
    // �ϐ��錾
    private Vector3 firstPosition;                                      // �����ʒu
    [SerializeField] Vector3 swingingSpeed = new Vector3(5,0,0);        // �ړ�����
    [SerializeField] Vector3 speed;                                     // ���x

    void Start()
    {
        firstPosition = transform.position;
    }

    private void Update()
    {
        // sin���g�p�������^��
        transform.position = new Vector3(firstPosition.x + Mathf.Sin(2 * Mathf.PI * speed.x * Time.time) * swingingSpeed.x,
                                         firstPosition.y + Mathf.Sin(2 * Mathf.PI * speed.y * Time.time) * swingingSpeed.y,
                                         firstPosition.z + Mathf.Sin(2 * Mathf.PI * speed.z * Time.time) * swingingSpeed.z);
    }
}

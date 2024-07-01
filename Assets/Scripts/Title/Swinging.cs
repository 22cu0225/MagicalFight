using UnityEngine;

public class Swinging : MonoBehaviour
{
    // 変数宣言
    private Vector3 firstPosition;                                      // 初期位置
    [SerializeField] Vector3 swingingSpeed = new Vector3(5,0,0);        // 移動距離
    [SerializeField] Vector3 speed;                                     // 速度

    void Start()
    {
        firstPosition = transform.position;
    }

    private void Update()
    {
        // sinを使用し往復運動
        transform.position = new Vector3(firstPosition.x + Mathf.Sin(2 * Mathf.PI * speed.x * Time.time) * swingingSpeed.x,
                                         firstPosition.y + Mathf.Sin(2 * Mathf.PI * speed.y * Time.time) * swingingSpeed.y,
                                         firstPosition.z + Mathf.Sin(2 * Mathf.PI * speed.z * Time.time) * swingingSpeed.z);
    }
}

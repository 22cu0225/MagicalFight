using UnityEngine;

public class Swinging : MonoBehaviour
{
    // ฯ้พ
    private Vector3 firstPosition;                                      // ๚สu
    [SerializeField] Vector3 swingingSpeed = new Vector3(5,0,0);        // ฺฎฃ
    [SerializeField] Vector3 speed;                                     // ฌx

    void Start()
    {
        firstPosition = transform.position;
    }

    private void Update()
    {
        // sin๐gpต^ฎ
        transform.position = new Vector3(firstPosition.x + Mathf.Sin(2 * Mathf.PI * speed.x * Time.time) * swingingSpeed.x,
                                         firstPosition.y + Mathf.Sin(2 * Mathf.PI * speed.y * Time.time) * swingingSpeed.y,
                                         firstPosition.z + Mathf.Sin(2 * Mathf.PI * speed.z * Time.time) * swingingSpeed.z);
    }
}

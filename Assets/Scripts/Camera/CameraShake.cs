using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [Header("デバック用　ホイールクリック")]
    [SerializeField] float dul = 0.3f;
    [SerializeField] float mag = 0.6f;

    private void Update()
    {
        // Debug Key
        if(Input.GetMouseButtonDown(2))
        {
            StartCoroutine(Shake(dul, mag));
        }
    }

    /// <summary>
    /// カメラを揺らす処理
    /// </summary>
    /// <param name="duration">期間</param>
    /// <param name="magnitude">揺れの大きさ</param>
    /// <returns></returns>
    public IEnumerator Shake(float duration, float magnitude)
    {
        // 初期位置
        Vector3 originalPosition = transform.position;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            transform.position = originalPosition + Random.insideUnitSphere * magnitude;
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.position = originalPosition;
    }

}

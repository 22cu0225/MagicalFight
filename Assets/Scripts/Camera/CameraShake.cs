using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [Header("�f�o�b�N�p�@�z�C�[���N���b�N")]
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
    /// �J������h�炷����
    /// </summary>
    /// <param name="duration">����</param>
    /// <param name="magnitude">�h��̑傫��</param>
    /// <returns></returns>
    public IEnumerator Shake(float duration, float magnitude)
    {
        // �����ʒu
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

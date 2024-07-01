using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamegeAction : MonoBehaviour
{
    // ノックバック力
    [SerializeField] private float SideknockBackPower = 0;  // 左右
    [SerializeField] private float UpknockBackPower = 0;    // 上下

    public IEnumerator KnockBack(GameObject traget,float direction,float waitTime)
    {
        Rigidbody2D tragetRigitbody = traget.GetComponent<Rigidbody2D>();
        yield return new WaitForSeconds(waitTime);
        tragetRigitbody.velocity = Vector2.right * (direction + SideknockBackPower) + Vector2.up * (direction + UpknockBackPower);
    }
}

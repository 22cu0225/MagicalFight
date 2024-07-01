using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionCheck : MonoBehaviour
{
    // 変数宣言
    private string ObjTag;      // 接触したオブジェクトについているタグ名を入れる変数

    // 接触時
    private void OnCollisionEnter2D(Collision2D collision)
    {
        {
            // 接触したオブジェクトのタグをチェック
            ObjTag = collision.gameObject.tag;

            Debug.Log("ObjTag:" + ObjTag);
        }
    }

    // 離れた時
    private void OnCollisionExit2D(Collision2D collision)
    {
        // 取得したタグ名を破棄
        ObjTag = null;

        Debug.Log("ObjTag:Discard");
    }

    // 接触中のオブジェクトのタグ名をString型で返す関数
    public string GetCollisionObjectTag()
    {
        if(ObjTag != null)
        {
            return ObjTag;
        }
        else
        {
            return null;
        }
    }



}

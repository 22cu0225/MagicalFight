using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstanceObjectManager : MonoBehaviour
{
    // プロパティ
    // -------------------------------------------------------------------------------------------------------------------

    // オブジェクトの生成パターンを設定する列挙型変数
    public enum InstantiatePattrn
    {
        [InspectorName("ゲームオブジェクトとして標準生成")]
        Standard = 0,
        [InspectorName("UIとして表示するためCanvas上に生成")]
        UI
    }

    // 生成するオブジェクトと生成の仕方を設定する構造体
    [System.Serializable]
    public struct Representative
    {
        [Header("DisplayObjに、オブジェクトとして生成したいプレハブを入れる")]
        public GameObject DisplayObj;           // 生成物
        public InstantiatePattrn Pattrn;        // 生成パターン

        [Header("座標系の設定(UIはPositionのみ有効)")]
        public Vector3 DisplayPosition;         // 生成座標
        public Vector3 DisplayRotation;         // 生成角度(主にZを使用)
        public Vector3 DisplayScale;            // 生成大きさ
    };


    // 複数オブジェクトのインスタンス化を行えるように構造体配列にする
    [SerializeField] public Representative[] InstanceManager;

    // Canvasトランスフォーム用変数(インスペクター上でCanvasを入れて使う)
    [SerializeField] private Transform CanvasTransform;

    // -------------------------------------------------------------------------------------------------------------------
    // メソッド
    // -------------------------------------------------------------------------------------------------------------------

    // オブジェクトをインスタンス化(生成)する処理（生成時に、各オブジェクトの SetActive を false にするため、表示したいときは、ObjectSetActive()を使って、Active状態をTrueにする）
    public void InstanceObject()
    {
        for (int i = 0; i < InstanceManager.Length; i++)      // オブジェクト生成用構造体の要素数を取得して、要素分だけ生成する
        {
            switch (InstanceManager[i].Pattrn)      // 設定した生成方法に応じたオブジェクト生成
            {
                case InstantiatePattrn.Standard:    // 標準生成

                    InstanceManager[i].DisplayObj = Instantiate(InstanceManager[i].DisplayObj, InstanceManager[i].DisplayPosition, Quaternion.Euler(InstanceManager[i].DisplayRotation));
                    InstanceManager[i].DisplayObj.transform.localScale = InstanceManager[i].DisplayScale;
                    
                    ObjectSetActive(InstanceManager[i].DisplayObj, false);                    

                    Debug.Log("Standard");

                    break;

                case InstantiatePattrn.UI:      // UIとしてCanvas上に生成

                    InstanceManager[i].DisplayObj = Instantiate(InstanceManager[i].DisplayObj, CanvasTransform, false);
                    InstanceManager[i].DisplayObj.transform.localPosition = InstanceManager[i].DisplayPosition;

                    ObjectSetActive(InstanceManager[i].DisplayObj, false);

                    Debug.Log("UI");

                    break;
            }

            Debug.Log("オブジェクトを生成した");
        }
    }

    // 指定したオブジェクトのActiveを切り替える処理（引数：切り替えるオブジェクト、trueかfalseか）
    public void ObjectSetActive(GameObject setObj, bool setActive)
    {
        if(setObj.activeSelf != setActive)
        {
            setObj.SetActive(setActive);
            Debug.Log("指定したオブジェクト：" + setObj + "SetActive：" + setActive);
        }
        else
        {
            Debug.Log("このオブジェクトは、既に " + setActive + " です");
        }        
    }

    // 指定したオブジェクトの座標を変更する処理
    public void ObjectPos(GameObject setObj, Vector3 pos)
    {
        setObj.transform.position = pos;
    }

    // インスタンス化したオブジェクトを他スクリプト上で変数として取得させる処理(引数：取得したい生成オブジェクトの要素番号)
    public GameObject GetInstansedObject(int InstanceNum)
    {
        return InstanceManager[InstanceNum].DisplayObj;
    }
}

// メモ
// インスペクター上で設定したオブジェクトのインスタンス化処理は完了
// 次に、別スクリプトで、勝利判定を取って中間リザルトを表示する処理を作る
//
//
//
// 2024/01/19
// 更新：UIオブジェクトの生成の際、表示する座標を設定できるように修正
// 　　　UIオブジェクト生成処理記述を簡潔になるよう更新
//
// 2024/01/26
// 修正：コードのコメントを一部変更
// 



// UI生成処理バックアップ
/*
                    GameObject parent = GameObject.Find("Canvas");
                    if (parent != null)
                    {
                        Debug.Log("ObjectFind:Succese");
                        InstanceManager[i].DisplayObj = Instantiate(InstanceManager[i].DisplayObj, parent.transform.position, Quaternion.identity);
                        InstanceManager[i].DisplayObj.transform.SetParent(parent.transform, false);

                        ObjectSetActive(InstanceManager[i].DisplayObj, false);

                    }
                    else
                    {
                        Debug.Log("ObjectFind:Failed");
                    }

                    Debug.Log("UI");

                    break;

*/
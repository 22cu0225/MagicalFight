using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetSingleton : MonoBehaviour
{
    private static SetSingleton instance;
    public static int WinnerNum;            // 勝利者の番号
    public static int WinnerKillCount;      // 勝利者の撃破数

    // Start is called before the first frame update
    private void Awake()
    {
        CheckInstance();
    }

    private void CheckInstance()
    {
        // 自身をインスタンス化、シーンをまたいでも破棄されないようにする（DontDestroyOnLoad）
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
        // 既に自身が存在する場合、自身を破棄する
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetWinnerNum(int Num)
    {
        WinnerNum = Num;
    }

    public int GetWinnerNum()
    {
        return WinnerNum;
    }

    public void SetWinnerKillCount(int Num)
    {
        WinnerKillCount = Num;
    }

    public int GetWinnerKillCount()
    {
        return WinnerKillCount;
    }
}

// 2024/01/15 特定のシーンに遷移した際にシーン遷移用オブジェクトが増殖してしまう現象の修正
//          　変数 inctance に static を追加
// 2024/01/29
// 更新：Ingame終了時、勝利したプレイヤーの要素番号をstatic変数に格納する処理、戻り値として返す処理を追加
// 　　　勝利したプレイヤーの番号を使う場合はこのスクリプトを変数で取得し、GetWinnerNum()を使う
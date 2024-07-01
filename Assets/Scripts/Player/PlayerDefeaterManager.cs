using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDefeaterManager : MonoBehaviour
{
    // やられたプレイヤーと撃破者を受け取る変数
    [HideInInspector] public string DefeaterRecord;       // 撃破者の名前
    [HideInInspector] public string VictimRecord;         // やられたプレイヤーの名前

    // やられたプレイヤーと撃破者を紐づけて管理する構造体の宣言
    [HideInInspector] public struct RecordOfDefeats
    {
        public string Deferter;
        public string Victim;
    }
 

    // やられたプレイヤーと撃破者を紐づけて管理する構造体
    [HideInInspector] public List<RecordOfDefeats> Record;

    // Start is called before the first frame update
    void Start()
    {
        // 変数、構造体の初期化
        DefeaterRecord = null;
        VictimRecord = null;

        Record = new List<RecordOfDefeats>();
    }

    // Update is called once per frame
    void Update()
    {
        // 誰かが撃破され、名前情報が送られてきたら、名前情報を管理構造体に格納する（Player内のOnTreeger~~から名前が送られてきたら処理が実行される）
        if(DefeaterRecord != null && VictimRecord != null)
        {
            RecordAddProcessing();
        }

        DebugProcessing();

    }

    // 誰かが撃破され、名前情報が送られてきたら、名前情報を管理構造体に格納する関数
    private void RecordAddProcessing()
    {
        // デバッグ用：この処理が開始された時ログを出力
        Debug.Log("ProcStart");

        // 仮記憶用構造体に名前情報を格納
        RecordOfDefeats Temporary = new RecordOfDefeats();
        Temporary.Deferter = DefeaterRecord;
        Temporary.Victim = VictimRecord;

        // 記憶用構造体に入れる
        Record.Add(Temporary);

        // 構造体に格納した後は、変数を初期化する
        DefeaterRecord = null;
        VictimRecord = null;
    }

    // 勝利者の撃破数を数え、Static変数に格納する関数
    public void KillCountProcessing(string WinnerName)
    {
        int WinnerKillCount = 0;

        // プレイヤー撃破情報管理構造体から、勝利者の撃破数を抽出
        foreach(RecordOfDefeats KillCount in Record)
        {
            if(KillCount.Deferter == WinnerName)
            {
                WinnerKillCount++;
            }
        }

        // 総撃破数と勝利者の撃破数をデバッグログに出力
        Debug.Log("AllKillCount：" + Record.Count);
        Debug.Log(WinnerName + "：KillCount = " + WinnerKillCount);

        // SetSingleton内のStatic変数に勝利者の撃破数を格納
        GameObject.Find("SceneManager").GetComponent<SetSingleton>().SetWinnerKillCount(WinnerKillCount);
    }


    private void DebugProcessing()
    {
        // List配列の現在の要素数と格納している名前を出力（現在の撃破記録をデバッグログに出力）
        if (Input.GetKeyDown(KeyCode.G))
        {
            Debug.Log("ListCount：" + Record.Count);
            foreach (RecordOfDefeats output in Record)
            {
                Debug.Log("defeart " + output.Deferter + " in " + output.Victim);
            }
        }
    }

}

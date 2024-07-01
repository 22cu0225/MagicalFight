using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // 勝ち点上限
    public int maxWinPoints;
    public int GetMaxWinPoints() { return maxWinPoints; }

    // Playerの人数を格納する変数
    public int playerCount;
    public int GetPlayerCount() { return playerCount; }

    // 各playerの勝利数を格納する配列
    List<int> winsCounterList = new List<int>();
    // 各playerの勝利数を格納する配列
    List<bool> isAliveCheckList = new List<bool>();

    PlayerGenerate PlayerGenerateScript;

    // ステージのPrefabを格納する配列
    [SerializeField] private　List<GameObject> stageList = new List<GameObject>();

    private void Awake()
    {
        playerCount = PlayerSetting.playerSettings[0];
        maxWinPoints = PlayerSetting.playerSettings[1];
    }

    void Start()
    {
        // Player の人数を取得
        Debug.Log("Count of players :" + playerCount);

        PlayerGenerateScript = this.gameObject.GetComponent<PlayerGenerate>();

        // プレイヤーごとに勝利数を初期化
        //for (int i = 0; i < playerCount;i++) winsCounterList.Add(0);
        //foreach (GameObject player in PGScript.PlayerList)
        //{
        //    isAliveCheckList.Add(player.GetComponent<PlayerDB>().MyIsAlive);
        //    winsCounterList.Add(player.GetComponent<PlayerDB>().MyWinPoints);
        //}
        Instantiate(stageList[0], Vector3.zero, transform.rotation);
    }

    void Update()
    {
        //if(プレイヤーの人数が一人になった時) 
        //{
        //    勝利数をカウントする（各々の勝ち点を加算）
        //    ゲームを再スタートさせる
        //}

        //// （デバッグ用）勝ち点が追加された時の処理
        //if (Input.GetKeyDown(KeyCode.K))
        //{
        //    winsCounterList[0]++;
        //    Debug.Log("Count of 1P's winPoins :" + winsCounterList[0]);
        //    Debug.Log("Count of 2P's winPoins :" + winsCounterList[1]);
        //    MidResult();
        //}
        //if (Input.GetKeyDown(KeyCode.L))
        //{
        //    winsCounterList[1]++;
        //    Debug.Log("Count of 1P's winPoins :" + winsCounterList[0]);
        //    Debug.Log("Count of 2P's winPoins :" + winsCounterList[1]);
        //    MidResult();
        //}


    }

    /*
    public void MidResult()
    {
        // 現在のステージを移動する
        // 結果を表示する
        // 次のステージをロードする
        Debug.Log("Display the MidResults screen. ");


        int playerNum = -1;        // 中間チャンピョンの要素番号を格納する変数

        // 中間チャンピョンの要素番号を格納する。
        foreach (GameObject player in PlayerGenerateScript.GetPlayerList())
        {
            playerNum++;
            //生存しているプレイヤーを見つけた場合にループを抜ける
            if (player.GetComponent<PlayerDB>().MyIsAlive != false) break;
        }
        Debug.Log(playerNum);

        // playerNumにチャンピョンの要素番号が格納された場合の処理
        if (playerNum >= 0 && playerNum < playerCount)
        {
            //現在の勝ち点から加算
            int winPoints = PlayerGenerateScript.GetPlayerList()[playerNum].GetComponent<PlayerDB>().MyWinPoints + 1;
            //勝ち点を反映
            PlayerGenerateScript.GetPlayerList()[playerNum].GetComponent<PlayerDB>().SetWinPoints(winPoints);
            Debug.Log(PlayerGenerateScript.GetPlayerList()[playerNum] + "の勝ち点： " + winPoints);
        }
        //勝ち点が上限に達したときの処理
        if (PlayerGenerateScript.GetPlayerList()[playerNum].GetComponent<PlayerDB>().MyWinPoints == maxWinPoints)
        {
            FinalResult();        // リザルト表示が終了したら呼ぶ

        }
    }
    */

    void FinalResult()
    {
        //int playerNum;        // チャンピョンの要素番号を格納する変数

        //// チャンピョンの要素番号を格納する。チャンピョン出現前は-1を返す
        //playerNum = winsCounterList.IndexOf(MaxWinPoints);

        //// playerNumにチャンピョンの要素番号が格納された場合の処理
        //if(playerNum != -1)
        //{
        //    // リザルト画面に遷移する
        //    SceneManager.LoadScene("FinalResult");
        //}

        Debug.Log("Display the FinalResults screen. ");

    }
}

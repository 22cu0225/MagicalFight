using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadManager : MonoBehaviour
{
    // プロパティ
    // ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

    // SetSingletonスクリプトを入れる変数
    SetSingleton setSingleton;

    // シーン遷移条件を管理する列挙型
    public enum SceneLoadConditions
    {
        [InspectorName("勝利数で切り替え")]
        WinCount,
        [InspectorName("ボタン入力で切り替え")]
        Button,
        [InspectorName("時間経過で切り替え")]
        TimeCount
    }


    // シーンの遷移の条件、遷移先を管理する構造体(SceneIndexと値を一致させる)
    // 下記：シーン一覧
    // Title       0
    // RuleSeting  1
    // Ingame      2
    // MidResult   3
    // FinalResult 4
    [System.Serializable]
    public struct SceneChangeManager
    {
        [Header("このシーンの遷移条件の設定")]
        [Header("Elementの番号 = 各シーンのBuildIndexの番号")]
        [Header("現在のシーンの遷移条件の設定")] public SceneLoadConditions LoadCondition;       // この列挙型変数でシーン遷移の条件を切り替えていく

        [Header("遷移するシーンの設定（複数設定可）")] public string[] ChengeScene;     // 遷移するシーンの設定（複数設定可)
        
        [Header("経過時間の設定")] public float TimeCount;               // 遷移条件：経過時間を入れる変数
    }

    [SerializeField] private SceneChangeManager[] Manager;      // 各シーンごとの遷移条件と遷移先を管理する構造体

    // 時間経過での遷移用変数
    private float TimeCounter;      　// 時間経過で遷移するシーンで経過時間を図るための変数
    private bool TimeFlag = false;    // 時間を計測しているかを判断するための変数

    // 勝利数での遷移用
    [HideInInspector] public bool WinnerFlag = false;   // 勝利者が決まったらTrueになり、シーン遷移処理を行わせるための変数

    // ボタン入力での遷移用
    [HideInInspector] public bool SceneLoadReady;       // ボタン入力で遷移してよいかの確認用フラグ

    // 遷移先が複数あるシーンの場合の遷移先設定用変数
    private int SelectSceneNum;

    // Player.csより、キーコンフィグ
    [HideInInspector] string[] GamepadButton = new string[4] { "A", "B", "X", "Y"};

    // ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    // メソッド
    // ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    // シーンの遷移条件に応じた処理をまとめたメソッド
    private void LoadManager(SceneLoadConditions condition)
    {
        switch (condition)
        {
            // 遷移条件が勝利数の時の処理
            case SceneLoadConditions.WinCount:

                // 勝利者確定フラグ
                if(WinnerFlag)
                {
                    WinnerFlag = false;
                    LoadScene(Manager[SceneIndex()].ChengeScene[0]);
                }
                break;

            // 遷移条件がボタンの入力の時の処理
            case SceneLoadConditions.Button:

                // 遷移可能の場合
                if(SceneLoadReady)
                {
                    // 現在のシーンがタイトルの場合、ABXYの全てのボタンで次のシーンへ遷移する
                    if(SceneManager.GetActiveScene().name == "Title")
                    {
                        if(PressedAllButtonCheck())
                        {
                            LoadScene(Manager[SceneIndex()].ChengeScene[0]);
                        }                        
                    }

                    // 対応するボタン（現在は１PのAボタン）を入力するとシーン遷移処理を行う
                    else if (AnyControllerPressed(GamepadButton[0]))
                    {
                        LoadScene(Manager[SceneIndex()].ChengeScene[0]);
                    }
                }                

                // シーン遷移可の場合(対応するボタンが押されてもまだ遷移したくない場合、この条件を使う)
                //if(SceneLoadReady)

                break;

            // 遷移条件が時間経過の時
            case SceneLoadConditions.TimeCount:
                // シーン開始時から計測開始
                if (TimeFlag != true)
                {
                    TimeCounter = Time.time;
                    TimeFlag = true;
                }
                // 設定した時間が経過したら、シーンの遷移処理を行う（計測フラグと計測時間はリセットする）
                if (TimeFlag == true && Manager[SceneIndex()].TimeCount <= (Time.time - TimeCounter))
                {
                    LoadScene(Manager[SceneIndex()].ChengeScene[0]);
                    TimeFlag = false;
                    TimeCounter = 0.0f;
                }

                break;
        }
    }


    // シーン遷移処理（引数で受け取った名前のシーンに遷移する）
    private void LoadScene(string SceneName)        // SceneNameに遷移先を渡す際、状況に応じた遷移先を読み込むために、対応したシーンの名前が入っている配列番号まで指定する
    {
        Debug.Log("LoadScene：" + SceneName);
        SceneManager.LoadScene(SceneName);
    }

    // SceneIndexを返す（現在のシーン判別用）
    private int SceneIndex()
    {
        return SceneManager.GetActiveScene().buildIndex;
    }


    // シーン番号に応じた遷移処理
    // シーン遷移処理（現在から次のIndexのシーンに遷移する）
    public void LoadNextScene()
    {
        // 現在のシーンのインデックスを取得
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        // 次のシーンのインデックスを計算
        int nextSceneIndex = (currentSceneIndex + 1) % SceneManager.sceneCountInBuildSettings;

        // 次のシーンに遷移
        SceneManager.LoadScene(nextSceneIndex);
    }

    // シーン遷移処理（現在から前のIndexのシーンに遷移する）
    public void LoadPrevScene()
    {
        // 現在のシーンのインデックスを取得
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        // 次のシーンのインデックスを計算
        int nextSceneIndex = (currentSceneIndex - 1) % SceneManager.sceneCountInBuildSettings;

        // 次のシーンに遷移
        SceneManager.LoadScene(nextSceneIndex);
    }


    // ボタン入力で遷移するシーンで、シーンごとに遷移可能フラグを設定する処理
    private void SetSceneLoadReady(int index)
    {
        // シーンに応じて遷移可能フラグの設定、または条件を付ける
        switch(index)
        {
            // タイトル画面
            case 0:

                SceneLoadReady = true;
                break;

            // ルール設定画面
            // ルール設定画面の遷移処理は、PlayerSettingスクリプトにまとめられているため、このスクリプトでは行わない
            case 1:
                // ルール設定画面に来た時、Static変数：勝利者の撃破数 の値を初期化する
                setSingleton.SetWinnerKillCount(0);
                SceneLoadReady = false;
                break;

            // 最終リザルト画面
            case 3:

                SceneLoadReady = true;
                break;

            // ボタン入力で遷移するシーン以外の場合は、フラグを下げておく
            default:

                SceneLoadReady = false;
                break;
        }
        
        // シーン一覧（ボタン入力で遷移するシーンは ● ）
        // Title       0　●
        // RuleSeting  1　●
        // Ingame      2
        // MidResult   3
        // FinalResult 4　●

    }




    // Player.csより、ボタン入力処理(入力はPlayer1のものに限定)
    public bool Pressd(string _str) { return Input.GetButtonDown("Player1" + _str); }

    // どのコントローラーでも入力を反映できるようにする関数
    public bool AnyControllerPressed(string _str)
    {
        // 案１(for文とif文を用いる方法)
        /*
        bool flag = false;
        for(int i = 1; i <= 4; i++)
        {
            if (Input.GetButtonDown("Player" + i + _str))
            {
                flag = true;
                break;
            }
        }
        return flag;
        */

        // 案２（if文のみを用いる方法）        
        if (Input.GetButtonDown("Player1" + _str)) return true;
        if (Input.GetButtonDown("Player2" + _str)) return true;
        if (Input.GetButtonDown("Player3" + _str)) return true;
        if (Input.GetButtonDown("Player4" + _str)) return true;
        return false;
    }
    
    // ABXYのいずれかが押された時Trueを返す関数
    private bool PressedAllButtonCheck()
    {
        bool pressed = false;
        
        for(int i = 0; i < 4 && pressed == false; i++)
        {
            if (Input.GetButtonDown("Player1" + GamepadButton[i])) pressed = true;
            if (Input.GetButtonDown("Player2" + GamepadButton[i])) pressed = true;
            if (Input.GetButtonDown("Player3" + GamepadButton[i])) pressed = true;
            if (Input.GetButtonDown("Player4" + GamepadButton[i])) pressed = true;
        }        
        return pressed;
    }


    // デバッグ用処理まとめ
    private void DebugProcessing()
    {
        // F1を押すとどのシーンからでもタイトルシーンに遷移する
        if(Input.GetKeyDown(KeyCode.F1))
        {
            LoadScene("Title");
        }


        // デバッグ用(右シフトを押しながら対応するキーを入力)
        if (Input.GetKey(KeyCode.RightShift))
        {
            // デバッグ用：シーンのビルドインデックスの出力（Pキー）
            if (Input.GetKeyDown(KeyCode.P))
            {
                Debug.Log("BuildIndex：" + SceneManager.GetActiveScene().buildIndex);
            }

            // デバッグ用：時間経過で遷移するシーンの時に、シーン開始からの経過時間を出力（Iキー）
            if (Input.GetKeyDown(KeyCode.I) && TimeFlag == true)
            {
                Debug.Log("TimeCount：" + (Time.time - TimeCounter));
            }
            // デバッグ用：シーンの切り替え
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                LoadNextScene();
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                LoadPrevScene();
            }

            // デバッグ用：シーン遷移可能フラグの操作
            if (Input.GetKeyDown(KeyCode.Y) && SceneLoadReady == false)
            {
                SceneLoadReady = true;
                Debug.Log("SceneLoadReady：" + SceneLoadReady);
            }
            if (Input.GetKeyDown(KeyCode.N) && SceneLoadReady == true)
            {
                SceneLoadReady = false;
                Debug.Log("SceneLoadReady：" + SceneLoadReady);
            }
            // デバッグ用：シーン遷移フラグの出力
            if (Input.GetKeyDown(KeyCode.F))
            {
                Debug.Log("SceneLoadReady：" + SceneLoadReady);
            }

            // ボタン入力遷移のシーンの時
            if (Manager[SceneIndex()].LoadCondition == SceneLoadConditions.Button)
            {
                if (SceneLoadReady)
                {
                    // デバッグ用：キーボードの入力でボタン入力のシーン遷移を行う場合はBキー
                    if (Input.GetKeyDown(KeyCode.B))
                    {
                        LoadScene(Manager[SceneIndex()].ChengeScene[0]);
                    }
                }
            }

            // 格納した勝利プレイヤーの要素番号を出力する
            if(Input.GetKeyDown(KeyCode.Q))
            {
                Debug.Log("WinnerPlayerNumber：" + setSingleton.GetWinnerNum());
            }
        }
    }

    // -------------------------------------------------------------------------------------------------------------------

    private void Start()
    {
        setSingleton = GetComponent<SetSingleton>();
    }


    void Update()
    {
        // シーンに応じた遷移処理
        LoadManager(Manager[SceneIndex()].LoadCondition);

        // ボタン入力で遷移するシーン時の遷移フラグの確認と設定
        SetSceneLoadReady(SceneIndex());

        // デバッグ処理
        DebugProcessing();        
    }
}

// メモ
// 2024/01/19
// 更新：勝利数によって行われるシーン遷移処理の実装
// 
// 2024/01/22
// 更新：ボタンでのシーン遷移を行う処理を、仮置きのspaceキーから、Player1のBボタン入力で実行に変更
// 
// 2024/01/26
// 更新：ボタンでのシーン遷移を行う際、シーンごとに遷移可能な条件を設定できるように処理を追加（SetSceneLoadReady）
// 　　　ルール設定画面の場合、先に進むボタンが選択されているとき（オブジェクトが表示されている間）Bボタン入力で遷移するようになっている
// 修正：コードのコメント、Headerでのインスペクター上での表示内容を一部変更
// 　　　デバッグ用処理を１つの関数にまとめるように変更
//  


//// 現在のシーンに応じて遷移条件を設定するメソッド
//private SceneLoadConditions SetCondition(string ActiveSceneName)
//{
//    switch (ActiveSceneName)
//    {
//        case "Title":

//            break;

//        case "RuleSetting":

//            break;

//        case "InGame":

//            break;

//        case "MidResult":

//            break;

//        case "FinalResult":

//            break;
//    }


//    return 0;
//}
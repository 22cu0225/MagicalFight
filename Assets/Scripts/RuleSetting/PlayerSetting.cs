using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


//---------------------------------------------------
//処理内容メモ：
//  プレイヤーの人数の設定
//  勝ち数上限の設定
//  設定項目を配列で管理
//  現在いる項目の色を変える
//  左右のボタンを押したときに対応する三角形を一瞬色変える
//---------------------------------------------------


public class PlayerSetting : MonoBehaviour
{
    //シーン遷移
    SceneController SceneControllerScript;


    //数値制限      要素番号（０：プレイヤー人数制限、１：勝ち点上限）
    //-----------------------------------------------------------------
    [SerializeField] int[] upperLmits;          //上限
    [SerializeField] int[] underLmits;          //下限   
    //-----------------------------------------------------------------


    //設定項目の背景
    [SerializeField] GameObject[] items;



    //設定項目の管理番号
    enum ItemNumbers {
        e_playerCount,
        e_maxWinPointsCount,
        e_startButton,
        e_ItemCount
    };

    //現在の選択項目の管理番号を格納
    int curentItem;

    public static int[] playerSettings;       //0 : プレイヤーの人数制限設定, 1 : 勝ち点の制限設定

    //入力関係
    //-----------------------------------------------------------------
    [SerializeField] float deadZone;
    [SerializeField] float limit;
    float timer;
    bool canInput = false;
    //-----------------------------------------------------------------

    void Start()
    {
        SceneControllerScript = this.gameObject.GetComponent<SceneController>();

        playerSettings = new int[2] { 
            underLmits[(int)ItemNumbers.e_playerCount],
            underLmits[(int)ItemNumbers.e_maxWinPointsCount]
        };

        curentItem = (int)ItemNumbers.e_playerCount;

        HighlightingCurentItem();
    }

    void Update()
    {

        float dx = Input.GetAxis("Horizontal");
        float dy = Input.GetAxis("Vertical");

        if (!canInput)
        {
            timer += Time.deltaTime;
            if (timer >= limit) canInput = true;
        }
        else
        {
            //上
            if (Input.GetKeyDown(KeyCode.UpArrow) || dy > deadZone)
            {
                //一項目上に
                if (curentItem > 0)
                {
                    curentItem--;
                }
                HighlightingCurentItem();
                canInput = false;
                timer = 0.0f;
            }
            //下
            else if (Input.GetKeyDown(KeyCode.DownArrow) || dy < -deadZone)
            {
                //一項目下に
                if (curentItem < (int)ItemNumbers.e_ItemCount - 1)
                {
                    curentItem++;
                }
                HighlightingCurentItem();
                canInput = false;
                timer = 0.0f;
            }
           
            if (curentItem < (int)ItemNumbers.e_startButton)
            {
                //左
                if (Input.GetKeyDown(KeyCode.LeftArrow) || dx < -deadZone)
                {
                    //カウントを１減らす
                    if (playerSettings[curentItem] > underLmits[curentItem])
                    {
                        playerSettings[curentItem]--;
                    }

                    canInput = false;
                    timer = 0.0f;
                }
                //右
                else if (Input.GetKeyDown(KeyCode.RightArrow) || dx > deadZone)
                {
                    //カウントを1増やす
                    if (playerSettings[curentItem] < upperLmits[curentItem])
                    {
                        playerSettings[curentItem]++;
                    }
                    canInput = false;
                    timer = 0.0f;
                }
                //三角形の色変える
                if (Input.GetKey(KeyCode.RightArrow) || dx > deadZone)
                {
                    VerificationGameobject(items[curentItem], Color.red, "Right");
                }
                else
                {
                    VerificationGameobject(items[curentItem], Color.white, "Right");
                }
                if (Input.GetKey(KeyCode.LeftArrow) || dx < -deadZone)
                {
                    VerificationGameobject(items[curentItem], Color.red, "Left");
                }
                else
                {
                    VerificationGameobject(items[curentItem], Color.white, "Left");
                }

            }
        }

        //決定キー
        // どのコントローラーでもボタン入力を拾えるように変更（岩本）
        if (Input.GetKeyDown(KeyCode.M) || AnyControllerPressed("A"))
        {
            //ボタンを押下
            if (curentItem == (int)ItemNumbers.e_startButton)
            {
                SceneControllerScript.LoadNextScene();
            }
            //一項目下に
            if (curentItem < (int)ItemNumbers.e_ItemCount - 1)
            {
                curentItem++;
            }
            HighlightingCurentItem();
        }
    }

    // 選択項目の背景の表示非表示を更新する処理
    void HighlightingCurentItem()
    {
        for (int i = 0; i < (int)ItemNumbers.e_ItemCount; ++i)
        {
            if (i == curentItem)
            {
                //現在の項目に対して行う処理

                ChangeColorOfGameObject(items[i], Color.white);
            }
            else
            {
                //現在の項目以外に対して行う処理
                ChangeColorOfGameObject(items[i], Color.gray);
            }
        }
    }

    public int GetCounter(int _index) { return playerSettings[_index]; }


    private void ChangeColorOfGameObject(GameObject _targetObject, Color _color)
    {
        //入力されたオブジェクトのRendererを全て取得し、さらにそのRendererに設定されている全Materialの色を変える
        foreach (Renderer targetRenderer in _targetObject.GetComponents<Renderer>())
        {
            foreach (Material material in targetRenderer.materials)
            {
                material.color = _color;
            }
        }

        //入力されたオブジェクトのTextを全て取得し、色を変える
        foreach (Text text in _targetObject.GetComponents<Text>())
        {
            text.color = _color;
        }

        //入力されたオブジェクトの子にも同様の処理を行う
        for (int i = 0; i < _targetObject.transform.childCount; i++)
        {
            ChangeColorOfGameObject(_targetObject.transform.GetChild(i).gameObject, _color);
        }
    }


    void VerificationGameobject(GameObject _targetObject, Color _color, string _obectName)
    {
        if (_targetObject.name == _obectName)
        {
            ChangeColorOfGameObject(_targetObject, _color);
        }

        //入力されたオブジェクトの子にも同様の処理を行う
        for (int i = 0; i < _targetObject.transform.childCount; i++)
        {
            VerificationGameobject(_targetObject.transform.GetChild(i).gameObject, _color, _obectName);
        }
    }

    // SceneLoadManagerからどのコントローラーの入力も拾える関数を持ってきた（岩本）
    public bool AnyControllerPressed(string _str)
    {        
        if (Input.GetButtonDown("Player1" + _str)) return true;
        if (Input.GetButtonDown("Player2" + _str)) return true;
        if (Input.GetButtonDown("Player3" + _str)) return true;
        if (Input.GetButtonDown("Player4" + _str)) return true;
        return false;
    }


}

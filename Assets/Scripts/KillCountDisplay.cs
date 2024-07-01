using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillCountDisplay : MonoBehaviour
{
    // 変数宣言
    UnityEngine.UI.Text text;       // テキストの表示内容を変えるためにTextコンポーネントを取得する用

    private int KillCount;          // 勝利者の撃破数を格納する用

    // Start is called before the first frame update
    void Start()
    {
        // コンポーネントの取得
        text = GetComponent<UnityEngine.UI.Text>();

        // 勝利者の撃破数を取得
        KillCount = GameObject.Find("SceneManager").GetComponent<SetSingleton>().GetWinnerKillCount();
    }

    // Update is called once per frame
    void Update()
    {
        // 取得した勝利数をstring型として撃破数をTextに反映する
        text.text = "" + KillCount;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Counter : MonoBehaviour
{
    [SerializeField]GameObject obj;
    PlayerSetting PlayerSettingScript;

    [SerializeField] int num;

    //public GameObject counter_object = null;
    void Start()
    {
        PlayerSettingScript = obj.GetComponent<PlayerSetting>();
    }

    void Update()
    {
        // オブジェクトからTextコンポーネントを取得
        Text counter_text = this.GetComponent<Text>();
        // テキストの表示を入れ替える
        counter_text.text = PlayerSettingScript.GetCounter(num).ToString();
    }
}

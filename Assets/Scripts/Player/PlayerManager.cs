using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private float damageValue;
    //PlayerDB pDB;
    PlayerGenerate PlayerGenerateScript;
    GameManager GameManagerScript;
    //プレイヤーの現在の人数を格納
    int currentPlayerCount;

    //[SerializeField] private Slider hpBar;

    [SerializeField] private GameObject mainCamera;     // メインカメラオブジェクト
    private CameraShake cameraShake;                    // カメラを揺らすスクリプト
    [SerializeField] private float duration = 0.3f;     // 期間
    [SerializeField] private float magnitude = 0.6f;    // 揺れの大きさ
    [SerializeField] private int OverPower;             // 揺れる様になる大きさ

    void Start()
    {
        PlayerGenerateScript = this.gameObject.GetComponent<PlayerGenerate>();
        GameManagerScript = this.gameObject.GetComponent<GameManager>();
        //プレイヤー人数の初期化
        currentPlayerCount = GameManagerScript.playerCount;

        cameraShake = mainCamera.GetComponent<CameraShake>();
    }

    void Update()
    {
        //debug
        if (Input.GetKeyDown(KeyCode.O)) Hit(PlayerGenerateScript.GetPlayerList()[0], 5);
        //debug
        if (Input.GetKeyDown(KeyCode.P)) Hit(PlayerGenerateScript.GetPlayerList()[1], 5);
    }

    public void SetHpBar(GameObject _hitObj, float _hp)
    {
        _hitObj.GetComponent<PlayerDB>().MyHpBar.GetComponent<Slider>().value = _hp / _hitObj.GetComponent<PlayerDB>().MyMaxHp;
    }

    //ヒット時の処理
    public void Hit(GameObject _hitObj, int _power)
    {
        float hp = _hitObj.GetComponent<PlayerDB>().MyHp;
        if (hp > 0)
        {
            hp -= _power;
            if (hp <= 0) hp = 0;
            _hitObj.GetComponent<PlayerDB>().SetHp(hp);
            SetHpBar(_hitObj, hp);
            Debug.Log(_hitObj.GetComponent<PlayerDB>().MyIsAlive);

        }
        // 死亡アニメーション処理のために、コメントアウトします
        /*
        if (hp <= 0 && _hitObj.GetComponent<PlayerDB>().MyIsAlive != false)
        {
            Debug.Log("デストロイ！ を呼び出し");
            PlayerDestroy(_hitObj);
        }
        */

        // パワーが指定した値以上だったらカメラを揺らす
        if (_power >= OverPower)
        {
            StartCoroutine(cameraShake.Shake(duration, magnitude));
        }
    }

    //プレイヤーが倒された時
    public void PlayerDestroy(GameObject _destroyObj)
    {
        //プレイヤーを死亡状態にする
        _destroyObj.GetComponent<PlayerDB>().SetIsAlive(false);
        //プレイヤーを非表示
        _destroyObj.SetActive(false);
        //プレイヤーに対応するHPバーを非表示にする
        _destroyObj.GetComponent<PlayerDB>().MyHpBar.SetActive(false);
        //現在のプレイヤーの人数を減らす
        currentPlayerCount--;
        Debug.Log("デストロイ！");
        //if (currentPlayerCount == 1) GameManagerScript.MidResult();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeEffectInstantiate : MonoBehaviour
{
    // 変数宣言
    // 各スクリプトやオブジェクトを取得する変数
    [SerializeField] private GameObject ChargeEffect;   // タメエフェクトオブジェクトを入れる変数
    private GameObject PlayingEffect;                   // インスタンス化したエフェクトオブジェクトを入れる変数
    [SerializeField] private GameObject SecondChargeEffect;   // 二回目のタメエフェクトオブジェクトを入れる変数
    private GameObject SecondPlayingEffect;                   // 二回目のインスタンス化したエフェクトオブジェクトを入れる変数

    private GameObject NormalShot;                      // 魔法オブジェクトを入れる変数

    [HideInInspector] public Normal_Attack normal_Attack;   // タメができる魔法のスクリプトを入れる変数

    private int Charge_step;                            // タメ段階を入れる変数

    private void Start()
    {
        // オブジェクトやスクリプトの取得
        normal_Attack = GetComponent<Normal_Attack>();
        NormalShot = gameObject;

        // タメ段階をNormal_Attackから取得する
        Charge_step = normal_Attack.GetCharge_step();
    }

    private void Update()
    {
        // チャージ段階が上がった時、チャージ段階変更エフェクトを生成する
        if (Charge_step != normal_Attack.GetCharge_step())
		{
            // 既にエフェクトが生成されている場合は１度破棄する
            if (SecondPlayingEffect != null)
            {
                Destroy(SecondPlayingEffect);
            }
            // エフェクト生成
            SecondPlayingEffect = Instantiate(SecondChargeEffect, NormalShot.transform);

            // チャージ段階更新
            Charge_step = normal_Attack.GetCharge_step();
        }
        
        // 魔法オブジェクトが生成されていたら、チャージエフェクトオブジェクトを生成する(重複して生成されないように、生成した後は行わないようにする)
        if (NormalShot != null && PlayingEffect == null)
        {
            InstanceEffect();
        }

        // 魔法を発射後の処理
        if (normal_Attack.Getis_Move())
        {
            ProcShotAfterEffect();
        }
    }

    private void InstanceEffect()
    {
        /*
        // 魔法のタメ段階を入れる（次のタメ段階更新の判定のため）
        Charge_step = normal_Attack.GetCharge_step();

        // 既にエフェクトが生成されている場合は１度破棄する
        if (PlayingEffect != null)
        {
            Destroy(PlayingEffect);
        }
        */

        // プレイヤーに応じたタメエフェクトオブジェクトを生成する
        PlayingEffect = Instantiate(ChargeEffect, NormalShot.transform);
    }

    private void ProcShotAfterEffect()
    {
        // 魔法発射後はエフェクトを消す
        if (PlayingEffect != null)
        {
            Destroy(PlayingEffect);
        }
        if (SecondPlayingEffect != null)
        {
            Destroy(SecondPlayingEffect);
        }
        // 発射後もエフェクトを残す場合は、以下の処理でエフェクトが魔法オブジェクトに追従するようになる
        /*
        if (PlayingEffect.transform.position != transform.position)
        {
            PlayingEffect.transform.position = transform.position;
        }
        */
    }
}

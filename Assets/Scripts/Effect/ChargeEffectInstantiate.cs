using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeEffectInstantiate : MonoBehaviour
{
    // �ϐ��錾
    // �e�X�N���v�g��I�u�W�F�N�g���擾����ϐ�
    [SerializeField] private GameObject ChargeEffect;   // �^���G�t�F�N�g�I�u�W�F�N�g������ϐ�
    private GameObject PlayingEffect;                   // �C���X�^���X�������G�t�F�N�g�I�u�W�F�N�g������ϐ�
    [SerializeField] private GameObject SecondChargeEffect;   // ���ڂ̃^���G�t�F�N�g�I�u�W�F�N�g������ϐ�
    private GameObject SecondPlayingEffect;                   // ���ڂ̃C���X�^���X�������G�t�F�N�g�I�u�W�F�N�g������ϐ�

    private GameObject NormalShot;                      // ���@�I�u�W�F�N�g������ϐ�

    [HideInInspector] public Normal_Attack normal_Attack;   // �^�����ł��閂�@�̃X�N���v�g������ϐ�

    private int Charge_step;                            // �^���i�K������ϐ�

    private void Start()
    {
        // �I�u�W�F�N�g��X�N���v�g�̎擾
        normal_Attack = GetComponent<Normal_Attack>();
        NormalShot = gameObject;

        // �^���i�K��Normal_Attack����擾����
        Charge_step = normal_Attack.GetCharge_step();
    }

    private void Update()
    {
        // �`���[�W�i�K���オ�������A�`���[�W�i�K�ύX�G�t�F�N�g�𐶐�����
        if (Charge_step != normal_Attack.GetCharge_step())
		{
            // ���ɃG�t�F�N�g����������Ă���ꍇ�͂P�x�j������
            if (SecondPlayingEffect != null)
            {
                Destroy(SecondPlayingEffect);
            }
            // �G�t�F�N�g����
            SecondPlayingEffect = Instantiate(SecondChargeEffect, NormalShot.transform);

            // �`���[�W�i�K�X�V
            Charge_step = normal_Attack.GetCharge_step();
        }
        
        // ���@�I�u�W�F�N�g����������Ă�����A�`���[�W�G�t�F�N�g�I�u�W�F�N�g�𐶐�����(�d�����Đ�������Ȃ��悤�ɁA����������͍s��Ȃ��悤�ɂ���)
        if (NormalShot != null && PlayingEffect == null)
        {
            InstanceEffect();
        }

        // ���@�𔭎ˌ�̏���
        if (normal_Attack.Getis_Move())
        {
            ProcShotAfterEffect();
        }
    }

    private void InstanceEffect()
    {
        /*
        // ���@�̃^���i�K������i���̃^���i�K�X�V�̔���̂��߁j
        Charge_step = normal_Attack.GetCharge_step();

        // ���ɃG�t�F�N�g����������Ă���ꍇ�͂P�x�j������
        if (PlayingEffect != null)
        {
            Destroy(PlayingEffect);
        }
        */

        // �v���C���[�ɉ������^���G�t�F�N�g�I�u�W�F�N�g�𐶐�����
        PlayingEffect = Instantiate(ChargeEffect, NormalShot.transform);
    }

    private void ProcShotAfterEffect()
    {
        // ���@���ˌ�̓G�t�F�N�g������
        if (PlayingEffect != null)
        {
            Destroy(PlayingEffect);
        }
        if (SecondPlayingEffect != null)
        {
            Destroy(SecondPlayingEffect);
        }
        // ���ˌ���G�t�F�N�g���c���ꍇ�́A�ȉ��̏����ŃG�t�F�N�g�����@�I�u�W�F�N�g�ɒǏ]����悤�ɂȂ�
        /*
        if (PlayingEffect.transform.position != transform.position)
        {
            PlayingEffect.transform.position = transform.position;
        }
        */
    }
}

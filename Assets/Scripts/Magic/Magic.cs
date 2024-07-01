using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magic : MonoBehaviour
{
    //�v���C���[��prefab
    [HideInInspector]public GameObject Player;

    // �ϐ��錾
    [SerializeField] Vector2 offsetMagicPosition;   // ���p�̐����ʒu
    [SerializeField] int power;                     // �З�
    [SerializeField] Vector2 speed;                 // ���x
    [SerializeField] float destroySecond = 10.0f;   // ���Ŏ���
    [SerializeField] float coolTime = 1.0f;         // �N�[���^�C��
    [SerializeField] string magicTag = "Magic";     // ���p�̃^�O��
    [SerializeField] bool FloorDestroy = true;      // ���A�ǂɓ����蔻������邩
    [SerializeField] bool pCol_Destroy = true;      // �v���C���[�Ɠ��������u�Ԃɏ�����
    private bool orientation = false;               // ���p�̌���
    private int pNum;                               // �����v���C���[�̔ԍ�

    // ���E�G�t�F�N�g
    [SerializeField] GameObject Counterbalancing;

    private void Update()
    {
        destroySecond -= Time.deltaTime;
        if (destroySecond <= 0.0f)
        {
            Destroy(gameObject);
        }
    }

    // Set Function
    public void SetPower(int _power) { power = _power; }
    public void SetOrientation(bool _orientation) { orientation = _orientation; }
    public void setPlayerObj(GameObject obj) { Player = obj; }
    public void setPNum(int P_NUM) { pNum = P_NUM; }
    public void SetF_Destroy(bool fd) { FloorDestroy = fd; }

    // Get Function
    public Vector2 GetMagicPosition() { return offsetMagicPosition; }
    public int GetPower() { return power; }
    public Vector2 GetSpeed() { return speed; }
    public float GetCoolTime() { return coolTime; }
    public int GetPNum() { return pNum; }
    public bool GetOrientation() { return orientation; }

    // ���j�v���C���[���ʗp�i��{�j
    public string GetPName() { return Player.name; }

    //Trigger
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(magicTag))
        {
            Debug.Log(pNum != collision.GetComponent<Magic>().GetPNum());
            //�����v���C���[�̔ԍ��ƈقȂ�ꍇ�̂ݏ���
            if(pNum!= collision.GetComponent<Magic>().pNum)
            {
                int ThisPower = this.GetPower();
                int OtherPower = collision.GetComponent<Magic>().GetPower();
                if (ThisPower - OtherPower <= 0)
                {
                    if(Counterbalancing != null)
                    {
                        GameObject efect = Instantiate(Counterbalancing);
                        efect.transform.position = this.transform.position;
                    }
                    Destroy(gameObject);
                }
                else
                {
                    collision.GetComponent<Magic>().SetPower(ThisPower - OtherPower);
                }
            }
        }
        //���ǂɐڐG������f�X�g���C
        if(collision.gameObject.tag == "Floor" || collision.gameObject.tag == "Wall")
		{
            if(FloorDestroy)
            {
                if (Counterbalancing != null)
                {
                    GameObject efect = Instantiate(Counterbalancing);
                    efect.transform.position = this.transform.position;
                }
                Destroy(this.gameObject);
            }
        }
        //�v���C���[�ɐG�ꂽ��f�X�g���C
        if(collision.gameObject.tag=="Player")
        {
            if (pCol_Destroy)
            {
                if (Counterbalancing != null)
                {
                    GameObject efect = Instantiate(Counterbalancing);
                    efect.transform.position = this.transform.position;
                }
                Destroy(this.gameObject);
            }
        }
    }
	private void OnTriggerStay2D(Collider2D collision)
	{
        //���ǂɐڐG������f�X�g���C
        if (collision.gameObject.tag == "Floor" || collision.gameObject.tag == "Wall")
        {
            if (FloorDestroy)
            {
                if (Counterbalancing != null)
                {
                    GameObject efect = Instantiate(Counterbalancing);
                    efect.transform.position = this.transform.position;
                }
                Destroy(this.gameObject);
            }
        }
    }
}

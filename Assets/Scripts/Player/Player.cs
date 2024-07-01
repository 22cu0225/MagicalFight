using UnityEngine;
//using static UnityEditor.PlayerSettings;

public class Player : MonoBehaviour
{
    [SerializeField] private int playerNumber = 1;          // �v���C���[�̔ԍ�
    
    //Move
    //-------------------------------------------------
    [SerializeField] float moveSpeed = 3;                   // �ړ����x
    [SerializeField] float MmgnificationRun = 1.5f;         // ����ۂ̔{��
    private float moveSpeedResult;                          // �ړ����x�v�Z����
    private bool orientation;                               // ����  true:���@false:�E
    //private bool canMove;                                 // �����邩
    //-------------------------------------------------

    Scaffold_p scaffold;

    //Jump
    //-----------------------------------
    private Rigidbody2D playerRb;               // Rigidbody
    [SerializeField] float jumpPower = 5;       // �W�����v��
    [SerializeField] float fallPower = 2f;      // �������x
    [SerializeField] int jumpLimit = 2;         // �W�����v������
    private int jumpCount;                      // �W�����v��
    private float jumpCoolTime;                 // ���̃W�����v�܂ł̃N�[���^�C��
    private bool coolTimeStart;                 // �W�����v�N�[���^�C�����n�܂邩
    private bool fallStart;                     // �����J�n������
    private bool fly;                           // ���ł��邩
    private bool addFall;                       // ��������
    //-----------------------------------

    //Size
    //-----------------------------------
    private CapsuleCollider2D playerCol;        // �v���C���[�̃R���W����
    private float MaxYSize;                     // �v���C���[�̍ő�Y�T�C�Y
    //-----------------------------------

    //--------------------------------------------
    [Header("�L�[�R���t�B�O")]
    [SerializeField] string[] GamepadButton = new string[4] {"A","B","X","Y"};
    //--------------------------------------------

    //--------------------------------------------
    [Header("TagName")]
    [SerializeField] string Floor = "Floor";
    [SerializeField] string Scaffold = "Scaffold";
    //[SerializeField] string someone = "";
    //--------------------------------------------

    //--------------------------------------------
    GameObject manager;
    PlayerManager playerManagerSclipt;
    //--------------------------------------------

    // Animation
    Animator animator;

    // �m�b�N�o�b�N
    DamegeAction damegeAction;

    [Header("Magic")]
    [SerializeField] GameObject magicPrehubNomal;
    [SerializeField] GameObject magicPrehubTop;
    [SerializeField] GameObject magicPrehubDown;

    float[] coolTimeCounter = new float[3];
    bool[] coolDown = new bool[3];        // �N�[���^�C���ɓ�������

    //�����蔻���؂邽��
    private Collider2D m_Col;

    [Header("Effect")]
    [SerializeField] private GameObject jumpEffectPrefub;
    private GameObject jumpEffect;

    // �|���ꂽ�㑀��s�\�ɂ��邽�߂̃t���O�i��{�j
    // PlayerDB����MyInAlive�ōs��

    // Start is called before the first frame update
    void Start()
    {
        manager = GameObject.Find("Manager");

        playerManagerSclipt = manager.GetComponent<PlayerManager>();
        playerNumber = GetComponent<PlayerDB>().MyNumber + 1;
        playerRb = GetComponent<Rigidbody2D>();
        playerCol = GetComponent<CapsuleCollider2D>();
        scaffold = GetComponent<Scaffold_p>();
        animator = GetComponent<Animator>();
        damegeAction = GetComponent<DamegeAction>();

        moveSpeedResult = moveSpeed;
        MaxYSize = playerCol.size.y;
        jumpCoolTime = 0.0f;
        jumpCount = 0;

        addFall = false;
        fallStart = false;
        coolTimeStart = false;
        fly = false;
        coolDown[0] = coolDown[1] = coolDown[2] = false;
    }

    // Update is called once per frame
    void Update()
    {
        // �R���g���[���[�̊m�F
        //Controoller();

        // �ȉ��̏����̓v���C���[���������Ă���ԍs���i��{�j
        // ���S���Ɉړ��▂�@�̏������s��Ȃ��悤�ɂ���
        if(GetComponent<PlayerDB>().MyIsAlive)
        {
            // �ړ�����
            Move();
            // ���E�̌���
            CangeScale(this.gameObject);
            // �W�����v����
            Jump();
            // ���p����
            Magic(magicPrehubNomal, GamepadButton[1], 0);
            Magic(magicPrehubTop, GamepadButton[2], 1);
            Magic(magicPrehubDown, GamepadButton[3], 2);
        }        
    }
    /*
    void Controoller()
    {
        // �ڑ�����Ă���R���g���[���̖��O�𒲂ׂ�
        var controllerNames = Input.GetJoystickNames();

        // �����̔ԍ��̃R���g���[�����ڑ�����Ă��Ȃ���΃G���[
        if (controllerNames[playerNumber - 1] == "")
        {
            Debug.Log("Error" + playerNumber);
        }
    }
    */
    void Move()
    {
            // ���͂��ꂽ�ړ������̃x�N�g��
            Vector2 moveDirection = new Vector2(Input.GetAxis("Player" + playerNumber + "Horizontal"), -Input.GetAxis("Player" + playerNumber + "Vertical"));
        if (moveDirection.x > 0.2 || moveDirection.x < -0.2)
        {
            // ���E����
            orientation = moveDirection.x > 0 ? true : false;

            // �_�b�V��
            if(moveDirection.x > 0.9 || moveDirection.x < -0.9)
            {
                moveSpeedResult = moveSpeed * MmgnificationRun;
            }
            else
            {
                moveSpeedResult = moveSpeed;
            }

            // X�ړ�
            transform.Translate(moveDirection.x * moveSpeedResult * Time.deltaTime, 0.0f, 0.0f);
            // �A�j���[�V���������@�J�n
            animator.SetBool("walk", true);
        }
        else
        {
            // �A�j���[�V���������@��~
            animator.SetBool("walk", false);
        }
            // crouch
            if (moveDirection.y < -0.1)
            {
                // �W�����v��
                if(fly)
                {
                    //�@�������x�㏸
                    if (!addFall)
                    {
                        playerRb.velocity += Vector2.down * fallPower*5;
                    }
					addFall = true;
                }
                // �n��
                else
                {
                    // ��������
                    scaffold.SetThrough(true);

                    animator.SetFloat("speed", 2f);
                    // ���Ⴊ�݃A�j���[�V���� �J�n
                    animator.SetBool("crouch", true);
                    // �R���W������Y�𔼕���
                    playerCol.size = new Vector2(playerCol.size.x, MaxYSize/2);
                    //Debug.Log("���Ⴊ��ł��");
                }
            }
            else
            {
            // �ő�T�C�Y��菬����������
            if (playerCol.size.y < MaxYSize)
            {
                // �ő�T�C�Y��
                playerCol.size = new Vector2(playerCol.size.x, MaxYSize);
            }
            // ���Ⴊ�݃A�j���[�V���� ��~
            animator.SetBool("crouch", false);
            }
        //Debug.Log(moveDirection);
    }
    void Jump()
    {
        // �W�����v�������������N�[���^�C�����I����Ă�����
        if (jumpCount < jumpLimit && !coolTimeStart)
        {
            // PressdButton A
            if (Pressd(GamepadButton[0]))
            {
                // �G�t�F�N�g����������Ă���ꍇ�͂P�x�j������
                if (jumpEffect != null) Destroy(jumpEffect);
                // �W�����v�G�t�F�N�g����
                jumpEffect = Instantiate(jumpEffectPrefub, transform);
                // ������ɗ͂������W�����v
                playerRb.velocity = Vector2.up * jumpPower;
                // �J�E���g����
                jumpCount++;
                // �N�[���^�C��
                coolTimeStart = true;
                // �������x�㏸�X�C�b�`��؂�
                addFall = false;
                fallStart = false;
                // �W�����v���ړ����x����
                moveSpeedResult = moveSpeed * 0.5f;
                //Debug.Log("�����Ղ����");
            }
        }
        // ��x�ڂ̃W�����v�̃N�[���^�C��
        if (coolTimeStart)
        {
            jumpCoolTime -= Time.deltaTime;
            // �J�E���g�I����
            if (jumpCoolTime < 0.0f)
            {
                // �X�C�b�`��؂�
                coolTimeStart = false;
                jumpCoolTime = 0.3f;
            }
        }

        // �������x�㏸
        if (playerRb.velocity.y < 0.6f�@&& !fallStart)
        {
            playerRb.velocity = Vector2.down * fallPower;
            fallStart = true;
        }
    }
    void Magic(GameObject magicPrehub,string GameButton,int magicNumber)
    {
        // �N�[���^�C��
        if (coolDown[magicNumber])
        {
            coolTimeCounter[magicNumber] -= Time.deltaTime;
            if (coolTimeCounter[magicNumber] <= 0.0f)
            {
                coolDown[magicNumber] = false;
            }
        }
        // pressdButton& �N�[���^�C���������Ă���
        if (Pressd(GameButton) && !coolDown[magicNumber])
        {
            // �I�t�Z�b�g�ʒu�ݒ�
            Vector2 OffsetPos = transform.position;
            Vector2 magicPosition = magicPrehub.GetComponent<Magic>().GetMagicPosition();
            OffsetPos += magicPosition;
            //�����ɍ��킹�Đ����ʒu��ύX
            //������
            if (!orientation)
            {
                OffsetPos.x = OffsetPos.x - (magicPosition.x * 2);
            }
            // ����
            GameObject magic = Instantiate(magicPrehub, OffsetPos, transform.rotation);
            //�e�l��n��
            magic.GetComponent<Magic>().setPlayerObj(gameObject);
            magic.GetComponent<Magic>().SetOrientation(orientation);
            magic.GetComponent<Magic>().setPNum(playerNumber);
            //���g�̕��ɓ�����Ȃ��悤�ɂ��邽�ߓ����蔻���؂�
            m_Col = magic.GetComponent<Collider2D>();
            Physics2D.IgnoreCollision(playerCol, m_Col, true);
            // ���g�̌����ύX
            CangeScale(magic);
            // coolDown�J�n
            coolDown[magicNumber] = true;
            coolTimeCounter[magicNumber] = magicPrehubNomal.GetComponent<Magic>().GetCoolTime();
        }
    }
    void CangeScale(GameObject target)
    {
        // target�̃X�P�[�����擾
        Vector3 cangeScale = target.transform.localScale;
        // ��U�v���X�ɕ␳
        if(cangeScale.x < 0) { cangeScale.x *= -1; }
        // �X�P�[���ύX
        cangeScale.x = !orientation ? cangeScale.x : cangeScale.x * -1;
        target.transform.localScale = cangeScale;
    }

    public bool Pressd(string _str) { return Input.GetButtonDown("Player" + playerNumber + _str); }

    public bool Release(string _str) { return Input.GetButtonUp("Player" + playerNumber + _str); }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag(Floor) || collision.gameObject.CompareTag(Scaffold))
        {
            jumpCount = 0;
            moveSpeedResult = moveSpeed;
            if (jumpEffect != null) Destroy(jumpEffect);
        }
        //Debug.Log(collision.gameObject.tag);
    }

	private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(Floor) || collision.gameObject.CompareTag(Scaffold))
        {
            fly = false;
        }
    }

	private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Magic"))
        {
            //Debug.Log("���p�Ɠ��������I");
            playerManagerSclipt.GetComponent<PlayerManager>().Hit(this.gameObject, collision.gameObject.GetComponent<Magic>().GetPower());
            // �m�b�N�o�b�N
            int moveOrientation = collision.gameObject.GetComponent<Magic>().GetOrientation() ? 1 : -1; // �E������
            StartCoroutine(damegeAction.KnockBack(this.gameObject, moveOrientation* collision.gameObject.GetComponent<Magic>().GetPower()/2, 0.1f));
            //Magic.cs�Ɉ�U�ڂ�
            //Destroy(collision);

            // ���p�Ɠ�����HP���Ȃ��Ȃ������A�����̖��O�ƒN�ɂ��ꂽ����ʃX�N���v�g�Ɋi�[
            if(GetComponent<PlayerDB>().MyHp <= 0�@&& GetComponent<PlayerDB>().MyIsAlive)
            {
                GameObject.Find("Manager").GetComponent<PlayerDefeaterManager>().VictimRecord = gameObject.name;
                GameObject.Find("Manager").GetComponent<PlayerDefeaterManager>().DefeaterRecord = collision.GetComponent<Magic>().GetPName();
            }

        }
        if(collision.gameObject.CompareTag("Abyss"))
        {
            //Debug.Log("�ޗ�");
            playerManagerSclipt.GetComponent<PlayerManager>().PlayerDestroy(gameObject);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        fly = true;
    }

    // Set Function
    // At the time of instantiate player
    public void SetPlayerNumber(int _playerNumber) { playerNumber = _playerNumber+1; }

    //Get Function
    public bool GetOrientation() { return orientation; }
    public string GetButton(int i) { return GamepadButton[i]; }
    public int GetPNum() { return playerNumber; }

    public bool isJumping() { return fly; }
}

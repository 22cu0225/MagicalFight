using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    private Camera mainCamera;

    [SerializeField] private float minSize = 3;     // �J�����ŏ��T�C�Y
    //[SerializeField] private float maxUnder = -8;   // �J�������Ǐ]������E�l

    [SerializeField] GameObject manager;            // �}�l�[�W���[
    private PlayerGenerate playerGenerateScript;    // �v���C���[�����X�N���v�g
    private List<GameObject> thisPlayerList;        // �v���C���[���X�g

    [SerializeField] float smoothTime = 0.3f;       // �J�������v���C���[��ǐՂ���ۂ̃X���[�Y���̒����p�p�����[�^
    private Vector3 velocity = Vector3.zero;        // �J�����ړ����̑��x�x�N�g��       
    
    [SerializeField] Vector2 ZoomOffset = new Vector2(2, 2);
    private float screenAspect = 0;

    // Start is called before the first frame update
    void Start()
    {
        playerGenerateScript = manager.GetComponent<PlayerGenerate>();
        thisPlayerList = playerGenerateScript.GetPlayerList();
        mainCamera = GetComponent<Camera>();
        screenAspect = (float)Screen.height / Screen.width;
    }

    private void Update()
    {
        // zoom
        mainCamera.orthographicSize = Zoom();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        // move
        transform.position = Vector3.SmoothDamp(transform.position, CatchTrackingTarget(), ref velocity, smoothTime);
    }

    // �J�������v���C���[�̒��S�ɒǏ]������
    private Vector3 CatchTrackingTarget()
    {
        float CenterPosX = 0;       // �v���C���[�����̒��SX
        float CenterPosY = 0;       // �v���C���[�����̒��SY
        int divisor = 0;            //����

        for (int i = 0; i < thisPlayerList.Count; i++)
        {
            // �������Ă���v���C���[����
            if (thisPlayerList[i].GetComponent<PlayerDB>().MyIsAlive)
            {
                // �v���C���[�̈ʒu���������ĉ��Z
                CenterPosX += thisPlayerList[i].transform.position.x;
                CenterPosY += thisPlayerList[i].transform.position.y;
                // ��������
                divisor++;
            }
        }

        // ������0
        if (divisor <= 0) { return transform.position; }

        // �I�t�Z�b�g�ʒu�����߂�
        CenterPosX = CenterPosX / divisor;
        CenterPosY = CenterPosY / divisor;

        if(CenterPosY < 0)
		{
            CenterPosY = 0;
		}

        // �J�����̈ړ��ʒu
        return new Vector3(CenterPosX, CenterPosY, -10);
    }
    // �J�������Y�[���C���E�A�E�g������
    private float Zoom()
	{
        int livePlayer = 0;             // ������
        int firstLivePlayer = 0;        // �ŏ��Ɋm�F���ꂽ������
        for (int i = 0; i < thisPlayerList.Count; i++)
		{
			// �������Ă���v���C���[����
			if (thisPlayerList[i].GetComponent<PlayerDB>().MyIsAlive && thisPlayerList[i].transform.position.y > -8)
			{
                firstLivePlayer = i;
                break;
            }
		}

		int mostLeftPlayer = firstLivePlayer;     // �ł����ɂ���v���C���[�̔ԍ�
        int mostRightPlayer = firstLivePlayer;    // �ł��E
        int mostUnderPlayer = firstLivePlayer;    // �ł���
        int mostTopPlayer = firstLivePlayer;      // �ł���

        for (int i = 0; i < thisPlayerList.Count; i++)
        {
            // �������Ă���v���C���[����
            if (thisPlayerList[i].GetComponent<PlayerDB>().MyIsAlive)
            {
                // �v���C���[�̈ʒu������
                // ���ꂼ��̈ʒu���r���čX�V
                if (thisPlayerList[i].transform.position.x < thisPlayerList[mostLeftPlayer].transform.position.x)
                {
                    mostLeftPlayer = i;
                }
                if (thisPlayerList[i].transform.position.x > thisPlayerList[mostRightPlayer].transform.position.x)
                {
                    mostRightPlayer = i;
                }
                if (thisPlayerList[i].transform.position.y < thisPlayerList[mostUnderPlayer].transform.position.y)
                {
                    mostUnderPlayer = i;
                }
                if (thisPlayerList[i].transform.position.y > thisPlayerList[mostTopPlayer].transform.position.y)
                {
                    mostTopPlayer = i;
                }
                livePlayer++;
            }
        }
        // ��������0
        if (livePlayer <= 0) { return mainCamera.orthographicSize; }

        // ����E�E���̈ʒu��ݒ�
        Vector3 LeftTopPosition = new Vector3(thisPlayerList[mostLeftPlayer].transform.position.x, thisPlayerList[mostTopPlayer].transform.position.y,0);
        Vector3 RightUnderPosition = new Vector3(thisPlayerList[mostRightPlayer].transform.position.x, thisPlayerList[mostUnderPlayer].transform.position.y, 0);

        // �Q�_�Ԃ̃x�N�g�����擾
        Vector3 targetsVector = AbsPositionDiff(LeftTopPosition, RightUnderPosition) + (Vector3)ZoomOffset;

        // �A�X�y�N�g�䂪�c���Ȃ�y�̔����A�����Ȃ�x�ƃA�X�y�N�g��ŃJ�����̃T�C�Y���X�V
        float targetsAspect = targetsVector.y / targetsVector.x;
        float targetOrthographicSize = 0;
        if (screenAspect < targetsAspect)
        {
            targetOrthographicSize = targetsVector.y * 0.6f;
        }
        else
        {
            targetOrthographicSize = targetsVector.x * (1 / mainCamera.aspect) * 0.6f;
        }
        // �ŏ��T�C�Y�ȉ��ɂȂ�Ȃ��悤�ɃZ�b�g
        if (targetOrthographicSize < minSize) { targetOrthographicSize = minSize; }

        return targetOrthographicSize;
    }

    private Vector3 AbsPositionDiff(Vector3 target1, Vector3 target2)
    {
        Vector3 targetsDiff = target1 - target2;
        return new Vector3(Mathf.Abs(targetsDiff.x), Mathf.Abs(targetsDiff.y));
    }
}

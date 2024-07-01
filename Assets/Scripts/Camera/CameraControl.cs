using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    private Camera mainCamera;

    [SerializeField] private float minSize = 3;     // カメラ最小サイズ
    //[SerializeField] private float maxUnder = -8;   // カメラが追従する限界値

    [SerializeField] GameObject manager;            // マネージャー
    private PlayerGenerate playerGenerateScript;    // プレイヤー生成スクリプト
    private List<GameObject> thisPlayerList;        // プレイヤーリスト

    [SerializeField] float smoothTime = 0.3f;       // カメラがプレイヤーを追跡する際のスムーズさの調整用パラメータ
    private Vector3 velocity = Vector3.zero;        // カメラ移動時の速度ベクトル       
    
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

    // カメラをプレイヤーの中心に追従させる
    private Vector3 CatchTrackingTarget()
    {
        float CenterPosX = 0;       // プレイヤーたちの中心X
        float CenterPosY = 0;       // プレイヤーたちの中心Y
        int divisor = 0;            //除数

        for (int i = 0; i < thisPlayerList.Count; i++)
        {
            // 生存しているプレイヤーだけ
            if (thisPlayerList[i].GetComponent<PlayerDB>().MyIsAlive)
            {
                // プレイヤーの位置を所得して加算
                CenterPosX += thisPlayerList[i].transform.position.x;
                CenterPosY += thisPlayerList[i].transform.position.y;
                // 除数増加
                divisor++;
            }
        }

        // 除数が0
        if (divisor <= 0) { return transform.position; }

        // オフセット位置を求める
        CenterPosX = CenterPosX / divisor;
        CenterPosY = CenterPosY / divisor;

        if(CenterPosY < 0)
		{
            CenterPosY = 0;
		}

        // カメラの移動位置
        return new Vector3(CenterPosX, CenterPosY, -10);
    }
    // カメラをズームイン・アウトさせる
    private float Zoom()
	{
        int livePlayer = 0;             // 生存数
        int firstLivePlayer = 0;        // 最初に確認された生存者
        for (int i = 0; i < thisPlayerList.Count; i++)
		{
			// 生存しているプレイヤーだけ
			if (thisPlayerList[i].GetComponent<PlayerDB>().MyIsAlive && thisPlayerList[i].transform.position.y > -8)
			{
                firstLivePlayer = i;
                break;
            }
		}

		int mostLeftPlayer = firstLivePlayer;     // 最も左にいるプレイヤーの番号
        int mostRightPlayer = firstLivePlayer;    // 最も右
        int mostUnderPlayer = firstLivePlayer;    // 最も下
        int mostTopPlayer = firstLivePlayer;      // 最も上

        for (int i = 0; i < thisPlayerList.Count; i++)
        {
            // 生存しているプレイヤーだけ
            if (thisPlayerList[i].GetComponent<PlayerDB>().MyIsAlive)
            {
                // プレイヤーの位置を所得
                // それぞれの位置を比較して更新
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
        // 生存数が0
        if (livePlayer <= 0) { return mainCamera.orthographicSize; }

        // 左上・右下の位置を設定
        Vector3 LeftTopPosition = new Vector3(thisPlayerList[mostLeftPlayer].transform.position.x, thisPlayerList[mostTopPlayer].transform.position.y,0);
        Vector3 RightUnderPosition = new Vector3(thisPlayerList[mostRightPlayer].transform.position.x, thisPlayerList[mostUnderPlayer].transform.position.y, 0);

        // ２点間のベクトルを取得
        Vector3 targetsVector = AbsPositionDiff(LeftTopPosition, RightUnderPosition) + (Vector3)ZoomOffset;

        // アスペクト比が縦長ならyの半分、横長ならxとアスペクト比でカメラのサイズを更新
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
        // 最小サイズ以下にならないようにセット
        if (targetOrthographicSize < minSize) { targetOrthographicSize = minSize; }

        return targetOrthographicSize;
    }

    private Vector3 AbsPositionDiff(Vector3 target1, Vector3 target2)
    {
        Vector3 targetsDiff = target1 - target2;
        return new Vector3(Mathf.Abs(targetsDiff.x), Mathf.Abs(targetsDiff.y));
    }
}

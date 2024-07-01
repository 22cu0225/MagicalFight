using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerGenerate : MonoBehaviour
{
    [SerializeField] GameObject playerPrefab;

    GameManager GameManagerScript;

    [SerializeField] private Vector3[] playerPos = new Vector3[]{
        new Vector3(-4.0f, 0.0f, 0.0f) ,
        new Vector3( 4.0f, 0.0f, 0.0f) ,
        new Vector3( 2.0f, 0.0f, 0.0f) ,
        new Vector3(-2.0f, 0.0f, 0.0f) ,
    };

    [SerializeField] private List<GameObject> PlayerPrefubList;
    [SerializeField] private List<GameObject> PlayerList;

    [SerializeField] private GameObject[] hpBars;
    [SerializeField] private GameObject[] playerIcons;


    [SerializeField] private Canvas canvas;
    
    void Awake()
    {
        GameManagerScript = gameObject.GetComponent<GameManager>();
        Debug.Log("Count of players in Generate Script: " + GameManagerScript.playerCount);


        for (int i = 0; i < GameManagerScript.playerCount; i++)
        {
            //GameObject newPlayer = Instantiate(PlayerList[i], playerPos[i], Quaternion.identity);

            //PlayerList.Add(newPlayer);

            ////�v���C���[�Ɏ��ʔԍ�������U��
            ////Player��Number�����������l�I
            //newPlayer.GetComponent<Player>().SetPlayerNumber(i);
            //newPlayer.GetComponent<PlayerDB>().SetNumber(i);
            ////�Ή�����HP�o�[���i�[
            //newPlayer.GetComponent<PlayerDB>().SetHpBar(hpBars[i]);
            ////�i�[����HP�΁[��\������
            //newPlayer.GetComponent<PlayerDB>().MyHpBar.SetActive(true);
            ////HP������������
            //newPlayer.GetComponent<PlayerDB>().SetHp(newPlayer.GetComponent<PlayerDB>().MyMaxHp);
            ////
            //Debug.Log("Number: " + newPlayer.GetComponent<PlayerDB>().MyNumber);

            //-------------------------------------------------------------------------------------
            GameObject newPlayer = Instantiate(PlayerPrefubList[i], playerPos[i], Quaternion.identity);

            PlayerList.Add(newPlayer);

            //�v���C���[�Ɏ��ʔԍ�������U��
            newPlayer.GetComponent<PlayerDB>().SetNumber(i);
            //�Ή�����HP�o�[���i�[
            newPlayer.GetComponent<PlayerDB>().SetHpBar(hpBars[i]);
            //�i�[����HP�΁[��\������
            newPlayer.GetComponent<PlayerDB>().MyHpBar.SetActive(true);
            //HP������������
            newPlayer.GetComponent<PlayerDB>().SetHp(newPlayer.GetComponent<PlayerDB>().MyMaxHp);

            newPlayer.GetComponent<PlayerDB>().SetIsAlive(true);

            //�Ή�����A�C�R�����i�[
            newPlayer.GetComponent<PlayerDB>().SetIcon(playerIcons[i]);
            //�i�[����HP�΁[��\������
            newPlayer.GetComponent<PlayerDB>().MyIcon.SetActive(true);


            //
            Debug.Log("Number: " + newPlayer.GetComponent<PlayerDB>().MyNumber);

        }
    }

    public List<GameObject> GetPlayerList() { return PlayerList; }
}

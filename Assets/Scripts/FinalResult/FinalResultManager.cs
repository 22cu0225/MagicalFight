using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FinalResultManager : MonoBehaviour
{
    SceneController sceneControllerScript;
    GameObject dontDestroyObject;
    SetSingleton setSingleton;

    [SerializeField] GameObject[] players;

    //決定ボタン用
    [SerializeField] GameObject button;
    bool holdButton;

    [SerializeField] float holdTimeLimit;
    float holdTimer;

    void Start()
    {

        sceneControllerScript = this.gameObject.GetComponent<SceneController>();
        holdButton = false;
        dontDestroyObject = GameObject.Find("SceneManager");
        setSingleton = dontDestroyObject.GetComponent<SetSingleton>();
        Debug.Log(setSingleton.GetWinnerNum());
        Instantiate(players[setSingleton.GetWinnerNum()], Vector3.zero, Quaternion.identity);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M) || Input.GetButtonDown("Player1A"))
        {
            holdButton = true;
            button.GetComponent<SpriteRenderer>().color = Color.red;

        }
        else if (Input.GetKeyUp(KeyCode.M) || Input.GetButtonUp("Player1A"))
        {
            holdButton = false;
            holdTimer = 0.0f;
            button.GetComponent<SpriteRenderer>().color = Color.white;

        }

        if (holdButton)
        {
            holdTimer += Time.deltaTime;
            if(holdTimer >= holdTimeLimit)
            {
                //sceneControllerScript.LoadNextScene();
                Debug.Log("シーン遷移");
            }
        }
    }
}

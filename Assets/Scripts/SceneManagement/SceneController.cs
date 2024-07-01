using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    // 他のメソッドや処理を追加する

    public void LoadNextScene()
    {
        // 現在のシーンのインデックスを取得
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        // 次のシーンのインデックスを計算
        int nextSceneIndex = (currentSceneIndex + 1) % SceneManager.sceneCountInBuildSettings;

        // 次のシーンに遷移
        SceneManager.LoadScene(nextSceneIndex);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    // ���̃��\�b�h�⏈����ǉ�����

    public void LoadNextScene()
    {
        // ���݂̃V�[���̃C���f�b�N�X���擾
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        // ���̃V�[���̃C���f�b�N�X���v�Z
        int nextSceneIndex = (currentSceneIndex + 1) % SceneManager.sceneCountInBuildSettings;

        // ���̃V�[���ɑJ��
        SceneManager.LoadScene(nextSceneIndex);
    }
}

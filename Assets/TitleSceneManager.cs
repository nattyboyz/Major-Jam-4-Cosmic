using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleSceneManager : MonoBehaviour
{
    public void Btn_Play()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void Btn_HowToPlay()
    {

    }
}

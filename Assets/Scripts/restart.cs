using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class restart : MonoBehaviour
{
    void Start()
    {
        Cursor.visible = true; // 显示鼠标
    }
    public void restartOnButtonClicked()
    {
        SceneManager.LoadScene("Playground");
    }
    public void endOnButtonClicked()
    {
        SceneManager.LoadScene("startgame");
        Application.Quit();
    }
}

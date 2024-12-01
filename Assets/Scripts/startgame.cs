using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class startgame : MonoBehaviour
{
    void Start()
    {
        Cursor.visible = true; // 显示鼠标
    }
    public void OnStartButtonClicked()
    {
        SceneManager.LoadScene("Playground");
    }
}        

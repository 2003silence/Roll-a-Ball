using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public Transform player;
    private float mouseX, mouseY;
    public float mouseSensitivity;
    private Vector3 offset;
    public float xRotation;
    public bool isFirstPerson = true;  // 当前是否为第一人称视角
    private Vector3 firstsight = new Vector3(0f, 0.747f, 0.143f);
    private Vector3 thirdsight = new Vector3(0f, 1.034f, -2.586f);
    private float anglelimit;

	// Use this for initialization
	void Start () {
	}

	void Update () {
                // 切换视角（按下Tab键）
                if (Input.GetKeyDown(KeyCode.Tab))
                {
                        isFirstPerson = !isFirstPerson; // 切换视角模式
                }
                if(isFirstPerson) 
                {
                        transform.localPosition = firstsight;
                        anglelimit = 70f;
                }
                else 
                {
                        transform.localPosition = thirdsight;
                        anglelimit = 15f;
                }
                mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
                mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

                xRotation -= mouseY;  // 防止鼠标不动时Y轴自动回弹到0
                xRotation = Mathf.Clamp(xRotation, -1.0f * anglelimit, anglelimit);  // 防止Y轴视角转动角度过大

                player.Rotate(Vector3.up * mouseX);
                transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
        }
}

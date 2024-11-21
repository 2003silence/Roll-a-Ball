using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public Transform player;
    private float mouseX, mouseY;
    public float mouseSensitivity;
    private Vector3 offset;
    public float xRotation;

	// Use this for initialization
	void Start () {
        //offset = transform.position - player.transform.position;
	}

	void Update () {
        //transform.position = player.transform.position + offset;
        mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;  // 防止鼠标不动时Y轴自动回弹到0
        xRotation = Mathf.Clamp(xRotation, -70f, 70f);  // 防止Y轴视角转动角度过大

        player.Rotate(Vector3.up * mouseX);
        transform.localRotation = Quaternion.Euler(xRotation, 0, 0);

	}
}

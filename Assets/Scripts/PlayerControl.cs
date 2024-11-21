using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerControl : MonoBehaviour
{
    private CharacterController cc;
    public float movespeed;
    public float jumpspeed;
    private Vector3 dir;

    public float gravity;
    private Vector3 velocity;

    public Transform groundCheck;
    public float checkRadius;
    public LayerMask groundLayer;
    public bool isGround;

    public Text countText;
    public Text winText;

    private int count;

    private Vector3 airMoveDir; // 用于存储空中移动方向

    private void Start()
    {
        cc = GetComponent<CharacterController>();
        count = 0;
        SetCountText();
        winText.text = "";
    }

    private void FixedUpdate()
    {
        // 判断是否着地
        isGround = Physics.CheckSphere(groundCheck.position, checkRadius, groundLayer);
        // 确保垂直速度归零
        if (isGround && velocity.y < 0)
        {
            velocity.y = -3f;
        }
        // 跳跃功能
        if (Input.GetButton("Jump") && isGround) // 这里需要用GetButton而不要用GetButtonDown，不然检测会有延迟
        {
            velocity.y = jumpspeed;
        }
        // 获取当前速度
        float currentSpeed = movespeed;
        // 检测是否按下 Shift 键，按下时二倍速
        if (Input.GetKey(KeyCode.LeftShift))
        {
            currentSpeed *= 2;
        }
        if (isGround)
        {
            // 在地上时允许输入移动
            float moveHorizontal = Input.GetAxis("Horizontal") * currentSpeed;
            float moveVertical = Input.GetAxis("Vertical") * currentSpeed;

            // 设置移动方向
            dir = transform.forward * moveVertical + transform.right * moveHorizontal;
            airMoveDir = dir; // 更新空中方向为当前方向
        }
        else
        {
            // 在空中时锁定移动方向为起跳前的方向
            dir = airMoveDir;
        }

        // 移动
        cc.Move(dir * Time.deltaTime);

        // 自由落体
        velocity.y -= gravity * Time.deltaTime;
        cc.Move(velocity * Time.deltaTime);
    }

    // 捡起物体
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Pick Up"))
        {
            other.gameObject.SetActive(false);
            count++;
            SetCountText();
        }
    }

    // 设置计数器文本
    void SetCountText()
    {
        countText.text = "count " + count.ToString();
        if (count == 6)
        {
            winText.text = "You Win!";
            SceneManager.LoadScene(2);
        }
    }
}

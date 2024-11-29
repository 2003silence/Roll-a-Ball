using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControl : MonoBehaviour
{
    private CharacterController cc;
    private Animator am;
    public float movespeed;
    public float jumpspeed;
    private Vector3 dir;

    public float gravity;
    private Vector3 velocity;

    public Transform groundCheck;
    public float checkRadius;
    public LayerMask groundLayer;
    public bool isGround;

    public float interactRange = 3f; // 交互范围
    public Text feedbackText; // 用于显示提示文字的 UI 元素

    private Vector3 airMoveDir; // 用于存储空中移动方向
    private GameManager gameManager; // 引用 GameManager 脚本

    private void Start()
    {
        cc = GetComponent<CharacterController>();
        am = GetComponent<Animator>();
        gameManager = FindObjectOfType<GameManager>(); // 获取 GameManager 实例
        if (feedbackText != null)
        {
            feedbackText.text = ""; // 初始化提示文字为空
            feedbackText.color = new Color(feedbackText.color.r, feedbackText.color.g, feedbackText.color.b, 0); // 设置透明
        }

        // 锁定鼠标到窗口内并显示，需要先在窗口内点击一下进行锁定
        Cursor.lockState = CursorLockMode.Confined; // 锁定鼠标到游戏窗口内
        Cursor.visible = true; // 显示鼠标
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
            am.SetBool("isrun", true);  // 跑步动作
            currentSpeed *= 2;
        }
        else am.SetBool("isrun", false);

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

        // 走路动作
        if(dir != Vector3.zero) am.SetBool("iswalk", true);
        else am.SetBool("iswalk", false);
        // 跳跃动作
        if(isGround) am.SetBool("isground", true);
        else am.SetBool("isground", false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F)) // 检测键盘 F 键按下
        {
            TryInteractWithEnemy();
        }
    }

    void TryInteractWithEnemy()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, interactRange);
        bool hasInteracted = false; // 标记是否与至少一个敌人交互

        foreach (var hitCollider in hitColliders) // 遍历制止范围内的所有敌人 
        {
            RandomMovement enemy = hitCollider.GetComponent<RandomMovement>();
            if (enemy != null && !enemy.IsStopped()) // 如果是未制止的敌人
            {
                enemy.StopThrowingTrash(); // 制止丢垃圾（触发逃离逻辑）
                hasInteracted = true; // 至少制止了一个敌人
            }
        }

        // 如果有交互，显示提示文字
        if (hasInteracted)
        {
            ShowFeedback("制止成功！");
        }
    }

    // 显示提示文字的方法
    void ShowFeedback(string message)
    {
        if (feedbackText != null)
        {
            feedbackText.text = message;
            StartCoroutine(FadeInAndOut(feedbackText));
        }
    }

    // 淡入淡出效果的协程
    IEnumerator FadeInAndOut(Text text)
    {
        float fadeInTime = 0.5f; // 淡入时间
        float fadeOutTime = 0.5f; // 淡出时间
        float displayTime = 1.0f; // 停留时间

        // 淡入
        for (float t = 0; t < fadeInTime; t += Time.deltaTime)
        {
            float alpha = t / fadeInTime;
            text.color = new Color(text.color.r, text.color.g, text.color.b, alpha);
            yield return null;
        }

        text.color = new Color(text.color.r, text.color.g, text.color.b, 1); // 确保完全显示

        // 停留
        yield return new WaitForSeconds(displayTime);

        // 淡出
        for (float t = 0; t < fadeOutTime; t += Time.deltaTime)
        {
            float alpha = 1 - (t / fadeOutTime);
            text.color = new Color(text.color.r, text.color.g, text.color.b, alpha);
            yield return null;
        }

        text.color = new Color(text.color.r, text.color.g, text.color.b, 0); // 确保完全隐藏
        text.text = ""; // 清空文字内容
    }

    // 捡起物体
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Pick Up"))
        {
            other.gameObject.SetActive(false); // 禁用被拾取的物体
            gameManager.AddScore(); // 通知 GameManager 增加分数
            gameManager.RemoveTrash(); // 减少垃圾计数
        }
    }
}

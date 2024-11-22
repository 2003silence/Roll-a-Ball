using System.Collections;
using UnityEngine;

public class RandomMovement : MonoBehaviour
{
    public float moveSpeed = 3f; // 默认移动速度
    public float escapeSpeedMultiplier = 3f; // 逃离时的速度倍数
    public float changeDirectionTime = 3f; // 每隔多久改变一次方向
    public GameObject beanPrefab; // 豆子预制件
    public Color stoppedColor = Color.green; // 制止后的颜色

    private Vector3 randomDirection; // 随机方向
    private float timer; // 改变方向计时器
    private CharacterController cc;

    private GameManager gameManager; // 引用 GameManager
    private float nextSpawnTime; // 下一次丢垃圾的时间间隔
    private int trashCount = 0; // 当前敌人生成的垃圾数量
    public int maxTrashCount = 3; // 每个敌人最多生成的垃圾数量

    private bool isStopped = false; // 标记是否被制止
    private bool isEscaping = false; // 是否处于逃离状态
    private float escapeTime; // 逃离持续时间
    private float escapeTimer = 0f; // 当前逃离时间计时

    private Renderer renderer; // 用于改变颜色

    void Start()
    {
        cc = GetComponent<CharacterController>(); // 获取 CharacterController
        gameManager = FindObjectOfType<GameManager>(); // 获取 GameManager 实例
        renderer = GetComponent<Renderer>(); // 获取材质渲染器

        ChangeDirection(); // 初始化随机方向

        // 随机初始化丢垃圾的时间间隔
        nextSpawnTime = Random.Range(5f, 15f); // 初始丢垃圾间隔随机 5-15 秒
        StartCoroutine(SpawnBeanCoroutine());
    }

    void Update()
    {
        if (isStopped && !isEscaping) return; // 如果已被制止且不在逃离状态，停止所有动作

        if (isEscaping)
        {
            // 逃离逻辑
            cc.Move(randomDirection * moveSpeed * escapeSpeedMultiplier * Time.deltaTime);
            escapeTimer += Time.deltaTime;

            // 如果逃离时间结束，停止逃离状态
            if (escapeTimer >= escapeTime)
            {
                isEscaping = false; // 结束逃离状态
                ChangeDirection(); // 恢复随机方向
            }
        }
        else
        {
            // 正常移动逻辑
            cc.Move(randomDirection * moveSpeed * Time.deltaTime);

            // 更新方向计时器
            timer += Time.deltaTime;
            if (timer >= changeDirectionTime)
            {
                ChangeDirection(); // 定期改变方向
            }
        }
    }

    void ChangeDirection()
    {
        // 随机生成一个新的方向
        randomDirection = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f)).normalized;
        timer = 0f; // 重置计时器
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        // 检查碰撞的物体是否是障碍物
        if (hit.gameObject.CompareTag("Obstacle"))
        {
            ChangeDirection(); // 撞到障碍物后立即改变方向
        }
    }

    IEnumerator SpawnBeanCoroutine()
    {
        while (trashCount < maxTrashCount && !isStopped) // 仅在垃圾数量未达上限且未被制止时继续生成垃圾
        {
            yield return new WaitForSeconds(nextSpawnTime); // 等待下一次丢垃圾的时间间隔

            // 在当前敌人位置生成豆子
            if (beanPrefab != null)
            {
                Instantiate(beanPrefab, new Vector3(transform.position.x, 0.5f, transform.position.z), Quaternion.identity);
                gameManager.AddTrash(); // 增加全局垃圾计数
                trashCount++; // 增加当前敌人生成的垃圾数量
            }

            // 随机生成下一次丢垃圾的时间间隔
            nextSpawnTime = Random.Range(5f, 15f);
        }
    }

    public void StopThrowingTrash()
    {
        if (isStopped) return; // 避免重复制止
        isStopped = true;
        isEscaping = true; // 进入逃离状态
        escapeTime = changeDirectionTime; // 设置逃离持续时间
        escapeTimer = 0f; // 重置逃离计时器

        // 计算逃离方向（Player 的反方向）
        PlayerControl player = FindObjectOfType<PlayerControl>();
        if (player != null)
        {
            Vector3 toPlayer = (player.transform.position - transform.position).normalized;
            randomDirection = -toPlayer; // 设置为反方向
        }
        else
        {
            randomDirection = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f)).normalized; // 备用随机方向
        }

        // 改变颜色
        if (renderer != null)
        {
            renderer.material.color = stoppedColor;
        }
    }

    public bool IsStopped()
    {
        return isStopped;
    }
}

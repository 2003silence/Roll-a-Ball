using System.Collections;
using UnityEngine;

public class RandomMovement : MonoBehaviour
{
    public float moveSpeed = 3f; // 移动速度
    public float changeDirectionTime = 2f; // 每隔多久改变一次方向
    public GameObject beanPrefab; // 豆子预制件

    private Vector3 randomDirection; // 随机方向
    private float timer; // 改变方向计时器
    private CharacterController cc;

    private GameManager gameManager; // 引用 GameManager
    private float nextSpawnTime; // 下一次丢垃圾的时间间隔
    private int trashCount = 0; // 当前敌人生成的垃圾数量
    public int maxTrashCount = 3; // 每个敌人最多生成的垃圾数量

    void Start()
    {
        cc = GetComponent<CharacterController>(); // 获取 CharacterController
        gameManager = FindObjectOfType<GameManager>(); // 获取 GameManager 实例
        ChangeDirection(); // 初始化随机方向

        // 随机初始化丢垃圾的时间间隔
        nextSpawnTime = Random.Range(5f, 15f); // 初始丢垃圾间隔随机 5-15 秒
        StartCoroutine(SpawnBeanCoroutine());
    }

    void Update()
    {
        // 移动敌人
        cc.Move(randomDirection * moveSpeed * Time.deltaTime);

        // 更新方向计时器
        timer += Time.deltaTime;
        if (timer >= changeDirectionTime)
        {
            ChangeDirection(); // 定期改变方向
        }
    }

    void ChangeDirection()
    {
        // 随机生成一个新的方向
        randomDirection = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f)).normalized;
        timer = 0f; // 重置计时器
    }

    IEnumerator SpawnBeanCoroutine()
    {
        while (trashCount < maxTrashCount) // 仅在垃圾数量未达上限时继续生成垃圾
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
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMovement : MonoBehaviour
{
    public float moveSpeed = 3f; // 移动速度
    public float changeDirectionTime = 2f; // 每隔多久改变一次方向
    public float beanSpawnInterval = 5f; // 每隔多久生成一个豆子
    public GameObject beanPrefab; // 豆子预制件

    private Vector3 randomDirection; // 随机方向
    private float timer; // 改变方向计时器
    private CharacterController cc;

    void Start()
    {
        cc = GetComponent<CharacterController>(); // 获取 CharacterController
        ChangeDirection(); // 初始化随机方向

        // 启动豆子生成协程
        StartCoroutine(SpawnBeanCoroutine());
    }

    void Update()
    {
        // 移动 Capsule
        cc.Move(randomDirection * moveSpeed * Time.deltaTime);

        // 更新方向计时器
        timer += Time.deltaTime;
        if (timer >= changeDirectionTime)
        {
            ChangeDirection(); // 改变方向
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
        while (true)
        {
            yield return new WaitForSeconds(beanSpawnInterval); // 每隔指定时间生成一次豆子

            // 在当前敌人位置生成豆子
            if (beanPrefab != null)
            {
                Instantiate(beanPrefab, new Vector3(transform.position.x, 0.5f, transform.position.z), Quaternion.identity);
            }
        }
    }
}

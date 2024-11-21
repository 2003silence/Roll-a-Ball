using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab; // 敌人预制件
    public int initialEnemyCount = 6; // 初始敌人数量
    public Vector2 boundaryX = new Vector2(-6f, 6f); // X轴边界
    public Vector2 boundaryZ = new Vector2(-6f, 6f); // Z轴边界

    void Start()
    {
        SpawnEnemies(initialEnemyCount); // 开始时生成敌人
    }

    void SpawnEnemies(int enemyCount)
    {
        for (int i = 0; i < enemyCount; i++)
        {
            GameObject enemy = Instantiate(enemyPrefab); // 实例化敌人
            enemy.transform.position = new Vector3(
                Random.Range(boundaryX.x, boundaryX.y),
                1f,
                Random.Range(boundaryZ.x, boundaryZ.y)
            );

            // 自动添加随机移动逻辑
            enemy.AddComponent<RandomMovement>();
        }
    }
}

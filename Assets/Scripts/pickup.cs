using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pickup : MonoBehaviour
{
    public float gravity = 10f;
    private Vector3 velocity;
    private float total = 0f;

    void Update()
    {
        if(total <= 1.0f){
            // 自由落体
            velocity.z += gravity * Time.deltaTime;
            // 累加位移总距离
            total += velocity.z * Time.deltaTime;
            // 使用 Translate 方法直接移动物体
            transform.Translate(velocity * Time.deltaTime);
        }
    }
}

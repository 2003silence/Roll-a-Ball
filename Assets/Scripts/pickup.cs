using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pickup : MonoBehaviour
{
    public float gravity = 3f;
    private Vector3 velocity;
    private float total = 0f;
    public int pickuptype;  // 0是塑料瓶，1是纸盒，2是奶茶杯
    private float fallheight;

    void Start()
    {
        // 随机的水平转向
        float randomRotationX = Random.Range(0f, 360f);
        float randomRotationY = Random.Range(0f, 360f);
        float randomRotationZ = Random.Range(0f, 360f);

        // 旋转为正确角度
        if (pickuptype == 0)
        {
            fallheight = 0.83f;
            transform.rotation = Quaternion.Euler(90, randomRotationY, randomRotationZ);
        }
        else if (pickuptype == 1)
        {
            fallheight = 0.9f;
            transform.rotation = Quaternion.Euler(0, randomRotationY, 90);
        }
        else if (pickuptype == 2)
        {
            fallheight = 0.84f;
            transform.rotation = Quaternion.Euler(0, randomRotationY, 90);
        }
    }
    void Update()
    {
        if(total <= fallheight && pickuptype == 0){
            // 自由落体
            velocity.z += gravity * Time.deltaTime;
            // 累加位移总距离
            total += velocity.z * Time.deltaTime;
            // 使用 Translate 方法直接移动物体
            transform.Translate(velocity * Time.deltaTime);
        }
        else if(total <= fallheight && pickuptype == 1){
            // 自由落体
            velocity.x += gravity * Time.deltaTime;
            // 累加位移总距离
            total += velocity.x * Time.deltaTime;
            // 使用 Translate 方法直接移动物体
            transform.Translate(-1.0f * velocity * Time.deltaTime);
        }
        else if(total <= fallheight && pickuptype == 2){
            // 自由落体
            velocity.x += gravity * Time.deltaTime;
            // 累加位移总距离
            total += velocity.x * Time.deltaTime;
            // 使用 Translate 方法直接移动物体
            transform.Translate(-1.0f * velocity * Time.deltaTime);
        }
    }
}

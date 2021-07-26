using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackBulletController : MonoBehaviour
{
    [SerializeField, Tooltip("最大转弯速度")]
    private float MaximumRotationSpeed = 120.0f;

    [SerializeField, Tooltip("加速度")]
    private float AcceleratedVeocity = 12.8f;

    [SerializeField, Tooltip("最高速度")]
    private float MaximumVelocity = 30.0f;

    [SerializeField, Tooltip("生命周期")]
    private float MaximumLifeTime = 8.0f;

    [SerializeField, Tooltip("上升期时间")]
    private float AccelerationPeriod = 0.5f;

    [HideInInspector]
    public Transform Target = null;        // 目标
    [HideInInspector]
    public float CurrentVelocity = 0.0f;   // 当前速度

    public float attackValue;

    private float lifeTime = 0.0f;            // 生命期

    float Velocity;
    float RotationSpeed;

    private void Start()
    {
        RotationSpeed = Random.Range(MaximumRotationSpeed, MaximumRotationSpeed - 20);
        Velocity = Random.Range(MaximumVelocity, MaximumVelocity - 10);
        AccelerationPeriod = Random.Range(AccelerationPeriod, AccelerationPeriod + 0.4f);
    }

    // 爆炸
    private void Explode()
    {
        // 三秒后删除导弹物体，这时候烟雾已经散去，可以删掉物体了
        Destroy(gameObject);
    }

    private void Update()
    {
        float deltaTime = Time.deltaTime;
        lifeTime += deltaTime;

        // 如果超出生命周期，则直接爆炸。
        if (lifeTime > MaximumLifeTime)
        {
            Explode();
            return;
        }

        // 计算朝向目标的方向偏移量，如果处于上升期，则忽略目标
        Vector3 offset =((lifeTime < AccelerationPeriod) && (Target != null)) ? Vector3.up : (Target.position - transform.position).normalized;

        // 计算当前方向与目标方向的角度差
        float angle = Vector3.Angle(transform.forward, offset);

        // 根据最大旋转速度，计算转向目标共计需要的时间
        float needTime = angle / (RotationSpeed * (CurrentVelocity / Velocity));

        // 如果角度很小，就直接对准目标
        if (Vector3.Distance(Target.position, transform.position) < 30)
        {
            transform.forward = offset;
        }
        else
        {
            // 当前帧间隔时间除以需要的时间，获取本次应该旋转的比例。
            transform.forward = Vector3.Slerp(transform.forward, offset, deltaTime / needTime).normalized;
        }

        // 如果当前速度小于最高速度，则进行加速
        if (CurrentVelocity < MaximumVelocity)
            CurrentVelocity += deltaTime * AcceleratedVeocity;

        // 朝自己的前方位移
        transform.position += transform.forward * CurrentVelocity * deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        // 当发生碰撞，爆炸
        if(other.tag == "Enemy")
        {
            other.gameObject.GetComponent<EnemyStatus>().HP -= attackValue;
            other.gameObject.GetComponent<EnemyStatus>().hasAttack = true;
        }
        Explode();
    }
}

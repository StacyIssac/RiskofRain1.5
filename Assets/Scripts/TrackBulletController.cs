using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackBulletController : MonoBehaviour
{
    public Transform Target = null;        // 目标

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
    public float CurrentVelocity = 0.0f;   // 当前速度

    private AudioSource audioSource = null;   // 音效组件
    private float lifeTime = 0.0f;            // 生命期

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        BulletMove();
    }

    void BulletMove()
    {
        lifeTime += Time.deltaTime;

        // 计算朝向目标的方向偏移量，如果处于上升期，则忽略目标
        Vector3 offset =
            ((lifeTime < AccelerationPeriod) && (Target != null))
            ? Vector3.up
            : (Target.position - transform.position).normalized;

        // 计算当前方向与目标方向的角度差
        float angle = Vector3.Angle(transform.forward, offset);

        // 根据最大旋转速度，计算转向目标共计需要的时间
        float needTime = angle / (MaximumRotationSpeed * (CurrentVelocity / MaximumVelocity));

        // 如果角度很小，就直接对准目标
        if (needTime < 0.001f)
        {
            transform.forward = offset;
        }
        else
        {
            // 当前帧间隔时间除以需要的时间，获取本次应该旋转的比例。
            transform.forward = Vector3.Slerp(transform.forward, offset, Time.deltaTime / needTime).normalized;
        }

        // 如果当前速度小于最高速度，则进行加速
        if (CurrentVelocity < MaximumVelocity)
            CurrentVelocity += Time.deltaTime * AcceleratedVeocity;

        // 朝自己的前方位移
        transform.position += transform.forward * CurrentVelocity * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        Explode();
    }

    private void Explode()
    {
        Destroy(this.gameObject);
    }
}

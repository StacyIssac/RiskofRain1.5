using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackBulletController : MonoBehaviour
{
    public Transform Target = null;        // Ŀ��

    [SerializeField, Tooltip("���ת���ٶ�")]
    private float MaximumRotationSpeed = 120.0f;

    [SerializeField, Tooltip("���ٶ�")]
    private float AcceleratedVeocity = 12.8f;

    [SerializeField, Tooltip("����ٶ�")]
    private float MaximumVelocity = 30.0f;

    [SerializeField, Tooltip("��������")]
    private float MaximumLifeTime = 8.0f;

    [SerializeField, Tooltip("������ʱ��")]
    private float AccelerationPeriod = 0.5f;

    [HideInInspector]
    public float CurrentVelocity = 0.0f;   // ��ǰ�ٶ�

    private AudioSource audioSource = null;   // ��Ч���
    private float lifeTime = 0.0f;            // ������

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

        // ���㳯��Ŀ��ķ���ƫ������������������ڣ������Ŀ��
        Vector3 offset =
            ((lifeTime < AccelerationPeriod) && (Target != null))
            ? Vector3.up
            : (Target.position - transform.position).normalized;

        // ���㵱ǰ������Ŀ�귽��ĽǶȲ�
        float angle = Vector3.Angle(transform.forward, offset);

        // ���������ת�ٶȣ�����ת��Ŀ�깲����Ҫ��ʱ��
        float needTime = angle / (MaximumRotationSpeed * (CurrentVelocity / MaximumVelocity));

        // ����ǶȺ�С����ֱ�Ӷ�׼Ŀ��
        if (needTime < 0.001f)
        {
            transform.forward = offset;
        }
        else
        {
            // ��ǰ֡���ʱ�������Ҫ��ʱ�䣬��ȡ����Ӧ����ת�ı�����
            transform.forward = Vector3.Slerp(transform.forward, offset, Time.deltaTime / needTime).normalized;
        }

        // �����ǰ�ٶ�С������ٶȣ�����м���
        if (CurrentVelocity < MaximumVelocity)
            CurrentVelocity += Time.deltaTime * AcceleratedVeocity;

        // ���Լ���ǰ��λ��
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

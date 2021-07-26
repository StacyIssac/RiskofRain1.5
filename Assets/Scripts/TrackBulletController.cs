using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackBulletController : MonoBehaviour
{
    public Transform target; //��׼��Ŀ��
    Vector3 speed = new Vector3(0, 0, 20); //�ڵ����������ٶ�
    Vector3 lastSpeed; //�洢ת��ǰ�ڵ��ı��������ٶ�
    int rotateSpeed = 100; //��ת���ٶȣ���λ ��/��
    Vector3 finalForward; //Ŀ�굽�������ߵ����������ճ���
    float angleOffset;  //�Լ���forward�����mFinalForward֮��ļн�
    RaycastHit hit;

    void Start()
    {
        //���ڵ��ı��������ٶ�ת��Ϊ��������
        speed = transform.TransformDirection(speed);
    }

    void Update()
    {
        CheckHint();
        UpdateRotation();
        UpdatePosition();
    }

    //���߼�⣬�������Ŀ����������ڵ�
    void CheckHint()
    {
        if (Physics.Raycast(transform.position, transform.forward, out hit))
        {
            if (hit.transform == target && hit.distance < 1)
            {
                Destroy(gameObject);
            }
        }
    }

    //����λ��
    void UpdatePosition()
    {
        transform.position = transform.position + speed * Time.deltaTime;
    }

    //��ת��ʹ�䳯��Ŀ��㣬Ҫ�ı��ٶȵķ���
    void UpdateRotation()
    {
        //�Ƚ��ٶ�תΪ�������꣬��ת֮���ٱ�Ϊ��������
        lastSpeed = transform.InverseTransformDirection(speed);

        ChangeForward(rotateSpeed * Time.deltaTime);

        speed = transform.TransformDirection(lastSpeed);
    }

    void ChangeForward(float speed)
    {
        //���Ŀ��㵽����ĳ���
        finalForward = (target.position - transform.position).normalized;
        if (finalForward != transform.forward)
        {
            angleOffset = Vector3.Angle(transform.forward, finalForward);
            if (angleOffset > rotateSpeed)
            {
                angleOffset = rotateSpeed;
            }
            //������forward��������ת�����ճ���
            transform.forward = Vector3.Lerp(transform.forward, finalForward, speed / angleOffset);
        }
    }
}

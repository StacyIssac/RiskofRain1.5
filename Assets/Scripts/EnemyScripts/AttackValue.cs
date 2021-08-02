using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttackValue : MonoBehaviour
{
    //Ŀ��λ��
    private Vector3 mTarget;

    //��Ļ����
    private Vector3 mScreen;

    //�˺���ֵ
    public int Value;

    //�ı����
    public float ContentWidth = 100f;

    //�ı��߶�
    public float ContentHeight = 3f;

    //�ı�ƫ���ٶ�
    public float ContentSpeed = 10.0f;

    //GUI����
    private Vector2 mPoint;

    //����ʱ��
    public float FreeTime = 5f;

    void Start()
    {
        //��ȡĿ��λ��
        mTarget = transform.position;
        //��ȡ��Ļ����
        mScreen = Camera.main.WorldToScreenPoint(mTarget);
        //����Ļ����ת��ΪGUI����
        mPoint = new Vector2(mScreen.x, Screen.height - mScreen.y);
        //�����Զ������߳�
        StartCoroutine("Free");
    }


    void Update()
    {
        //ʹ�ı��ڴ�ֱ�����ϲ���һ��ƫ��
        transform.Translate(Vector3.up * ContentSpeed * Time.deltaTime);
        //���¼�������
        mTarget = transform.position;
        //��ȡ��Ļ����
        mScreen = Camera.main.WorldToScreenPoint(mTarget);
        //����Ļ����ת��ΪGUI����
        mPoint = new Vector2(mScreen.x, Screen.height - mScreen.y);
    }
    void OnGUI()
    {
        //��֤Ŀ���������ǰ��
        if (mScreen.z > 0)
        {
            GUI.color = Color.red;
            GUI.skin.label.fontSize = 25;
            //�ڲ�ʹ��GUI������л���
            GUI.Label(new Rect(mPoint.x, mPoint.y, ContentWidth, ContentHeight), Value.ToString());
            Debug.Log(1);
        }
    }
    IEnumerator Free()
    {
        yield return new WaitForSeconds(FreeTime);
        Destroy(this.gameObject);
    }
}

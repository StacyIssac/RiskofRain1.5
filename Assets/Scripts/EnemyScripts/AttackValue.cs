using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttackValue : MonoBehaviour
{
    //目标位置
    [HideInInspector]
    public Vector3 mTarget;

    //屏幕坐标
    private Vector3 mScreen;

    //伤害数值
    public int Value;

    //文本宽度
    public float ContentWidth = 100f;

    //文本高度
    public float ContentHeight = 3f;

    //文本偏移速度
    public float ContentSpeed = 10.0f;

    //GUI坐标
    private Vector2 mPoint;

    //销毁时间
    public float FreeTime = 5f;

    GUIStyle gUIStyle = new GUIStyle();

    void Start()
    {
        //获取屏幕坐标
        mScreen = Camera.main.WorldToScreenPoint(mTarget);
        //将屏幕坐标转化为GUI坐标
        mPoint = new Vector2(mScreen.x, Screen.height - mScreen.y);
        //开启自动销毁线程
        StartCoroutine("Free");

        gUIStyle.fontSize = 15;
    }


    void Update()
    {
        //使文本在垂直方向上产生一个偏移
        mTarget = mTarget + Vector3.up * 0.01f;
        //获取屏幕坐标
        mScreen = Camera.main.WorldToScreenPoint(mTarget);
        //将屏幕坐标转化为GUI坐标
        mPoint = new Vector2(mScreen.x, Screen.height - mScreen.y);
    }
    void OnGUI()
    {
        //保证目标在摄像机前方
        if (mScreen.z > 0)
        {
            GUI.color = Color.red;
            GUI.skin.label.fontSize = 25;
            //内部使用GUI坐标进行绘制
            GUI.Label(new Rect(mPoint.x, mPoint.y, ContentWidth, ContentHeight), Value.ToString(), gUIStyle);
        }
    }
    IEnumerator Free()
    {
        yield return new WaitForSeconds(FreeTime);
        Destroy(this.gameObject);
    }
}

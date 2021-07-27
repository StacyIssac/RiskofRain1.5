using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("怪物生成")]
    public Transform playerPos;
    public GameObject[] enemy;
    public int maxCount;
    public int minCount;
    public float maxRange;
    public float minRange;
    public float maxTime;
    public float minTime;
    public float checkDis;
    float tempTime = 0;
    RaycastHit hit;

    // Start is called before the first frame update
    void Start()
    {
        tempTime = Random.Range(maxTime, minTime);
    }

    // Update is called once per frame
    void Update()
    {
        if(tempTime <= 0)
        {
            //随机数获得生成敌人数量
            int enemyCount = Random.Range(maxCount, minCount);
            //随机数获得生成范围
            float range = Random.Range(maxRange, minRange);
            //选择怪物生成的种类
            int temp = Random.Range(0, 2);
            //用数组存储坐标
            Vector2[] enemyPos = new Vector2[enemyCount];
            //循环
            for (int i = 0; i < enemyCount; i++)
            {
                //生成范围为空心圆
                Vector2 p = Random.insideUnitCircle * maxRange;
                Vector2 pos = p.normalized * (minRange + p.magnitude);

                //判断是否与其余怪物生成地点重合
                if(i != 0)
                {
                    if(!CheckPos(enemyPos, i - 1, pos))
                    {
                        i--;
                        continue;
                    }
                }
                enemyPos[i] = pos;

                //创建射线检测的射线
                Ray landRay = new Ray(pos, Vector3.down);
                
                //用射线检测确定地面位置
                if(Physics.Raycast(landRay, out hit))
                {
                    if (hit.transform.tag == "Ground")
                    {
                        Vector3 pos2 = new Vector3(pos.x, 0, pos.y) + playerPos.position;
                        //获得随机方向
                        //var rotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);
                        //朝向玩家生成
                        var rotation = Quaternion.LookRotation(playerPos.position);
                        //生成怪物
                        Instantiate(enemy[temp], pos2, rotation, null);
                    }
                }

                StartCoroutine(Waiting());
            }
            //随机获得怪物生成间隔时间
            tempTime = Random.Range(maxTime, minTime);
        }
        else
        {
            tempTime -= Time.deltaTime;
        }
    }

    bool CheckPos(Vector2[] enemyPos, int count, Vector2 pos)
    {
        for(int i = count; i >= 0; i--)
        {
            var dis = (pos - enemyPos[i]).sqrMagnitude;
            if(dis < checkDis)
            {
                return false;
            }
        }
        return true;
    }

    IEnumerator Waiting()
    {
        yield return new WaitForSeconds(Random.Range(4f, 9f));
    }
}

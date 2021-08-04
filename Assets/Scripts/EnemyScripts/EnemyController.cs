using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("��������")]
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
            //�����������ɵ�������
            int enemyCount = Random.Range(maxCount, minCount);
            //ѡ��������ɵ�����
            int temp = Random.Range(0, 2);
            //������洢����
            Vector2[] enemyPos = new Vector2[enemyCount];
            //ѭ��
            for (int i = 0; i < enemyCount; i++)
            {
                //���ɷ�ΧΪ����Բ
                Vector2 p = Random.insideUnitCircle * maxRange;
                Vector2 pos = p.normalized * (minRange + p.magnitude);

                //�ж��Ƿ�������������ɵص��غ�
                if(i != 0)
                {
                    if(!CheckPos(enemyPos, i - 1, pos))
                    {
                        i--;
                        continue;
                    }
                }
                enemyPos[i] = pos;

                //�������߼�������
                Ray landRayDown = new Ray(new Vector3(pos.x, 20, pos.y), Vector3.down);

                //�����߼��ȷ������λ��
                if (Physics.Raycast(landRayDown, out hit))
                {
                    if (hit.transform.tag == "Ground")
                    {
                        Vector3 pos2 = new Vector3(pos.x, hit.point.y + 2, pos.y) + playerPos.position;
                        //�����������
                        var rotation = Quaternion.LookRotation(playerPos.position);
                        //���ɹ���
                        StartCoroutine(Waiting(temp, pos2, rotation));
                    }
                }
                else
                {
                    i--;
                }
            }
            //�����ù������ɼ��ʱ��
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

    IEnumerator Waiting(int temp, Vector3 pos, Quaternion rotation)
    {
        yield return new WaitForSeconds(Random.Range(4f, 9f));
        Instantiate(enemy[temp], pos, rotation, null);
    }
}

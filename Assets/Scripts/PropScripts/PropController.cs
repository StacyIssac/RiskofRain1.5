using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropController : MonoBehaviour
{
    public GameObject box;
    public GameObject buying;
    public int maxBoxCount;
    public int minBoxCount;
    public float minRange;
    public float maxRange;
    public float checkDis;
    RaycastHit hit;

    // Start is called before the first frame update
    void Start()
    {
        CreateBox();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void CreateBox()
    {
        //��������������������
        int boxCount = Random.Range(maxBoxCount, minBoxCount);
        //������洢����
        Vector2[] boxPos = new Vector2[boxCount];

        //ѭ��
        for (int i = 0; i < boxCount; i++)
        {
            //���ɷ�ΧΪ����Բ
            Vector2 p = Random.insideUnitCircle * maxRange;
            Vector2 pos = p.normalized * (minRange + p.magnitude);

            //�ж��Ƿ�������������ɵص��غ�
            if (i != 0)
            {
                if (!CheckPos(boxPos, i - 1, pos))
                {
                    i--;
                    continue;
                }
            }
            boxPos[i] = pos;

            //�������߼�������
            Ray landRayDown = new Ray(new Vector3(pos.x, 10, pos.y), Vector3.down);

            //�����߼��ȷ������λ��
            if (Physics.Raycast(landRayDown, out hit))
            {
                if (hit.transform.tag == "Ground")
                {
                    Vector3 pos2 = new Vector3(pos.x, hit.point.y + 0.3f, pos.y);

                    //��������
                    Instantiate(box, pos2, Quaternion.identity, null);
                }
            }
            else
            {
                i--;
            }
        }
    }

    bool CheckPos(Vector2[] enemyPos, int count, Vector2 pos)
    {
        for (int i = count; i >= 0; i--)
        {
            var dis = (pos - enemyPos[i]).sqrMagnitude;
            if (dis < checkDis)
            {
                return false;
            }
        }
        return true;
    }
}

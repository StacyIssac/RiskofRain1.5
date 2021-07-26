using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;

public class PlayerSkills : MonoBehaviour
{
    PlayerController playerController;

    [Header("Ѫ��")]
    public float HP;
    public float maxHP;
    public Slider HPSlider;

    [Header("���")]
    //public float shootSpeed = 2f;
    public float shootValue = 10f;
    public float shootRange = 5f;
    public float shootLength = 10f;
    public float capsuleLength = 0.1f;
    //bool canHit = false;
    public float height;
    RaycastHit hit;
    RaycastHit enemyHit;
    Ray ray;

    [Header("����")]
    public CinemachineFreeLook cam1;
    public CinemachineFreeLook cam2;
    public float runningValue;
    float runSpeed;
    int canRunning = -1;

    [Header("˲�Ƴ��")]
    public SkillButtonController rushButton;
    public float rushCDTime;
    public float rushDis;
    public float rushSpeed;
    public float rushMaxTime = 0.2f;
    float rushTimer;
    float rushTime = 0;
    bool canRush = false;
    Vector3 rushVec = Vector3.zero;

    [Header("ѣ�ε�")]
    public SkillButtonController vertigoButton;
    public GameObject vertigoObj;
    public float vertigoCDTime;
    public float createDis;
    public float vertigoSpeed;
    public float vertigoValue;
    public float vertigoRadius;
    float vertigoTimer;

    [Header("���ٵ���")]
    public SkillButtonController trackButton;
    public GameObject trackObj;
    public float trackCDTime;
    public float trackSpeed;
    public float trackValue;
    public float trackTempTimer = 1;
    int putTrack = 0;
    int enemyNums = 0;
    int indexNum = 0;
    float trackNum = 20;
    float trackTimer;
    bool isTrack = false;
    List<GameObject> enemyObjs = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        playerController = GetComponent<PlayerController>();
        //moveSpeed = playerController.moveSpeed;
        runSpeed = playerController.moveSpeed;
        HP = maxHP;

        //���ü���CD
        rushTimer = 0;
        vertigoTimer = 0;
        trackTimer = 0;

        rushButton.skillCDTime = rushCDTime;
        vertigoButton.skillCDTime = vertigoCDTime;
        trackButton.skillCDTime = trackCDTime;
    }

    // Update is called once per frame
    void Update()
    {
        //Ѫ��
        HPSlider.value = HP / maxHP;

        if (canRunning == 1)
        {
            playerController.moveSpeed = runningValue;
            cam1.enabled = true;
            cam2.enabled = false;
        }
        else
        {
            playerController.moveSpeed = runSpeed;
            cam1.enabled = false;
            cam2.enabled = true;
        }

        Shooting();
        Running();

        //���
        if (rushTimer < 0)
        {
            Rush();
        }
        else
        {
            rushTimer -= Time.deltaTime;
        }

        //ѣ��
        if (vertigoTimer < 0)
        {
            Vertigo();
        }
        else
        {
            vertigoTimer -= Time.deltaTime;
        }

        //׷��
        if (trackTimer < 0)
        {
            TrackCheck();
        }
        else
        {
            trackTimer -= Time.deltaTime;
        }

        Track();

        if (canRush)
        {
            if (rushTime == 0)
            {
                //��������ģ��
                this.GetComponent<MeshRenderer>().enabled = false;
                rushTime += Time.deltaTime;
            }
            else if (rushTime > 0 && rushTime < rushMaxTime)
            {
                //�����ƶ�
                if(WallCheck())
                {
                    rushTime = rushMaxTime;
                }
                else
                {
                    transform.position = Vector3.MoveTowards(transform.position, transform.position + rushVec * rushDis, rushSpeed);

                }
                rushTime += Time.deltaTime;
            }
            else if (rushTime >= rushMaxTime)
            {
                //��ʾ����ģ��
                this.GetComponent<MeshRenderer>().enabled = true;
                rushTime = 0;
                canRush = false;
                canRunning = 1;
            }
        }
    }

    void Shooting()
    {
        Vector2 point = new Vector2(Camera.main.pixelWidth / 2, Camera.main.pixelHeight / 2);
        ray = Camera.main.ScreenPointToRay(point);

        //���߼�⵽�ĵ�һ������
        if(Physics.Raycast(ray, out hit))
        {
            //Debug.DrawLine(transform.position, hit.transform.position, Color.red);

            //��ý���
            if (Physics.Linecast(transform.position, hit.transform.position, out hit) && hit.transform.tag == "Enemy")
            {
                enemyHit = hit;
                CanShoot();
            }
            else
            {
                if (enemyHit.collider != null)
                {
                    enemyHit.transform.GetComponent<EnemyStatus>().canSeeHP = false;
                }
            }
        }
        else
        {
            //Debug.DrawLine(transform.position, ray.origin + ray.direction * shootLength, Color.red);
            if (Physics.Linecast(transform.position, ray.origin + ray.direction * shootLength, out hit) && hit.transform.tag == "Enemy")
            {
                CanShoot();
            }
        }
    }

    void CanShoot()
    {
        if(enemyHit.collider != null)
        {
            //׼����׼������ʾHP
            enemyHit.transform.GetComponent<EnemyStatus>().canSeeHP = true;
        }

        //������
        if (Input.GetMouseButtonDown(0))
        {
            enemyHit.transform.gameObject.GetComponent<EnemyStatus>().HP -= shootValue;
            enemyHit.transform.gameObject.GetComponent<EnemyStatus>().hasAttack = true;
        }

        //����ʱ��ϱ���״̬
        canRunning = -1;
    }

    void Running()
    {
        if((Input.GetKeyDown(KeyCode.RightControl) || Input.GetKeyDown(KeyCode.LeftControl)) && Input.GetAxis("Vertical") > 0.1f)
        {
            canRunning = -canRunning;
        }
        else if(Input.GetAxis("Vertical") <= 0.1f)
        {
            canRunning = -1;
        }
    }

    void Rush()
    {
        if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            rushTimer = rushCDTime;
            rushButton.isSkill = true;

            //���ƶ�����
            if (playerController.direction.magnitude >= 0.1f)
            {
                //���ƶ�����˲��
                rushVec = (Quaternion.Euler(0f, playerController.targetAngle, 0f) * Vector3.forward).normalized;
            }
            else //���ƶ�����
            {
                //��ͷָ����˲��
                rushVec = Vector3.Normalize(new Vector3(playerController.moveDir.x, ray.direction.y, playerController.moveDir.z));
            }
            canRush = true;
        }
    }

    void Vertigo()
    {
        if(Input.GetMouseButtonDown(1))
        {
            vertigoTimer = vertigoCDTime;
            vertigoButton.isSkill = true;

            //����һ������
            var moveDir = Vector3.Normalize(playerController.moveDir);
            Instantiate(vertigoObj, transform.position + new Vector3(moveDir.x * createDis, 0, moveDir.z * createDis), Quaternion.identity);
        }
    }

    void TrackCheck()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            trackTimer = trackCDTime;
            trackButton.isSkill = true;

            //����һ�ΰ���Rʱ����������
            if (putTrack == 0 && !isTrack)
            {
                putTrack = 1;
                isTrack = true;
                Debug.Log("open");
            }
        }
    }

    void Track()
    {
        if (Input.GetKeyDown(KeyCode.R) && putTrack == 1 && !isTrack)
        {
            putTrack = 2;
            Debug.Log("shoot");
        }

        if (putTrack == 1)
        {
            //��ý���
            if (Physics.Raycast(ray.origin, ray.direction, out hit) && enemyNums < trackNum)
            {
                if(hit.transform.tag == "Enemy")
                {
                    if (!hit.transform.gameObject.GetComponent<EnemyStatus>().isTrack)
                    {
                        hit.transform.gameObject.GetComponent<EnemyStatus>().isTrack = true;
                        enemyObjs.Add(hit.transform.gameObject);
                    }
                }
            }
            isTrack = false;
        }
        else if (putTrack == 2)
        {
            //���䵼��
            indexNum = 0;
            for (int i = 0; i < trackNum; i++)
            {
                if (indexNum < enemyObjs.Count)
                {
                    var rotation = Quaternion.Euler(0f, Random.Range(0, 360f), Random.Range(0f, 90f));
                    var tempBullet = Instantiate(trackObj, transform.position + new Vector3(0, 3, 0), rotation);
                    tempBullet.GetComponent<TrackBulletController>().Target = enemyObjs[indexNum].transform;
                    indexNum++;
                }
                else
                {
                    indexNum = 0;
                }

            }

            Invoke("UnTrack", 5f);
            putTrack = 0;
        }
    }

    bool WallCheck()
    {
        Ray wallRay = new Ray(transform.position, new Vector3(playerController.moveDir.x, ray.direction.y, playerController.moveDir.z));
        Debug.DrawRay(transform.position, new Vector3(playerController.moveDir.x, ray.direction.y, playerController.moveDir.z));
        if (Physics.Raycast(wallRay, out hit))
        {
            //��ý���
            if (Physics.Linecast(transform.position, hit.transform.position, out hit) && hit.transform.tag == "Ground" && hit.distance < 0.5)
            {
                return true;
            }
        }
        return false;
    }

    void UnTrack()
    {
        for (int i = 0; i < enemyObjs.Count; i++)
        {
            if(enemyObjs[i] != null)
            {
                enemyObjs[i].GetComponent<EnemyStatus>().isTrack = false;
            }
        }

        enemyObjs.Clear();
    }
}

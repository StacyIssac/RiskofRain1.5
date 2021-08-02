using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;

public class PlayerSkills : MonoBehaviour
{
    PlayerController playerController;

    [Header("血量")]
    public int HP;
    public int maxHP;
    public GameObject failPanel;

    [Header("等级")]
    public int maxExp;
    public float shootValAdd;
    public int HPValAdd;
    [HideInInspector]
    public int exp = 0;
    [HideInInspector]
    public int level = 1;
    
    [Header("能量")]
    public int energy = 0;

    [Header("射击")]
    public float shootValue = 10f;
    public float shootRange = 5f;
    public float shootLength = 10f;
    public float capsuleLength = 0.1f;
    public float height;
    RaycastHit hit;
    RaycastHit enemyHit;
    Ray ray;

    [Header("快跑")]
    public CinemachineFreeLook cam1;
    public CinemachineFreeLook cam2;
    public GameObject playerObj;
    public float runningValue;
    float runSpeed;
    int canRunning = -1;

    [Header("瞬移冲刺")]
    public SkillButtonController rushButton;
    public float rushCDTime;
    public float rushDis;
    public float rushSpeed;
    public float rushMaxTime = 0.2f;
    float rushTimer;
    float rushTime = 0;
    bool canRush = false;
    Vector3 rushVec = Vector3.zero;

    [Header("眩晕弹")]
    public SkillButtonController vertigoButton;
    public GameObject vertigoObj;
    public float vertigoCDTime;
    public float createDis;
    public float vertigoSpeed;
    public float vertigoValue;
    public float vertigoRadius;
    float vertigoTimer;

    [Header("跟踪导弹")]
    public SkillButtonController trackButton;
    public GameObject trackObj;
    public float trackCDTime;
    public float trackSpeed;
    public float trackValue;
    public float trackTempTimer = 1;
    public float trackNum = 20;
    int putTrack = 0;
    int enemyNums = 0;
    int indexNum = 0;
    float trackTimer;
    bool isTrack = false;
    List<GameObject> enemyObjs = new List<GameObject>();

    [Header("伤害数值")]
    public GameObject PopupDamage;

    // Start is called before the first frame update
    void Start()
    {
        playerController = GetComponent<PlayerController>();
        //moveSpeed = playerController.moveSpeed;
        runSpeed = playerController.moveSpeed;
        HP = maxHP;

        //设置技能CD
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
        //等级
        if(exp == maxExp)
        {
            level++;
            exp = maxExp - exp;
            shootValue += shootValAdd;
            maxHP += HPValAdd;
            HP += HPValAdd;
        }

        //血量
        if(HP < 0)
        {
            failPanel.SetActive(true);
            Time.timeScale = 0;
        }
        else if(HP > maxHP)
        {
            HP = maxHP;
        }

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

        //冲刺
        if (rushTimer < 0)
        {
            Rush();
        }
        else
        {
            rushTimer -= Time.deltaTime;
        }

        //眩晕
        if (vertigoTimer < 0)
        {
            Vertigo();
        }
        else
        {
            vertigoTimer -= Time.deltaTime;
        }

        //追踪
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
                //隐藏人物模型
                playerObj.SetActive(false);
                rushTime += Time.deltaTime;
            }
            else if (rushTime > 0 && rushTime < rushMaxTime)
            {
                //快速移动
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
                //显示人物模型
                playerObj.SetActive(true);
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

        //射线检测到的第一个物体
        if(Physics.Raycast(ray, out hit))
        {
            //Debug.DrawLine(transform.position, hit.transform.position, Color.red);

            //获得交点
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
            //准心瞄准怪物显示HP
            enemyHit.transform.GetComponent<EnemyStatus>().canSeeHP = true;
        }

        //左键射击
        if (Input.GetMouseButtonDown(0))
        {
            CreateDamageVal(hit.point, (int)shootValue);
            enemyHit.transform.gameObject.GetComponent<EnemyStatus>().HP -= shootValue;
            enemyHit.transform.gameObject.GetComponent<EnemyStatus>().hasAttack = true;
        }

        //攻击时打断奔跑状态
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

            //有移动方向
            if (playerController.direction.magnitude >= 0.1f)
            {
                //向移动方向瞬移
                rushVec = (Quaternion.Euler(0f, playerController.targetAngle, 0f) * Vector3.forward).normalized;
            }
            else //无移动方向
            {
                //向镜头指向方向瞬移
                if(ray.direction.y > 0)
                {
                    rushVec = Vector3.Normalize(new Vector3(playerController.moveDir.x, ray.direction.y, playerController.moveDir.z));
                }
                else
                {
                    rushVec = Vector3.Normalize(new Vector3(playerController.moveDir.x, 0, playerController.moveDir.z));
                }
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

            //创建一个导弹
            var moveDir = Vector3.Normalize(ray.direction);
            var tempVertigo = Instantiate(vertigoObj, transform.position + new Vector3(moveDir.x * createDis, 2, moveDir.z * createDis), Quaternion.identity);
            tempVertigo.GetComponent<VertigoController>().moveForce = ray.direction;
        }
    }

    void TrackCheck()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            trackTimer = trackCDTime;
            trackButton.isSkill = true;

            //当第一次按下R时，开启锁定
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
            //获得交点
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
            //发射导弹
            indexNum = 0;
            for (int i = 0; i < trackNum; i++)
            {
                if (indexNum < enemyObjs.Count)
                {
                    var rotation = Quaternion.Euler(0f, Random.Range(0, 360f), Random.Range(0f, 90f));
                    var tempBullet = Instantiate(trackObj, transform.position + new Vector3(0, 2, 0), rotation);
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
            //获得交点
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

    void CreateDamageVal(Vector3 pos, int value)
    {
        GameObject mObject = (GameObject)Instantiate(PopupDamage, transform.position, Quaternion.identity);
        mObject.GetComponent<AttackValue>().Value = value;
    }
}

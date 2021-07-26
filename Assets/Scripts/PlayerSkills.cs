using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;

public class PlayerSkills : MonoBehaviour
{
    PlayerController playerController;

    [Header("血量")]
    public float HP;
    public float maxHP;
    public Slider HPSlider;

    [Header("射击")]
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

    [Header("快跑")]
    public CinemachineFreeLook cam1;
    public float runningValue;
    float runSpeed;
    int canRunning = -1;

    [Header("瞬移冲刺")]
    public float rushDis;
    public float rushSpeed;
    public float rushMaxTime = 0.2f;
    float rushTime = 0;
    bool canRush = false;
    Vector3 rushVec = Vector3.zero;

    [Header("眩晕弹")]
    public GameObject vertigoObj;
    public float createDis;
    public float vertigoSpeed;
    public float vertigoValue;
    public float vertigoRadius;

    [Header("跟踪导弹")]
    public float trackSpeed;
    public float trackValue;
    bool canTrack = false;
    int putTrack = 0;
    int enemyNums = 0;
    float trackNum = 20;
    GameObject[] enemyObjs = new GameObject[20];

    // Start is called before the first frame update
    void Start()
    {
        playerController = GetComponent<PlayerController>();
        //moveSpeed = playerController.moveSpeed;
        runSpeed = playerController.moveSpeed;
        HP = maxHP;
    }

    // Update is called once per frame
    void Update()
    {
        //血量
        HPSlider.value = HP / maxHP;

        if (canRunning == 1)
        {
            playerController.moveSpeed = runningValue;
            SetCamera(15);
        }
        else
        {
            playerController.moveSpeed = runSpeed;
            SetCamera(5);
        }

        Shooting();
        Running();
        Rush();
        Vertigo();
        Track();

        if (canRush)
        {
            if (rushTime == 0)
            {
                //隐藏人物模型
                this.GetComponent<MeshRenderer>().enabled = false;
                rushTime += Time.deltaTime;
            }
            else if (rushTime > 0 && rushTime < rushMaxTime)
            {
                //快速移动
                transform.position = Vector3.MoveTowards(transform.position, transform.position + rushVec * rushDis, rushSpeed);
                rushTime += Time.deltaTime;
            }
            else if (rushTime >= rushMaxTime)
            {
                //显示人物模型
                this.GetComponent<MeshRenderer>().enabled = true;
                rushTime = 0;
                canRush = false;
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

    void SetCamera(int priority)
    {
        cam1.Priority = priority;
    }

    void Rush()
    {
        if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            //有移动方向
            if (playerController.direction.magnitude >= 0.1f)
            {
                //向移动方向瞬移
                rushVec = (Quaternion.Euler(0f, playerController.targetAngle, 0f) * Vector3.forward).normalized;
            }
            else //无移动方向
            {
                //向镜头指向方向瞬移
                rushVec = playerController.moveDir;
            }
            canRush = true;
        }
    }

    void Vertigo()
    {
        if(Input.GetMouseButtonDown(1))
        {
            //创建一个导弹
            var moveDir = Vector3.Normalize(playerController.moveDir);
            Instantiate(vertigoObj, transform.position + new Vector3(moveDir.x * createDis, 0, moveDir.z * createDis), Quaternion.identity);
        }
    }

    void Track()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            //当第一次按下R时，开启锁定
            if(putTrack == 0)
            {
                putTrack = 1;
                Debug.Log("open");
            }
            //当第二次按下R时，关闭锁定，同时生成导弹
            else if(putTrack == 1)
            {
                putTrack = 2;
            }
        }

        if (putTrack == 1)
        {
            if (Physics.Raycast(ray, out hit))
            {
                //获得交点
                if (Physics.Linecast(transform.position, hit.transform.position, out hit) && hit.transform.tag == "Enemy" && enemyNums < trackNum)
                {
                    if(!hit.transform.gameObject.GetComponent<EnemyStatus>().isTrack)
                    {
                        hit.transform.gameObject.GetComponent<EnemyStatus>().isTrack = true;
                        enemyObjs[enemyNums++] = hit.transform.gameObject;
                    }
                }
            }
        }
        else if (putTrack == 2)
        {
            //发射导弹
            ;
        }
    }
}

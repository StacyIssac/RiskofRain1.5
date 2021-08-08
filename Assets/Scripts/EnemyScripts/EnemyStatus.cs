using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;

public class EnemyStatus : MonoBehaviour
{
    [Header("敌人种类")]
    public int type = 0;

    [Header("行为树")]
    BehaviorTree behaviorTree;

    [Header("生命值")]
    public float HP;
    public float maxHP = 50;
    public bool canSeeHP = false;
    public GameObject HPSlider;
    public float timeHP = 3f;

    [Header("攻击")]
    GameObject player;
    public int attackVal;
    public float attackDis;
    public float maxAttackTime;
    public float minAttackTime;

    [Header("远程攻击")]
    public GameObject bulletObj;
    public float bulletSpeed;

    [Header("受到攻击")]
    public float waitTime;
    public bool hasAttack = false;
    public bool hasVertigo = false;

    [Header("导弹锁定")]
    public GameObject trackObj;
    public bool isTrack = false;

    // Start is called before the first frame update
    void Start()
    {
        HP = maxHP;
        behaviorTree = GetComponent<BehaviorTree>();
        SharedGameObject temp = (SharedGameObject)GameObject.FindGameObjectWithTag("Player");
        behaviorTree.SetVariable("Player", temp);
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        Physics.SyncTransforms();

        //生命值为0时消失
        if (HP <= 0)
        {
            player.GetComponent<PlayerSkills>().exp += 5;
            player.GetComponent<PlayerSkills>().energy += 4;
            Destroy(this.gameObject);
        }
        //准心瞄准时显示HP
        if(canSeeHP)
        {
            HPSlider.SetActive(true);
            timeHP = 3f;
        }
        else if(!canSeeHP)
        {
            if(timeHP < 0)
            {
                HPSlider.SetActive(false);
            }
            else
            {
                timeHP -= Time.deltaTime;
            }
        }

        //被攻击时产生僵直
        if(hasAttack || hasVertigo)
        {
            StartCoroutine(HadAttack());
            canSeeHP = true;
        }

        //如果被导弹瞄准
        if(isTrack)
        {
            trackObj.SetActive(true);
        }
        else
        {
            trackObj.SetActive(false);
        }
    }

    public void CreateBullet()
    {
        var dis = (player.transform.position - transform.position).normalized;
        var bullet = Instantiate(bulletObj, transform.position + dis * 2, Quaternion.identity);
        bullet.GetComponent<BulletController>().speed = bulletSpeed;
        bullet.GetComponent<BulletController>().attackVal = attackVal;
    }

    IEnumerator HadAttack()
    {
        yield return new WaitForSeconds(waitTime);
        hasAttack = false;
        hasVertigo = false;
    }
}

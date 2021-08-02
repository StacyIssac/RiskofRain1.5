using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class EnemyAttack : Action
{
    public EnemyStatus enemyStatus;

    GameObject player;
    float attackDis;
    float attackTime = 0;
    int attackVal;
    float maxAttackTime;
    float minAttackTime;

    // Start is called before the first frame update
    public override void OnStart()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        attackDis = enemyStatus.attackDis * enemyStatus.attackDis;
        attackVal = enemyStatus.attackVal;
        maxAttackTime = enemyStatus.maxAttackTime;
        minAttackTime = enemyStatus.minAttackTime;
    }

    // Update is called once per frame
    public override TaskStatus OnUpdate()
    {
        //攻击模式
        if(enemyStatus.type == 0)
        {
            AttackNear();//近战

        }
        else if(enemyStatus.type == 1)
        {
            AttackFar();//远战
        }

        return TaskStatus.Success;
    }

    void AttackNear()
    {
        var distance = (transform.position - player.transform.position).sqrMagnitude;
        if (distance < attackDis)
        {
            if (attackTime <= 0)
            {
                Debug.Log("attack");
                player.GetComponent<PlayerSkills>().HP -= attackVal;
                attackTime = Random.Range(maxAttackTime, minAttackTime);
            }
            else
            {
                attackTime -= Time.deltaTime;
            }
        }
    }

    void AttackFar()
    {
        var distance = (transform.position - player.transform.position).sqrMagnitude;
        //if (distance < attackDis)
        {
            if (attackTime <= 0)
            {
                enemyStatus.CreateBullet();
                attackTime = Random.Range(maxAttackTime, minAttackTime);
            }
            else
            {
                attackTime -= Time.deltaTime;
            };
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VertigoController : MonoBehaviour
{
    [Header("眩晕")]
    float speed;
    int attackVal;
    float radius;
    float timer;
    bool canVertigo = false;
    [HideInInspector]
    public Vector3 moveForce;
    Rigidbody rigi;
    PlayerController playerController;
    PlayerSkills playerSkills;

    [Header("伤害")]
    public GameObject PopupDamage;

    // Start is called before the first frame update
    void Start()
    {
        playerSkills = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerSkills>();
        speed = playerSkills.vertigoSpeed;
        attackVal = playerSkills.vertigoValue;
        radius = playerSkills.vertigoRadius;
        rigi = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!canVertigo)
        {
            rigi.AddForce(moveForce * speed);
        }
        else
        {
            rigi.AddForce(Vector3.zero);
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius);
            foreach (var hitCollider in hitColliders)
            {
                if(hitCollider.tag == "Enemy")
                {
                    hitCollider.gameObject.GetComponent<EnemyStatus>().hasVertigo = true;
                    hitCollider.gameObject.GetComponent<EnemyStatus>().HP -= attackVal;
                    CreateDamageVal(hitCollider.transform.position, attackVal);
                }
            }
            Destroy(this.gameObject);
        }

        if(timer > 5)
        {
            Destroy(this.gameObject);
        }
        timer += Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!canVertigo)
        {
            GetComponent<MeshRenderer>().enabled = false;
            canVertigo = true;
        }
    }

    void CreateDamageVal(Vector3 pos, int value)
    {
        GameObject mObject = (GameObject)Instantiate(PopupDamage, pos, Quaternion.identity);
        mObject.GetComponent<AttackValue>().Value = value;
        mObject.GetComponent<AttackValue>().mTarget = pos;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [Header("×Óµ¯ÒÆ¶¯")]
    public float speed;
    public int attackVal;
    Vector3 moveForce;
    Rigidbody rigi;
    Transform targetPos;

    // Start is called before the first frame update
    void Start()
    {
        targetPos = GameObject.FindGameObjectWithTag("Player").transform;
        rigi = GetComponent<Rigidbody>();
        moveForce = Vector3.Normalize(targetPos.position - transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        rigi.AddForce(moveForce * speed);
        //transform.position = Vector3.MoveTowards(transform.position, targetPos.position, speed);
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            other.gameObject.GetComponent<PlayerSkills>().HP -= attackVal;
        }
        Destroy(this.gameObject);
    }
}

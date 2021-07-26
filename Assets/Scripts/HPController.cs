using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPController : MonoBehaviour
{
    public Slider HP;
    public EnemyStatus enemyStatus;
    public bool isHP = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(isHP)
        {
            HP.value = enemyStatus.HP / enemyStatus.maxHP;
        }

        transform.LookAt(Camera.main.transform);
    }
}

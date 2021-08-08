using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimController : MonoBehaviour
{
    public GameObject[] aimObj = new GameObject[4];
    Vector3[] aimPos = new Vector3[4];

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < 4; i++)
        {
            aimPos[i] = aimObj[i].transform.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            for(int i = 0; i < 4; i++)
            {
                var dir = Vector3.Normalize(transform.position - aimObj[i].transform.position);
                aimObj[i].transform.position += dir * 1.5f;
            }
        }
        
        if(Input.GetMouseButtonUp(0))
        {
            for(int i = 0; i < 4; i++)
            {
                aimObj[i].transform.position = aimPos[i];
            }
        }
    }
}

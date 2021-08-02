using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillButtonController : MonoBehaviour
{
    Image imageController;
    
    public float skillCDTime;
    public bool isSkill;
    float timer = 0;

    // Start is called before the first frame update
    void Start()
    {
        imageController = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isSkill)
        {
            imageController.fillAmount = 0;
            isSkill = false;
            timer = 0;
        }

        if (timer < skillCDTime)
        {
            timer += Time.deltaTime;
            imageController.fillAmount += Time.deltaTime / skillCDTime;
        }


    }
}

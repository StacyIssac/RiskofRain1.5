using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [Header("暂停面板")]
    public GameObject stopPanel;

    [Header("血量显示")]
    public Text HPText;
    public Slider HPSlider;
    PlayerSkills playerSkills;

    [Header("等级显示")]
    public Text LVText;
    public Slider LVSlider;

    [Header("能量显示")]
    public Text energyText;

    // Start is called before the first frame update
    void Start()
    {
        ToHideCursor();
        playerSkills = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerSkills>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            StopGame();
        }
        //血量
        HPText.text = playerSkills.HP + " / " + playerSkills.maxHP;
        HPSlider.value = (float)playerSkills.HP / (float)playerSkills.maxHP;

        //等级
        LVText.text = "等级:" + playerSkills.level;
        LVSlider.value = (float)playerSkills.exp / (float)playerSkills.maxExp;

        //能量
        energyText.text = "能量:" + playerSkills.energy;
    }

    void StopGame()
    {
        Time.timeScale = 0;
        stopPanel.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
    }

    public void ContinueGame()
    {
        Time.timeScale = 1;
        stopPanel.SetActive(false);
        ToHideCursor();
    }

    public void BackToMain()
    {
        SceneManager.LoadSceneAsync(0);
    }

    void ToHideCursor()
    {
        //隐藏鼠标
        //Cursor.visible = false;
        //锁定鼠标
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            Debug.Log(1);
            other.gameObject.transform.position = new Vector3(0, 2, 0);
        }
    }
}

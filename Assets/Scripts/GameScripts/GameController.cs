using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [Header("��ͣ���")]
    public GameObject stopPanel;

    [Header("Ѫ����ʾ")]
    public Text HPText;
    public Slider HPSlider;
    PlayerSkills playerSkills;

    [Header("�ȼ���ʾ")]
    public Text LVText;
    public Slider LVSlider;

    [Header("������ʾ")]
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
        //Ѫ��
        HPText.text = playerSkills.HP + " / " + playerSkills.maxHP;
        HPSlider.value = (float)playerSkills.HP / (float)playerSkills.maxHP;

        //�ȼ�
        LVText.text = "�ȼ�:" + playerSkills.level;
        LVSlider.value = (float)playerSkills.exp / (float)playerSkills.maxExp;

        //����
        energyText.text = "����:" + playerSkills.energy;
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
        //�������
        //Cursor.visible = false;
        //�������
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

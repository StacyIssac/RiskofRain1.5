using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public GameObject stopPanel;
    // Start is called before the first frame update
    void Start()
    {
        ToHideCursor();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            StopGame();
        }
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
        //Òþ²ØÊó±ê
        //Cursor.visible = false;
        //Ëø¶¨Êó±ê
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

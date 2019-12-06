using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseScript : MonoBehaviour
{
    [SerializeField] GameObject Settings;
    [SerializeField] Blackboard blackboard;

    // Start is called before the first frame update
    void Start()
    {
        if (!blackboard)
            blackboard = GameObject.Find("GameManager").GetComponent<Blackboard>();
    }

    public void Continue()
    {
        blackboard.setPause(false);
    }

    public void ResetScene()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadSettings()
    {
        Settings.SetActive(true);
        gameObject.SetActive(false);
    }

    public void Quit()
    {
        Application.Quit();
    }
}

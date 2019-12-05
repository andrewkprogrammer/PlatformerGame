using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public void LoadSettings()
    {
        Settings.SetActive(true);
        GetComponent<CanvasGroup>().interactable = false;
        Settings.GetComponent<CanvasGroup>().interactable = true;
        gameObject.SetActive(false);
    }

    public void Quit()
    {
        Application.Quit();
    }
}

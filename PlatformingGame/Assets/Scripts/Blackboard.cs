﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Blackboard : MonoBehaviour
{
    [SerializeField]
    GameObject Player;

    [Header("UI")]
    [SerializeField]
    Canvas playerUI;
    [SerializeField]
    Image scoreSprite;
    [SerializeField]
    GameObject CollectedCollectablePrefab;
    [SerializeField]
    TextMeshProUGUI playerScoreText;

    int playerScore;
    Vector2 scoreSpritePos;

    [SerializeField]    GameObject PauseMenu;

    public float XSensitivity { get { return Camera.main.GetComponent<CameraFollow>().xSensitivity; } }
    public float YSensitivity { get { return Camera.main.GetComponent<CameraFollow>().ySensitivity; } }

    bool isPaused = false;
    public bool IsPaused { get { return isPaused; } }

    private void Reset()
    {
        gameObject.name = "GameManager";
        scoreSpritePos = scoreSprite.transform.position;
    }

    // Start is called before the first frame update
    void Start()
    {
        playerScoreText.text = playerScore.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void togglePause()
    {
        isPaused = !isPaused;
        PauseMenu.SetActive(isPaused);

        if (isPaused)
        {
            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.Confined;
        }
        else
        {
            Time.timeScale = 1;
            Cursor.lockState = CursorLockMode.Locked;
        }
        Cursor.visible = isPaused;
    }

    public void setPause(bool setting)
    {
        isPaused = setting;
        PauseMenu.SetActive(isPaused);

        if (isPaused)
        {
            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.Confined;
        }
        else
        {
            Time.timeScale = 1;
            Cursor.lockState = CursorLockMode.Locked;
        }
        Cursor.visible = isPaused;
    }

    public void AddPlayerScore(int amount, Vector3 collectablePos)
    {
        playerScore += amount;
        playerScoreText.text = playerScore.ToString();
        GameObject temp = Instantiate(CollectedCollectablePrefab, playerUI.transform);
        temp.transform.position = Camera.main.WorldToScreenPoint(collectablePos);
        temp.GetComponent<CollectedUIElement>().targetPos = scoreSprite.transform.position;
    }

    public void setMouseCamSensX(float value)
    {
        Camera.main.GetComponent<CameraFollow>().setSensX(value);
    }

    public void setMouseCamSensY(float value)
    {
        Camera.main.GetComponent<CameraFollow>().setSensY(value);
    }
}

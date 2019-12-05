using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Audio;

public class SettingsScript : MonoBehaviour
{
    [SerializeField] Blackboard blackboard;
    [SerializeField] GameObject prevMenu;

    [Header("Screen Settings")]
    [SerializeField] Dropdown windowMode;

    [Header("Audio")]
    [SerializeField] Slider audioSlider;
    [SerializeField] TextMeshProUGUI audioNum;

    [Header("Sensitivity")]
    [SerializeField] Slider xSensSlider;
    [SerializeField] Slider ySensSlider;
    [SerializeField] TextMeshProUGUI xSensNum;
    [SerializeField] TextMeshProUGUI ySensNum;

    // Start is called before the first frame update
    void Start()
    {
        if (!blackboard)
            blackboard = GameObject.Find("GameManager").GetComponent<Blackboard>();

        instantiateScreenSettings();
        instantiateAudioSettings();
        instantiateSensitivitySettings();
    }

    /// <summary>
    /// Makes sure the settings menu has updated info of the current screen settings.
    /// </summary>
    void instantiateScreenSettings()
    {
        int temp = (int)Screen.fullScreenMode;
        if (temp == 3) temp = 2;
        windowMode.value = temp;

        windowMode.onValueChanged.AddListener(windowModeDropdownChange);
    }

    /// <summary>
    /// Makes sure the settings menu has updated info of the current audio settings.
    /// </summary>
    void instantiateAudioSettings()
    {
        audioSlider.value = AudioListener.volume * 100;
        audioNum.text = (AudioListener.volume * 100).ToString();
        audioSlider.onValueChanged.AddListener(audioSliderChange);
    }

    /// <summary>
    /// Makes sure the settings menu has updated info of the current sensitivity settings.
    /// </summary>
    void instantiateSensitivitySettings()
    {
        float tempx = blackboard.XSensitivity;
        float tempy = blackboard.YSensitivity;
        xSensNum.text = tempx.ToString();
        ySensNum.text = tempy.ToString();
        xSensSlider.value = tempx;
        ySensSlider.value = tempy;
        xSensSlider.onValueChanged.AddListener(xSliderChange);
        ySensSlider.onValueChanged.AddListener(ySliderChange);
    }


    /// <summary>
    /// Utilized with a button on the settings menu. Used to return to the previous menu state (I.E. Pause Menu, Main Menu).
    /// </summary>
    public void returnPrevMenu()
    {
        prevMenu.SetActive(true);
        prevMenu.GetComponent<CanvasGroup>().interactable = true;
        GetComponent<CanvasGroup>().interactable = false;
        gameObject.SetActive(false);
    }
    
    /// <summary>
    /// Called whenever the window mode dropdown is changed. Changes the window mode of the game (I.E. fullscreen, borderless, windowed).
    /// Cursor lockstate is toggled to none, then back to confined because of unity shenanigans.
    /// </summary>
    /// <param name="value"> The current state of the dropdown (0-2, but 2 is set to 3 to line up with the "FullScreenMode" struct) </param>
    public void windowModeDropdownChange(int value)
    {
        if (value == 2) value = 3;
        Screen.fullScreenMode = (FullScreenMode)value;
        Cursor.lockState = CursorLockMode.None;
        Cursor.lockState = CursorLockMode.Confined;
    }

    /// <summary>
    /// Called whenever the audio slider value is changed. Changes the game volume.
    /// </summary>
    /// <param name="value"> The current value of the audio slider (1-100 whole number). </param>
    public void audioSliderChange(float value)
    {
        AudioListener.volume = value * 0.01f;
        audioNum.text = value.ToString();
    }

    /// <summary>
    /// Called whenever the X-Sensitivity slider value is changed. Changes X camera sensitivity.
    /// </summary>
    /// <param name="value"> The current value of the X-Sensitivity slider (1-10 float) </param>
    public void xSliderChange(float value)
    {
        value = Mathf.Round(value * 10) * 0.1f;
        xSensNum.text = value.ToString();
        blackboard.setMouseCamSensX(value);
    }

    /// <summary>
    /// Called whenever the Y-Sensitivity slider value is changed. Changes Y camera sensitivity.
    /// </summary>
    /// <param name="value"> The current value of the Y-Sensitivity slider (1-10 float) </param>
    public void ySliderChange(float value)
    {
        value = Mathf.Round(value * 10) * 0.1f;
        ySensNum.text = value.ToString();
        blackboard.setMouseCamSensY(value);
    }

}

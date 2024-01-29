using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class MainMenuManager : MonoBehaviour
{
    enum CamRotationState
    {
        BOARD_MAIN = 0,
        BOARD_SETTINGS = 90,
        BOARD_CREDITS = -90,
    }

    [SerializeField]
    private Camera mainCamera;

    private float cameraTargetRotation = 0.0f;
    private CamRotationState camRotationState = CamRotationState.BOARD_MAIN;

    // Settings Menu
    [SerializeField]
    private Slider settingsMusicVolSlider;
    [SerializeField]
    private TMPro.TextMeshProUGUI settingsMusicVolText;
    [SerializeField]
    private Slider settingsSFXVolSlider;
    [SerializeField]
    private TMPro.TextMeshProUGUI settingsSFXVolText;
    [SerializeField]
    private Slider settingsAmbianceVolSlider;
    [SerializeField]
    private TMPro.TextMeshProUGUI settingsAmbianceVolText;
    [SerializeField]
    private Slider settingsCameraSensSlider;
    [SerializeField]
    private TMPro.TextMeshProUGUI settingsCameraSensText;

    [SerializeField]
    private VideoPlayer introCutscene;
    [SerializeField]
    private GameObject cutsceneBorder;

    [HideInInspector]
    public bool inSubMenu = false;

    private void Start()
    {
        float musicVol = PlayerPrefs.GetFloat("MusicVol");
        settingsMusicVolText.text = "" + musicVol;
        settingsMusicVolSlider.value = musicVol;

        float sfxVol = PlayerPrefs.GetFloat("SFXVol");
        settingsSFXVolText.text = "" + sfxVol;
        settingsSFXVolSlider.value = sfxVol;

        float ambianceVol = PlayerPrefs.GetFloat("AmbianceVol");
        settingsAmbianceVolText.text = "" + ambianceVol;
        settingsAmbianceVolSlider.value = ambianceVol;

        float cameraSense = PlayerPrefs.GetFloat("CameraSens");
        settingsCameraSensText.text = "" + cameraSense;
        settingsCameraSensSlider.value = cameraSense;

    }

    private void Update()
    {
        if (mainCamera.transform.localEulerAngles.y != cameraTargetRotation)
        {
            // Calculate the new rotation
            Vector3 newRotation = new Vector3(0.0f, Mathf.LerpAngle(mainCamera.transform.localEulerAngles.y, cameraTargetRotation, Time.deltaTime * 5.0f), 0.0f);

            // Apply the new rotation
            mainCamera.transform.localEulerAngles = newRotation;
        }
    }

    public void PlayButton()
    {
        introCutscene.Play();
        cutsceneBorder.SetActive(true);
        StartCoroutine(IntroCutsceneSequence());
    }

    public void SettingsButton()
    {
        cameraTargetRotation = (float)CamRotationState.BOARD_SETTINGS;
    }
    public void CreditsButton()
    {
        cameraTargetRotation = (float)CamRotationState.BOARD_CREDITS;
    }
    public void ReturnButton()
    {
        cameraTargetRotation = (float)CamRotationState.BOARD_MAIN;
    }

    public void SettingsMusicSlider()
    {
        settingsMusicVolText.text = "" + (int)(settingsMusicVolSlider.value * 100.0f);
        PlayerPrefs.SetFloat("MusicVol", settingsMusicVolSlider.value);
    }

    public void SettingsSFXSlider()
    {
        settingsSFXVolText.text = "" + (int)(settingsSFXVolSlider.value * 100.0f);
        PlayerPrefs.SetFloat("SFXVol", settingsSFXVolSlider.value);
    }

    public void SettingsAmbianceSlider()
    {
        settingsAmbianceVolText.text = "" + (int)(settingsAmbianceVolSlider.value * 100.0f);
        PlayerPrefs.SetFloat("AmbianceVol", settingsAmbianceVolSlider.value);
    }

    public void SettingsCameraSensSlider()
    {
        settingsCameraSensText.text = "" + (settingsCameraSensSlider.value).ToString("F2")
;
        PlayerPrefs.SetFloat("CameraSens", settingsCameraSensSlider.value);
    }

    public void ReturnMenu()
    {
        // Update variables
        inSubMenu = false;
    }

    private IEnumerator IntroCutsceneSequence()
    {
        yield return new WaitForSeconds((float)introCutscene.length);
        SceneManager.LoadScene("MainScene");
    }
}

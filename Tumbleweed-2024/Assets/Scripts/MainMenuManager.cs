using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    [HideInInspector]
    public bool inSubMenu = false;

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
        SceneManager.LoadScene("MainScene");
    }

    public void SettingsButton()
    {
        cameraTargetRotation = (float)CamRotationState.BOARD_SETTINGS;
    }

    public void ReturnMenu()
    {
        // Update variables
        inSubMenu = false;
    }
}

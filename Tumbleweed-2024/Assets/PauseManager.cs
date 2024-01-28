using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseManager : MonoBehaviour
{
    [HideInInspector]
    public bool isPaused = false;
    [HideInInspector]
    public bool inSubMenu = false;

    [SerializeField]
    private GameObject pauseMenu;
    [SerializeField]
    private GameObject optionsMenu;
    [SerializeField]
    private PlayerInput gameplayControls;

    public void OnPauseInput(InputAction.CallbackContext ctx)
    {
        TogglePause();
    }

    public void TogglePause()
    {
        // Invert pause variable
        isPaused = !isPaused;

        // Update UI and timescale
        if (isPaused)
        {
            // Don't allow the player to move in pause
            gameplayControls.enabled = false;

            inSubMenu = false;
            if (optionsMenu.activeSelf)
                optionsMenu.SetActive(false);
            pauseMenu.SetActive(true);
            Time.timeScale = 0.0f;
        }
        else
        {
            // Allow the player to move again
            gameplayControls.enabled = true;

            inSubMenu = false;
            if (optionsMenu.activeSelf)
                optionsMenu.SetActive(false);
            if (pauseMenu.activeSelf)
                pauseMenu.SetActive(false);
            Time.timeScale = 1.0f;
        }
    }

    public void ResumeButton()
    {
        TogglePause();
    }

    public void OptionsButton()
    {
        // Update variables
        inSubMenu = true;

        // Toggle between menus
        optionsMenu.SetActive(true);
        pauseMenu.SetActive(false);
    }

    public void ReturnMenu()
    {
        // Update variables
        inSubMenu = false;

        // Toggle between menus
        optionsMenu.SetActive(false);
        pauseMenu.SetActive(true);
    }

    public void MainMenuButton()
    {

    }
}

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class PauseMenuManager : MonoBehaviour
{
    [Header("Canvas References")]
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject settingsMenu;
    
    [SerializeField] private PlayerInput player1Input;
    [SerializeField] private PlayerInput player2Input;

    [Header("First Selected Options")]
    [SerializeField] private GameObject pauseMenuFirstSelected;
    [SerializeField] private GameObject settingsMenuFirstSelected;
    
    private bool isPaused = false;
    public static PauseMenuManager Instance { get; private set; }

    
    private void Awake()
    {
        Instance = this;
    }

    public void TogglePause()
    {
        if (isPaused)
            Unpause();
        else
            Pause();
    }

    public void Pause()
    {
        isPaused = true;

        OpenPauseMenu();

        player1Input.SwitchCurrentActionMap("PauseUI");
        player2Input.SwitchCurrentActionMap("PauseUI");
    }

    public void Unpause()
    {
        isPaused = false;

        CloseAllMenus();

        player1Input.SwitchCurrentActionMap("Gameplay");
        player2Input.SwitchCurrentActionMap("Gameplay");
    }

    public void OpenPauseMenu()
    {
        settingsMenu.SetActive(false);
        pauseMenu.SetActive(true);
        EventSystem.current.SetSelectedGameObject(pauseMenuFirstSelected);
    }
    public void OpenSettingsMenu()
    {
        pauseMenu.SetActive(false);
        settingsMenu.SetActive(true);
        EventSystem.current.SetSelectedGameObject(settingsMenuFirstSelected);
    }
    
    
    public void CloseAllMenus()
    {
        pauseMenu.SetActive(false);
        settingsMenu.SetActive(false);
        EventSystem.current.SetSelectedGameObject(null);
    }
}
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class PauseMenuManager : MonoBehaviour
{
    [Header("Player Input References")]
    [SerializeField] private PlayerInput posiPlayerInput;
    [SerializeField] private PlayerInput eliPlayerInput;
    
    [Header("Canvas References")]
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject settingsMenu;
    [SerializeField] private GameObject audioSettingsMenu;
    [SerializeField] private GameObject keyboardSettingsMenu;
    [SerializeField] private GameObject controllerSettingsMenu;

    [Header("First Selected Options")]
    [SerializeField] private GameObject pauseMenuFirstSelected;
    [SerializeField] private GameObject settingsMenuFirstSelected;
    [SerializeField] private GameObject audioSettingsMenuFirstSelected;
    [SerializeField] private GameObject keyboardSettingsMenuFirstSelected;
    [SerializeField] private GameObject controllerSettingsMenuFirstSelected;
    
    private bool isPaused = false;
    public static PauseMenuManager Instance { get; private set; }
    
    private void Awake()
    {
        Instance = this;
        Physics2D.simulationMode = SimulationMode2D.FixedUpdate;
    }
    
    public bool IsPaused
    {
        get => isPaused;
        private set => isPaused = value;
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
        Physics2D.simulationMode = SimulationMode2D.Script;
        
        posiPlayerInput.SwitchCurrentActionMap("PauseUI");
        eliPlayerInput.SwitchCurrentActionMap("PauseUI");
    }

    public void Unpause()
    {
        isPaused = false;

        CloseAllMenus();
        Physics2D.simulationMode = SimulationMode2D.FixedUpdate;

        posiPlayerInput.SwitchCurrentActionMap("Posi");
        eliPlayerInput.SwitchCurrentActionMap("Eli");
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
        audioSettingsMenu.SetActive(true);
        keyboardSettingsMenu.SetActive(false);
        controllerSettingsMenu.SetActive(false);
        EventSystem.current.SetSelectedGameObject(settingsMenuFirstSelected);
    }

    public void OpenAudioSettingsMenu()
    {
        pauseMenu.SetActive(false);
        settingsMenu.SetActive(true);
        audioSettingsMenu.SetActive(true);
        keyboardSettingsMenu.SetActive(false);
        controllerSettingsMenu.SetActive(false);
        EventSystem.current.SetSelectedGameObject(audioSettingsMenuFirstSelected);
    }

    public void OpenKeyBoardSettingsMenu()
    {
        pauseMenu.SetActive(false);
        settingsMenu.SetActive(true);
        audioSettingsMenu.SetActive(false);
        keyboardSettingsMenu.SetActive(true);
        controllerSettingsMenu.SetActive(false);
        EventSystem.current.SetSelectedGameObject(keyboardSettingsMenuFirstSelected);
    }

    public void OpenControllerSettingsMenu()
    {
        pauseMenu.SetActive(false);
        settingsMenu.SetActive(true);
        audioSettingsMenu.SetActive(false);
        keyboardSettingsMenu.SetActive(false);
        controllerSettingsMenu.SetActive(true);
        EventSystem.current.SetSelectedGameObject(controllerSettingsMenuFirstSelected);
    }
    
    public void CloseAllMenus()
    {
        pauseMenu.SetActive(false);
        settingsMenu.SetActive(false);
        audioSettingsMenu.SetActive(false);
        keyboardSettingsMenu.SetActive(false);
        controllerSettingsMenu.SetActive(false);
        EventSystem.current.SetSelectedGameObject(null);
    }
}
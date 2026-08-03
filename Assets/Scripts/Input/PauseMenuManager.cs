using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class PauseMenuManager : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] private InputActionReference pauseAction;

    [Header("Canvas References")]
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject settingsMenu;
    
    [SerializeField] private PlayerInput player1Input;
    [SerializeField] private PlayerInput player2Input;

    [Header("First Selected Options")]
    [SerializeField] private GameObject pauseMenuFirstSelected;
    [SerializeField] private GameObject settingsMenuFirstSelected;
    
    private bool isPaused;

    private void OnEnable()
    {
        pauseAction.action.Enable();
    }

    private void OnDisable()
    {
        pauseAction.action.Disable();
    }

    private void Start()
    {
        pauseMenu.SetActive(false);
    }

    private void Update()
    {
        if (pauseAction.action.WasPressedThisFrame())
        {
            if (isPaused)
                Unpause();
            else
                Pause();
        }
    }

    public void Pause()
    {
        isPaused = true;

        OpenPauseMenu();

        player1Input.DeactivateInput();
        player2Input.DeactivateInput();
    }

    public void Unpause()
    {
        isPaused = false;

        CloseAllMenus();

        player1Input.ActivateInput();
        player2Input.ActivateInput();
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
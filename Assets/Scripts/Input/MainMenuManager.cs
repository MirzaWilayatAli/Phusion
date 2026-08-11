using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class MainMenuManager : MonoBehaviour
{
    [Header("Canvas References")] 
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject levelSelectMenu;
    [SerializeField] private GameObject settingsMenu;
    [SerializeField] private GameObject audioSettingsMenu;
    [SerializeField] private GameObject keyboardSettingsMenu;
    [SerializeField] private GameObject controllerSettingsMenu;
    [SerializeField] private GameObject creditsMenu;
    [SerializeField] private GameObject congratsMenu;
    
    [Header("First Selected Options")]
    [SerializeField] private GameObject mainMenuFirstSelected;
    [SerializeField] private GameObject levelSelectFirstSelected;
    [SerializeField] private GameObject settingsMenuFirstSelected;
    [SerializeField] private GameObject audioSettingsMenuFirstSelected;
    [SerializeField] private GameObject creditsMenuFirstSelected;
    [SerializeField] private GameObject congratsMenuFirstSelected;
    [SerializeField] private GameObject keyboardSettingsMenuFirstSelected;
    [SerializeField] private GameObject controllerSettingsMenuFirstSelected;
    
    private bool isPaused;

    private void Start()
    {
        OpenMainMenu();
    }

    public void OpenMainMenu()
    {
        mainMenu.SetActive(true);
        levelSelectMenu.SetActive(false);
        settingsMenu.SetActive(false);
        creditsMenu.SetActive(false);
        congratsMenu.SetActive(false);
        EventSystem.current.SetSelectedGameObject(mainMenuFirstSelected);
    }

    public void OpenLevelSelectMenu()
    {
        mainMenu.SetActive(false);
        levelSelectMenu.SetActive(true);
        settingsMenu.SetActive(false);
        creditsMenu.SetActive(false);
        congratsMenu.SetActive(false);
        EventSystem.current.SetSelectedGameObject(levelSelectFirstSelected);
    }
    
    public void OpenSettingsMenu()
    {
        mainMenu.SetActive(false);
        levelSelectMenu.SetActive(false);
        settingsMenu.SetActive(true);
        audioSettingsMenu.SetActive(true);
        creditsMenu.SetActive(false);
        congratsMenu.SetActive(false);
        keyboardSettingsMenu.SetActive(false);
        controllerSettingsMenu.SetActive(false);
        EventSystem.current.SetSelectedGameObject(settingsMenuFirstSelected);
    }

    public void OpenCreditsMenu()
    {
        mainMenu.SetActive(false);
        levelSelectMenu.SetActive(false);
        settingsMenu.SetActive(false);
        creditsMenu.SetActive(true);
        congratsMenu.SetActive(false);
        EventSystem.current.SetSelectedGameObject(creditsMenuFirstSelected);
    }

    public void OpenAudioSettingsMenu()
    {
        mainMenu.SetActive(false);
        levelSelectMenu.SetActive(false);
        settingsMenu.SetActive(true);
        creditsMenu.SetActive(false);
        congratsMenu.SetActive(false);
        audioSettingsMenu.SetActive(true);
        keyboardSettingsMenu.SetActive(false);
        controllerSettingsMenu.SetActive(false);
        EventSystem.current.SetSelectedGameObject(audioSettingsMenuFirstSelected);
    }
    
    public void OpenKeyboardSettingsMenu()
    {
        mainMenu.SetActive(false);
        levelSelectMenu.SetActive(false);
        settingsMenu.SetActive(true);
        audioSettingsMenu.SetActive(false);
        creditsMenu.SetActive(false);
        congratsMenu.SetActive(false);
        keyboardSettingsMenu.SetActive(true);
        controllerSettingsMenu.SetActive(false);
        EventSystem.current.SetSelectedGameObject(keyboardSettingsMenuFirstSelected);
    }

    public void OpenControllerSettingsMenu()
    {
        mainMenu.SetActive(false);
        levelSelectMenu.SetActive(false);
        settingsMenu.SetActive(true);
        audioSettingsMenu.SetActive(false);
        creditsMenu.SetActive(false);
        congratsMenu.SetActive(false);
        keyboardSettingsMenu.SetActive(false);
        controllerSettingsMenu.SetActive(true);
        EventSystem.current.SetSelectedGameObject(controllerSettingsMenuFirstSelected);
    }

    public void OpenCongratsMenu()
    {
        mainMenu.SetActive(false);
        levelSelectMenu.SetActive(false);
        settingsMenu.SetActive(false);
        creditsMenu.SetActive(false);
        congratsMenu.SetActive(true);
        EventSystem.current.SetSelectedGameObject(congratsMenuFirstSelected);
    }
}
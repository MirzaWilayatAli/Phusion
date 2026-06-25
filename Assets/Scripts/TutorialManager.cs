using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public GameObject shared;
    public GameObject oneGamepad;
    public GameObject twoGamepads;
    public DeviceAssigner assigner;
    public TutorialPopup_MassFadeOut[] tutorialPopups;
    
    private void OnEnable()
    {
        if (assigner)
        {
            assigner.onTypeChange += OnDeviceChanged;
        }
    }

    public void StartFade()
    {
        foreach (var tutorialPopup in tutorialPopups)
        {
            tutorialPopup.StartFade();
            tutorialPopup.fadeOnAwake = true;
        }
    }
    
    private void OnDeviceChanged()
    {
        switch (assigner.controlType)
        {
            case ControlType.Unknown:
                shared.SetActive(false);
                oneGamepad.SetActive(false);
                twoGamepads.SetActive(false);
                break;
            case ControlType.SharedKeyboard:
                shared.SetActive(true);
                oneGamepad.SetActive(false);
                twoGamepads.SetActive(false);
                break;
            case ControlType.OneGamepadOneKeyboard:
                shared.SetActive(false);
                oneGamepad.SetActive(true);
                twoGamepads.SetActive(false);
                break;
            case ControlType.TwoGamepads:
                shared.SetActive(false);
                oneGamepad.SetActive(false);
                twoGamepads.SetActive(true);
                break;
            default:
                shared.SetActive(false);
                oneGamepad.SetActive(false);
                twoGamepads.SetActive(false);
                break;
        }
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;

public class AllLevelsCompletion : MonoBehaviour
{
    [SerializeField] private GameObject congratulationsPanel;

    private const string CongratsShownKey = "CongratsShown";

    private void Start()
    {
        // Won't show it again if we've already shown it once
        if (PlayerPrefs.GetInt(CongratsShownKey, 0) == 1)
        {
            congratulationsPanel.SetActive(false);
            return;
        }

        bool allComplete = true;

        for (int i = 1; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            if (PlayerPrefs.GetInt("LevelCompleted_" + i, 0) == 0)
            {
                allComplete = false;
                break;
            }
        }

        if (allComplete)
        {
            congratulationsPanel.SetActive(true);

            // Remembers that we've already shown it
            PlayerPrefs.SetInt(CongratsShownKey, 1);
            PlayerPrefs.Save();
        }
        else
        {
            congratulationsPanel.SetActive(false);
        }
    }
}
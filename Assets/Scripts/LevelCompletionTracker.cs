using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelCompletionTracker : MonoBehaviour
{
    public void MarkCurrentLevelComplete()
    {
        int levelIndex = SceneManager.GetActiveScene().buildIndex;

        PlayerPrefs.SetInt("LevelCompleted_" + levelIndex, 1);
        PlayerPrefs.Save();
    }
}
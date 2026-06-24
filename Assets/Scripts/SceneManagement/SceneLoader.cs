using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class SceneLoader : MonoBehaviour
{
    [Header("Fade Settings")]
    [SerializeField] private Image fadeImage;
    [SerializeField] private float fadeDuration = 1f;
    
    private void Start()
    {
        // Fade in when game starts
        StartCoroutine(FadeIn());
    }

    public void ReloadScene()
    {
        // Should just reload the scene, just don't wanna figure out bindings right now. Should be easy to hook up later though.
        StartCoroutine(LoadSceneRoutine(SceneManager.GetActiveScene().name));
    }
    
    public void ReloadSceneWithDelay(float delay)
    {
        StartCoroutine(ReloadSceneWithDelayRoutine(delay));
    }

    private IEnumerator ReloadSceneWithDelayRoutine(float delay)
    {
        yield return new WaitForSeconds(delay);

        StartCoroutine(LoadSceneRoutine(SceneManager.GetActiveScene().name));
    }
    
    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneRoutine(sceneName));
    }

    private IEnumerator LoadSceneRoutine(string sceneName)
    {
        yield return StartCoroutine(FadeOut());

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

        while (operation is { isDone: false }) // change to prevent nullreferenceexception, god bless ReSharp i love you jetbrains
        {
            yield return null;
        }

        yield return StartCoroutine(FadeIn());
    }

    private IEnumerator FadeOut()
    {
        float t = 0f;
        Color color = fadeImage.color;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            color.a = Mathf.Lerp(0f, 1f, t / fadeDuration);
            fadeImage.color = color;
            yield return null;
        }
    }

    private IEnumerator FadeIn()
    {
        float t = 0f;
        Color color = fadeImage.color;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            color.a = Mathf.Lerp(1f, 0f, t / fadeDuration);
            fadeImage.color = color;
            yield return null;
        }
    }
    
    public void QuitGame()
    {
        Application.Quit();

        // This line is only for testing inside Unity Editor
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
    
}


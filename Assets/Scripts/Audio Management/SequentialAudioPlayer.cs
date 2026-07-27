using UnityEngine;
using System.Collections;
using TMPro;

public class SequentialAudioPlayer : MonoBehaviour
{
    [SerializeField] private AudioClip[] audioClips;
    [SerializeField] private TextMeshProUGUI nowPlayingText;
    [SerializeField] private float fadeInDuration = 2f;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        StartCoroutine(PlaySequence());
    }

    private IEnumerator PlaySequence()
    {
        foreach (AudioClip clip in audioClips)
        {
            if (nowPlayingText != null)
            {
                nowPlayingText.text = clip.name;
            }

            audioSource.clip = clip;
            audioSource.volume = 0f;
            audioSource.Play();

            yield return StartCoroutine(FadeIn());
            
            yield return new WaitForSeconds(Mathf.Max(0, clip.length - fadeInDuration));
        }
    }

    private IEnumerator FadeIn()
    {
        float timer = 0f;

        while (timer < fadeInDuration)
        {
            timer += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(0f, 1f, timer / fadeInDuration);
            yield return null;
        }

        audioSource.volume = 1f;
    }
}
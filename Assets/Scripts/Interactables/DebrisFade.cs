using System.Collections;
using UnityEngine;

public class DebrisFade : MonoBehaviour
{
    [SerializeField] private float lifeTime = 2f;
    [SerializeField] private float fadeDuration = 1f;

    private SpriteRenderer[] renderers;

    private void Start()
    {
        renderers = GetComponentsInChildren<SpriteRenderer>();
        StartCoroutine(FadeAndDestroy());
    }

    private IEnumerator FadeAndDestroy()
    {
        yield return new WaitForSeconds(lifeTime);

        float timer = 0f;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;

            float alpha = Mathf.Lerp(1f, 0f, timer / fadeDuration);

            foreach (SpriteRenderer sr in renderers)
            {
                Color color = sr.color;
                color.a = alpha;
                sr.color = color;
            }

            yield return null;
        }

        Destroy(gameObject);
    }
}
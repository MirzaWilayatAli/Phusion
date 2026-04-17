using System.Collections;
using TMPro;
using UnityEngine;

public class FadeOutText : MonoBehaviour
{
    public TMP_Text text;
    public float delay = .2f;
    void Start()
    {
        StartCoroutine(FadeOut());
    }

    IEnumerator FadeOut()
    {
        while (text.color.a > 0)
        {
            var color = text.color;
            color.a -= .01f;
            text.color = color;
            yield return new WaitForSeconds(delay);
        }
    }
}

using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialPopup_Fade : MonoBehaviour
{
    public TMP_Text text;
    public SpriteRenderer sprite;
    public RawImage image;
    public float delay = .2f;
    void Start()
    {
        if(text) StartCoroutine(FadeOutT());
        if(sprite) StartCoroutine(FadeOutS());
        if(image) StartCoroutine(FadeOutI());
    }
    IEnumerator FadeOutT()
    {
        while (text.color.a > 0)
        {
            var color = text.color;
            color.a -= .01f;
            text.color = color;
            yield return new WaitForSeconds(delay);
        }
    }
    
    IEnumerator FadeOutS()
    {
        while (sprite.color.a > 0)
        {
            var color = sprite.color;
            color.a -= .01f;
            sprite.color = color;
            yield return new WaitForSeconds(delay);
        }
    }
    
    IEnumerator FadeOutI()
    {
        while (image.color.a > 0)
        {
            var color = image.color;
            color.a -= .01f;
            image.color = color;
            yield return new WaitForSeconds(delay);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialPopup_MassFadeIn : MonoBehaviour
{
    public List<SpriteRenderer> sprites = new List<SpriteRenderer>();
    public List<TMP_Text> texts = new List<TMP_Text>();
    public float delay = .2f;

    public float fadingStartDelay = 0f;
    public bool fadeOnAwake = false;
    private bool fading = false;
    public float fadeCap = 1f;
    private void OnEnable()
    {
        if(fadeOnAwake) StartFade();
    }

    private IEnumerator StartFadeWithDelay()
    {
        yield return new WaitForSeconds(fadingStartDelay);
        foreach (var c in sprites)
        {
            StartCoroutine(Fade(c));
        }

        foreach (var t in texts)
        {
            StartCoroutine(Fade(t));
        } 
    }
    public void StartFade()
    {
        if (isActiveAndEnabled == false) return;
        if(fading) return;
        fading = true;

        if (fadingStartDelay > 0f)
        {
            StartCoroutine(StartFadeWithDelay());
        }
        else
        {
            foreach (var c in sprites)
            {
                StartCoroutine(Fade(c));
            }

            foreach (var t in texts)
            {
                StartCoroutine(Fade(t));
            } 
        }
    }
    
    IEnumerator Fade(TMP_Text toFade)
    {
        while (toFade.color.a <= fadeCap)
        {
            var color = toFade.color;
            color.a += .01f;
            toFade.color = color;
            yield return new WaitForSeconds(delay);
        }
    }
    IEnumerator Fade(SpriteRenderer toFade)
    {
        while (toFade.color.a <= fadeCap)
        {
            var color = toFade.color;
            color.a += .01f;
            toFade.color = color;
            yield return new WaitForSeconds(delay);
        }
    }
}

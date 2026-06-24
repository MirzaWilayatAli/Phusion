using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialPopup_MassFade : MonoBehaviour
{
    public List<SpriteRenderer> sprites = new List<SpriteRenderer>();
    public List<TMP_Text> texts = new List<TMP_Text>();
    public float delay = .2f;

    public bool fadeOnAwake = false;
    private bool fading = false;
    private void OnEnable()
    {
        if(fadeOnAwake) StartFade();
    }

    public void StartFade()
    {
        if (isActiveAndEnabled == false) return;
        if(fading) return;
        fading = true;
        foreach (var c in sprites)
        {
            StartCoroutine(Fade(c));
        }

        foreach (var t in texts)
        {
            StartCoroutine(Fade(t));
        } 
    }
    
    IEnumerator Fade(TMP_Text toFade)
    {
        while (toFade.color.a > 0)
        {
            var color = toFade.color;
            color.a -= .01f;
            toFade.color = color;
            yield return new WaitForSeconds(delay);
        }
    }
    IEnumerator Fade(SpriteRenderer toFade)
    {
        while (toFade.color.a > 0)
        {
            var color = toFade.color;
            color.a -= .01f;
            toFade.color = color;
            yield return new WaitForSeconds(delay);
        }
    }
}

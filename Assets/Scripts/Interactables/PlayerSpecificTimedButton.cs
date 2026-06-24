using UnityEngine;
using UnityEngine.UI;

public class PlayerSpecificTimedButton : TriggerBase
{
    public GameObject allowedPlayer;
    public float delay = 5f;
    public Slider timerSlider;

    private bool activated;
    private float timer;

    private bool isPlayerOnButton;

    private void Update()
    {
        if (!activated)
            return;

        if (isPlayerOnButton)
        {
            timer = 0f;
            timerSlider.value = delay; // full bar while standing
            return;
        }

        timer += Time.deltaTime;

        timerSlider.value = delay - timer;

        if (timer >= delay)
        {
            activated = false;
            timer = 0f;

            timerSlider.value = 0f;

            onTriggerOff.Invoke();
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject != allowedPlayer)
            return;

        isPlayerOnButton = true;
        
        if (activated)
            return;

        activated = true;
        timer = 0f;

        timerSlider.maxValue = delay;
        timerSlider.value = delay;

        onTriggerOn.Invoke();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject != allowedPlayer)
            return;

        isPlayerOnButton = false;
    }
}
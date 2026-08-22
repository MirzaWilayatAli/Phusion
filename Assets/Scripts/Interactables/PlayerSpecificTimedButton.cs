using System;
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
    [SerializeField] private Transform playerCheckCenter;
    [SerializeField] private Vector2 playerCheckBoxSize = Vector2.one;
    public LayerMask playerCheckMask; 
    private bool _triggerDisable;
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
            _triggerDisable = true;
        }
        
        if (_triggerDisable)
        {
            if (!playerCheckCenter)
            {
                _triggerDisable = false;
                onTriggerOff.Invoke();
                return;
            }
            Collider2D[] colliders = Physics2D.OverlapBoxAll(playerCheckCenter.position, playerCheckBoxSize, 0f, playerCheckMask);
            if (colliders.Length <= 0)
            {
                _triggerDisable = false;
                onTriggerOff.Invoke();
            }
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
        
        PlayerController player = other.gameObject.GetComponent<PlayerController>();
        if (player != null)
        {
            if(RumbleManager.Instance) RumbleManager.Instance.RumblePulse(player.playerID,0.25f,0.75f);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject != allowedPlayer)
            return;

        isPlayerOnButton = false;
    }

    private void OnDrawGizmos()
    {
        if (playerCheckCenter)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(playerCheckCenter.position, playerCheckBoxSize);
        }
    }
}
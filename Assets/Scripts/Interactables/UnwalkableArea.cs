using UnityEngine;

public class UnwalkableArea : MonoBehaviour
{
    public GlobalEventCalls globalEventCalls;

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("PsionForm"))
        {
            if (other.gameObject.TryGetComponent(out PsionFormManager manager))
            {
                globalEventCalls.onPsionDeath.Invoke();
                manager.player1.gameObject.SetActive(false);
                manager.player2.gameObject.SetActive(false);
                RumbleManager.Instance.RumblePulse(0,0.25f,1f);
                RumbleManager.Instance.RumblePulse(1,0.25f,1f);
            }
            else
            {
                if (other.gameObject.TryGetComponent<PlayerController>(out var playerController))
                {
                    globalEventCalls.onPlayerDeath.Invoke();
                    RumbleManager.Instance.RumblePulse(playerController.playerID,0.25f,1f);
                }
            }

            other.gameObject.SetActive(false);
        }
        else
        {
            globalEventCalls.onObjectDestroyed.Invoke();
            Destroy(other.gameObject);
        }
    }
}
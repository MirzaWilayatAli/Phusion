using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class RumbleManager : MonoBehaviour
{
    public static RumbleManager Instance { get; private set; }

    [Header("Default Settings")]
    [SerializeField] private float defaultDuration = 0.25f;

    private bool rumbleEnabled = true;
    private readonly Gamepad[] playerPads = new Gamepad[2];
    private readonly Coroutine[] rumbleCoroutines = new Coroutine[2];

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        
        DontDestroyOnLoad(gameObject);
    }
    
    public bool RumbleEnabled => rumbleEnabled;
    

    public void SetRumbleEnabled(bool enabled)
    {
        Debug.Log($"Rumble set to {enabled}");

        rumbleEnabled = enabled;

        if (!enabled)
            StopAllRumble();
    }
    
    // Called by your DeviceAssigner whenever a player gets assigned a controller.
    
    public void RegisterPlayerPad(int playerID, Gamepad pad)
    {
        if (playerID < 0 || playerID >= playerPads.Length)
            return;

        // Stop rumble on the previously assigned controller.
        if (playerPads[playerID] != null)
            playerPads[playerID].SetMotorSpeeds(0f, 0f);

        if (rumbleCoroutines[playerID] != null)
        {
            StopCoroutine(rumbleCoroutines[playerID]);
            rumbleCoroutines[playerID] = null;
        }

        playerPads[playerID] = pad;
    }

    public void StartRumble(int playerID, float lowFrequency, float highFrequency)
    {
        if (!rumbleEnabled)
            return;

        if (!IsValidPlayer(playerID))
            return;

        Gamepad pad = playerPads[playerID];

        if (pad == null)
            return;

        pad.SetMotorSpeeds(lowFrequency, highFrequency);
    }

    public void StopRumble(int playerID)
    {
        if (!IsValidPlayer(playerID))
            return;

        Gamepad pad = playerPads[playerID];

        if (pad == null)
            return;

        if (rumbleCoroutines[playerID] != null)
        {
            StopCoroutine(rumbleCoroutines[playerID]);
            rumbleCoroutines[playerID] = null;
        }

        pad.SetMotorSpeeds(0f, 0f);
    }

    public void RumblePulse(int playerID, float lowFrequency, float highFrequency)
    {
        RumblePulse(playerID, lowFrequency, highFrequency, defaultDuration);
    }

    public void RumblePulse(int playerID, float lowFrequency, float highFrequency, float duration)
    {
        if (!rumbleEnabled)
            return;

        if (!IsValidPlayer(playerID))
            return;

        Gamepad pad = playerPads[playerID];

        if (pad == null)
            return;

        if (rumbleCoroutines[playerID] != null)
            StopCoroutine(rumbleCoroutines[playerID]);

        pad.SetMotorSpeeds(lowFrequency, highFrequency);

        rumbleCoroutines[playerID] =
            StartCoroutine(StopRumbleAfterTime(playerID, duration));
    }

    private IEnumerator StopRumbleAfterTime(int playerID, float duration)
    {
        yield return new WaitForSeconds(duration);

        Gamepad pad = playerPads[playerID];

        if (pad != null)
            pad.SetMotorSpeeds(0f, 0f);

        rumbleCoroutines[playerID] = null;
    }

    private bool IsValidPlayer(int playerID)
    {
        return playerID >= 0 && playerID < playerPads.Length;
    }

    private void StopAllRumble()
    {
        foreach (Gamepad pad in playerPads)
        {
            if (pad != null)
                pad.SetMotorSpeeds(0f, 0f);
        }
    }

    private void OnDisable()
    {
        StopAllRumble();
    }

    private void OnApplicationQuit()
    {
        StopAllRumble();
    }
}
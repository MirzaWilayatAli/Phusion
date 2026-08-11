using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class RebindManager : MonoBehaviour
{
    public static RebindManager Instance { get; private set; }

    [Header("Separate Input Action Assets")]
    [SerializeField] private InputActionAsset player1ActionAsset;
    [SerializeField] private InputActionAsset player2ActionAsset;

    private const string Player1RebindsKey = "Player1_Rebinds";
    private const string Player2RebindsKey = "Player2_Rebinds";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            
            DontDestroyOnLoad(gameObject);

            LoadAllRebinds();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // =========================================================
    // GET PLAYER ASSET
    // =========================================================

    private InputActionAsset GetPlayerAsset(int playerIndex)
    {
        if (playerIndex == 0)
            return player1ActionAsset;

        if (playerIndex == 1)
            return player2ActionAsset;

        Debug.LogError($"Invalid player index: {playerIndex}");
        return null;
    }

    public InputActionAsset GetPlayerAssetPublic(int playerIndex)
    {
        return GetPlayerAsset(playerIndex);
    }
    
    public void StartRebind(
    int playerIndex,
    string actionName,
    int bindingIndex,
    Button uiButton,
    System.Action onComplete = null,
    System.Action onCancel = null)
{
    InputActionAsset asset = GetPlayerAsset(playerIndex);

    if (asset == null)
        return;

    InputAction action = asset.FindAction(actionName);

    if (action == null)
    {
        Debug.LogWarning(
            $"Action '{actionName}' was not found in Player {playerIndex + 1}'s Input Action Asset."
        );

        if (onCancel != null)
            onCancel();

        return;
    }

    if (bindingIndex < 0 || bindingIndex >= action.bindings.Count)
    {
        Debug.LogWarning(
            $"Invalid binding index {bindingIndex} for action '{actionName}'."
        );

        if (onCancel != null)
            onCancel();

        return;
    }

    InputBinding binding = action.bindings[bindingIndex];

    if (binding.isComposite)
    {
        Debug.LogWarning(
            $"Binding {bindingIndex} is a composite. " +
            $"Rebind one of its individual parts instead."
        );

        if (onCancel != null)
            onCancel();

        return;
    }

    // Remember whether the action was enabled before rebinding.
    bool wasEnabled = action.enabled;

    // Unity requires the action to be disabled before rebinding.
    if (wasEnabled)
    {
        action.Disable();
    }

    if (uiButton != null)
    {
        uiButton.interactable = false;
    }

    Debug.Log(
        $"Waiting for input: Player {playerIndex + 1} | " +
        $"{actionName} | {binding.name}"
    );

    var rebindOperation = action
        .PerformInteractiveRebinding(bindingIndex)

        .WithCancelingThrough("<Keyboard>/escape")

        .WithControlsExcluding("<Mouse>")

        .OnComplete(operation =>
        {
            string newBinding =
                action.bindings[bindingIndex].effectivePath;

            Debug.Log(
                $"Rebind complete: Player {playerIndex + 1} | " +
                $"{actionName} | {binding.name} = {newBinding}"
            );

            // Save the new binding.
            SavePlayerRebinds(playerIndex);

            // Restore the action's previous enabled state.
            if (wasEnabled)
            {
                action.Enable();
            }

            if (uiButton != null)
            {
                uiButton.interactable = true;
            }

            if (onComplete != null)
            {
                onComplete();
            }

            operation.Dispose();
        })

        .OnCancel(operation =>
        {
            Debug.Log(
                $"Rebind cancelled: Player {playerIndex + 1} | " +
                $"{actionName} | {binding.name}"
            );

            // Restore the action's previous enabled state.
            if (wasEnabled)
            {
                action.Enable();
            }

            if (uiButton != null)
            {
                uiButton.interactable = true;
            }

            if (onCancel != null)
            {
                onCancel();
            }

            operation.Dispose();
        });

    rebindOperation.Start();
}

    // =========================================================
    // SAVE
    // =========================================================

    private void SavePlayerRebinds(int playerIndex)
    {
        InputActionAsset asset = GetPlayerAsset(playerIndex);

        if (asset == null)
            return;

        string json = asset.SaveBindingOverridesAsJson();

        string key = GetPlayerSaveKey(playerIndex);

        PlayerPrefs.SetString(key, json);
        PlayerPrefs.Save();

        Debug.Log(
            $"Saved Player {playerIndex + 1} bindings:\n{json}"
        );
    }

    // =========================================================
    // LOAD
    // =========================================================

    private void LoadAllRebinds()
    {
        LoadPlayerRebinds(0);
        LoadPlayerRebinds(1);
    }

    private void LoadPlayerRebinds(int playerIndex)
    {
        InputActionAsset asset = GetPlayerAsset(playerIndex);

        if (asset == null)
            return;

        string key = GetPlayerSaveKey(playerIndex);

        if (!PlayerPrefs.HasKey(key))
        {
            Debug.Log(
                $"No saved rebinds found for Player {playerIndex + 1}."
            );

            return;
        }

        string json = PlayerPrefs.GetString(key);

        if (string.IsNullOrEmpty(json))
            return;

        asset.LoadBindingOverridesFromJson(json);

        Debug.Log(
            $"Loaded Player {playerIndex + 1} bindings:\n{json}"
        );
    }

    // =========================================================
    // RESET
    // =========================================================

    private void ResetPlayerRebinds(int playerIndex)
    {
        InputActionAsset asset = GetPlayerAsset(playerIndex);

        if (asset == null)
            return;

        asset.RemoveAllBindingOverrides();

        PlayerPrefs.DeleteKey(GetPlayerSaveKey(playerIndex));
        PlayerPrefs.Save();

        Debug.Log($"Reset all bindings for Player {playerIndex + 1}.");

        RefreshAllBindingDisplays();
    }

    // =========================================================
    // RESET BOTH PLAYERS
    // =========================================================

    public void RefreshAllBindingDisplays()
    {
        RebindButton[] buttons = FindObjectsByType<RebindButton>(
            FindObjectsInactive.Include,
            FindObjectsSortMode.None
        );

        foreach (RebindButton button in buttons)
        {
            button.RefreshBindingDisplay();
        }
    }
    
    public void ResetAllRebinds()
    {
        ResetPlayerRebinds(0);
        ResetPlayerRebinds(1);
    }

    // =========================================================
    // SAVE KEY
    // =========================================================

    private string GetPlayerSaveKey(int playerIndex)
    {
        return playerIndex == 0
            ? Player1RebindsKey
            : Player2RebindsKey;
    }
}
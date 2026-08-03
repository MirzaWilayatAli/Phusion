using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{
    [SerializeField] private RectTransform rectTransform;

    [Header("Movement")]
    [SerializeField] private float hoverOffsetX = 20f;
    [SerializeField] private float moveSpeed = 8f;

    [Header("Audio")]
    [SerializeField] private AudioClip hoverSound;
    [SerializeField] private AudioSource audioSource;

    private Vector2 originalPosition;
    private Vector2 targetPosition;

    private void Awake()
    {
        originalPosition = rectTransform.anchoredPosition;
        targetPosition = originalPosition;
    }

    private void Update()
    {
        rectTransform.anchoredPosition = Vector2.Lerp(rectTransform.anchoredPosition, targetPosition, Time.deltaTime * moveSpeed);
    }

    // for mouse
    public void OnPointerEnter(PointerEventData eventData)
    {
        HoverEnter();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        HoverExit();
    }

    // well this is called automatically by Unity
    public void OnSelect(BaseEventData eventData)
    {
        HoverEnter();
    }

    public void OnDeselect(BaseEventData eventData)
    {
        HoverExit();
    }
    
    // these are here made into separate methods just in case they need to be called manually somewhere
    public void HoverEnter()
    {
        targetPosition = originalPosition + new Vector2(hoverOffsetX, 0f);

        if (hoverSound != null && audioSource != null)
            audioSource.PlayOneShot(hoverSound);
    }

    public void HoverExit()
    {
        targetPosition = originalPosition;
    }

    
    private void OnDisable()
    {
        HoverExit();
        rectTransform.anchoredPosition = originalPosition;
    }

    
}
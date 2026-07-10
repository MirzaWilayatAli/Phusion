using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public RectTransform rectTransform;

    [Header("Movement")]
    public float hoverOffsetX = 20f;
    public float moveSpeed = 8f;

    private Vector2 originalPosition;
    private Vector2 targetPosition;
    
    public AudioClip hoverSound;
    public AudioSource audioSource;
    
    private void Awake()
    {
        originalPosition = rectTransform.anchoredPosition;
        targetPosition = originalPosition;
    }

    private void Start()
    {
        originalPosition = rectTransform.anchoredPosition;
        targetPosition = originalPosition;
    }

    private void Update()
    {
        rectTransform.anchoredPosition = Vector2.Lerp(rectTransform.anchoredPosition, targetPosition, Time.deltaTime * moveSpeed);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        targetPosition = originalPosition + new Vector2(hoverOffsetX, 0f);
        audioSource.PlayOneShot(hoverSound);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        targetPosition = originalPosition;
    }
    private void OnDisable()
    {
        targetPosition = originalPosition;
        rectTransform.anchoredPosition = originalPosition;
    }
}
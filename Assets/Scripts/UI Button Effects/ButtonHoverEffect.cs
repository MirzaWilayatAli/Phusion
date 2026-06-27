using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public RectTransform hoverImage;

    [Header("Movement")]
    public float hoverOffsetX = 20f;
    public float moveSpeed = 8f;

    private Vector2 originalPosition;
    private Vector2 targetPosition;

    private void Start()
    {
        originalPosition = hoverImage.anchoredPosition;
        targetPosition = originalPosition;
    }

    private void Update()
    {
        hoverImage.anchoredPosition = Vector2.Lerp(hoverImage.anchoredPosition, targetPosition, Time.deltaTime * moveSpeed);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        targetPosition = originalPosition + new Vector2(hoverOffsetX, 0f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        targetPosition = originalPosition;
    }
}
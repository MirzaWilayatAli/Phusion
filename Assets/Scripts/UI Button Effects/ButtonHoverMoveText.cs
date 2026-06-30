using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHoverMoveText : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public RectTransform buttonText;
    public float moveDistance = 20f;

    private Vector2 originalPosition;

    private Vector2 targetPosition;

    private void Start()
    {
        originalPosition = buttonText.anchoredPosition;
        targetPosition = originalPosition;
    }

    private void Update()
    {
        buttonText.anchoredPosition = Vector2.Lerp(
            buttonText.anchoredPosition,
            targetPosition,
            Time.deltaTime * 8f
        );
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        targetPosition = originalPosition + Vector2.right * moveDistance;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        targetPosition = originalPosition;
    }
    private void OnDisable()
    {
        targetPosition = originalPosition;
        buttonText.anchoredPosition = originalPosition;
    }
}
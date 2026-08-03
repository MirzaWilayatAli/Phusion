using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHoverMoveText : MonoBehaviour,
    IPointerEnterHandler,
    IPointerExitHandler,
    ISelectHandler,
    IDeselectHandler
{
    [SerializeField] private RectTransform buttonText;
    [SerializeField] private float moveDistance = 20f;
    [SerializeField] private float moveSpeed = 8f;

    private Vector2 originalPosition;
    private Vector2 targetPosition;

    private void Awake()
    {
        originalPosition = buttonText.anchoredPosition;
        targetPosition = originalPosition;
    }

    private void Update()
    {
        buttonText.anchoredPosition = Vector2.Lerp(
            buttonText.anchoredPosition,
            targetPosition,
            Time.deltaTime * moveSpeed
        );
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        HoverEnter();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        HoverExit();
    }
    
    public void OnSelect(BaseEventData eventData)
    {
        HoverEnter();
    }

    public void OnDeselect(BaseEventData eventData)
    {
        HoverExit();
    }
    
    public void HoverEnter()
    {
        targetPosition = originalPosition + Vector2.right * moveDistance;
    }

    public void HoverExit()
    {
        targetPosition = originalPosition;
    }

    private void OnDisable()
    {
        HoverExit();
        buttonText.anchoredPosition = originalPosition;
    }
}
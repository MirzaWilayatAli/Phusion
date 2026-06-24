using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonTextScale : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public RectTransform buttonText;

    [Header("Scale")]
    public float hoverScale = 1.1f;
    public float scaleSpeed = 8f;

    private Vector3 originalScale;
    private Vector3 targetScale;

    private void Start()
    {
        originalScale = buttonText.localScale;
        targetScale = originalScale;
    }

    private void Update()
    {
        buttonText.localScale = Vector3.Lerp(
            buttonText.localScale,
            targetScale,
            Time.deltaTime * scaleSpeed
        );
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        targetScale = originalScale * hoverScale;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        targetScale = originalScale;
    }
}

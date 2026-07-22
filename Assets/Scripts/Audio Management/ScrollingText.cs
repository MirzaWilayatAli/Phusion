using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class ScrollingText : MonoBehaviour
{
    public float scrollSpeed = 60f;
    public float startX = 400f;
    public float endX = -800f;

    private RectTransform rect;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    private void Update()
    {
        rect.anchoredPosition += Vector2.left * scrollSpeed * Time.deltaTime;

        if (rect.anchoredPosition.x <= endX)
        {
            rect.anchoredPosition = new Vector2(startX, rect.anchoredPosition.y);
        }
    }

    public void RestartScroll()
    {
        rect.anchoredPosition = new Vector2(startX, rect.anchoredPosition.y);
    }
}
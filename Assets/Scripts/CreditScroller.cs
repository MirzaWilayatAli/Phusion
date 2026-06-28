using UnityEngine;
using UnityEngine.EventSystems;

public class CreditScroller : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
        public float step = 60f;
        public Transform startPos;
        public Transform endPos;

        private bool _paused;
        private float _step;

        public void OnPointerEnter(PointerEventData eventData)
        {
                _paused = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
                _paused = false;
        }

        private void Awake()
        {
                transform.position = startPos.position;
        }

        private void OnDisable()
        {
                transform.position = startPos.position;
        }

        private void FixedUpdate()
        {
                if (!_paused) transform.position = Vector3.Distance(transform.position, endPos.position) <= 0.15f ? startPos.position : Vector3.MoveTowards(transform.position, endPos.position, step * Time.fixedDeltaTime);
        }
}

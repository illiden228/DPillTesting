using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.OnScreen;

public class JoystickView : OnScreenControl, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    [SerializeField] private RectTransform _backgroundRect;
    [SerializeField] private RectTransform _knob;

    public void Init()
    {
        _backgroundRect.gameObject.SetActive(false);
    }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (eventData == null)
                throw new System.ArgumentNullException(nameof(eventData));
            _backgroundRect.anchoredPosition = eventData.position;
            _backgroundRect.gameObject.SetActive(true);

            RectTransformUtility.ScreenPointToLocalPointInRectangle(_backgroundRect, eventData.position, eventData.pressEventCamera, out m_PointerDownPos);
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (eventData == null)
                throw new System.ArgumentNullException(nameof(eventData));

            RectTransformUtility.ScreenPointToLocalPointInRectangle(_backgroundRect, eventData.position, eventData.pressEventCamera, out var position);
            var delta = position - m_PointerDownPos;

            delta = Vector2.ClampMagnitude(delta, movementRange);
            _knob.anchoredPosition = m_StartPos + (Vector3)delta;

            var newPos = new Vector2(delta.x / movementRange, delta.y / movementRange);
            SendValueToControl(newPos);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _backgroundRect.gameObject.SetActive(false);
            _knob.anchoredPosition = m_StartPos;
            SendValueToControl(Vector2.zero);
        }

        private void Start()
        {
            m_StartPos = _knob.anchoredPosition;
        }

        public float movementRange
        {
            get => m_MovementRange;
            set => m_MovementRange = value;
        }

        [SerializeField]
        private float m_MovementRange = 50;

        [SerializeField]
        [InputControl(layout = "Vector2")]
        private string m_ControlPath;

        private Vector3 m_StartPos;
        private Vector2 m_PointerDownPos;

        protected override string controlPathInternal
        {
            get => m_ControlPath;
            set => m_ControlPath = value;
        }
}
using UnityEngine;
using UnityEngine.EventSystems;

public sealed class FloatingJoystick : MonoBehaviour,
    IPointerDownHandler,
    IPointerUpHandler,
    IDragHandler
{
    [Header("References")]
    [SerializeField] private RectTransform inputArea;
    [SerializeField] private RectTransform background;
    [SerializeField] private RectTransform handle;

    [Header("Settings")]
    [SerializeField, Range(0.1f, 1f)]
    private float handleRange = 0.65f;

    private Vector2 input;

    public Vector2 Input => input;

    private void Awake()
    {
        ValidateReferences();
        ResetJoystick();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        ShowJoystickAt(eventData);
        UpdateInput(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        UpdateInput(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        ResetJoystick();
    }

    private void ShowJoystickAt(PointerEventData eventData)
    {
        if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(
                inputArea,
                eventData.position,
                eventData.pressEventCamera,
                out Vector2 localPoint))
        {
            return;
        }

        background.anchoredPosition = ClampBackgroundPosition(localPoint);
        background.gameObject.SetActive(true);

        handle.anchoredPosition = Vector2.zero;
    }

    private void UpdateInput(PointerEventData eventData)
    {
        if (!background.gameObject.activeSelf)
        {
            return;
        }

        if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(
                background,
                eventData.position,
                eventData.pressEventCamera,
                out Vector2 localPoint))
        {
            return;
        }

        Vector2 radius = background.rect.size * 0.5f;

        if (radius.x <= 0f || radius.y <= 0f)
        {
            input = Vector2.zero;
            return;
        }

        Vector2 normalizedInput = new Vector2(
            localPoint.x / radius.x,
            localPoint.y / radius.y);

        input = Vector2.ClampMagnitude(normalizedInput, 1f);

        handle.anchoredPosition = new Vector2(
            input.x * radius.x * handleRange,
            input.y * radius.y * handleRange);
    }

    private Vector2 ClampBackgroundPosition(Vector2 position)
    {
        Vector2 inputAreaHalfSize = inputArea.rect.size * 0.5f;
        Vector2 backgroundHalfSize = background.rect.size * 0.5f;

        float minX = -inputAreaHalfSize.x + backgroundHalfSize.x;
        float maxX = inputAreaHalfSize.x - backgroundHalfSize.x;

        float minY = -inputAreaHalfSize.y + backgroundHalfSize.y;
        float maxY = inputAreaHalfSize.y - backgroundHalfSize.y;

        return new Vector2(
            Mathf.Clamp(position.x, minX, maxX),
            Mathf.Clamp(position.y, minY, maxY));
    }

    private void ResetJoystick()
    {
        input = Vector2.zero;

        if (handle != null)
        {
            handle.anchoredPosition = Vector2.zero;
        }

        if (background != null)
        {
            background.gameObject.SetActive(false);
        }
    }

    private void ValidateReferences()
    {
        if (inputArea != null &&
            background != null &&
            handle != null)
        {
            return;
        }

        Debug.LogError(
            $"{nameof(FloatingJoystick)} on {gameObject.name} has missing references.",
            this);

        enabled = false;
    }

    private void OnDisable()
    {
        ResetJoystick();
    }
}
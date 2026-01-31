using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;

public class AnimatedButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    [Header("Animation Settings")]
    [SerializeField]
    private float scaleUpDuration = 0.05f;  // How fast the button scales up on click
    [SerializeField]
    private float scaleDownDuration = 0.1f; // How fast the button scales back down on click
    [SerializeField]
    private float clickScaleFactor = 0.95f; // How much bigger the button gets on click
    [SerializeField]
    private float hoverScaleFactor = 1.1f; // How much bigger the button gets on hover
    [SerializeField]
    private float hoverDuration = 0.1f;     // How fast the button scales up/down on hover

    private Vector3 _originalScale;
    private Coroutine _animationCoroutine;
    private Button _button;

    void Awake()
    {
        _button = GetComponent<Button>();
        _originalScale = transform.localScale;
    }

    // Called when the pointer enters the button's area
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_button.interactable) // Only animate if the button is interactable
        {
            if (_animationCoroutine != null) StopCoroutine(_animationCoroutine);
            _animationCoroutine = StartCoroutine(AnimateScale(transform.localScale, _originalScale * hoverScaleFactor, hoverDuration));
        }
    }

    // Called when the pointer exits the button's area
    public void OnPointerExit(PointerEventData eventData)
    {
        if (_button.interactable) // Only animate if the button is interactable
        {
            if (_animationCoroutine != null) StopCoroutine(_animationCoroutine);
            _animationCoroutine = StartCoroutine(AnimateScale(transform.localScale, _originalScale, hoverDuration));
        }
    }

    // Called when the pointer is pressed down on the button
    public void OnPointerDown(PointerEventData eventData)
    {
        if (_button.interactable) // Only animate if the button is interactable
        {
            if (_animationCoroutine != null) StopCoroutine(_animationCoroutine);
            _animationCoroutine = StartCoroutine(AnimateScale(transform.localScale, _originalScale * clickScaleFactor, scaleUpDuration));
        }
    }

    // Called when the pointer is released from the button
    public void OnPointerUp(PointerEventData eventData)
    {
        if (_button.interactable) // Only animate if the button is interactable
        {
            if (_animationCoroutine != null) StopCoroutine(_animationCoroutine);
            _animationCoroutine = StartCoroutine(AnimateScale(transform.localScale, _originalScale * hoverScaleFactor, scaleDownDuration)); // Return to hover scale or original if not hovering
        }
    }

    // Generic coroutine for scaling animation
    private IEnumerator AnimateScale(Vector3 startScale, Vector3 targetScale, float duration)
    {
        float timer = 0f;
        while (timer < duration)
        {
            timer += Time.unscaledDeltaTime;
            transform.localScale = Vector3.Lerp(startScale, targetScale, timer / duration);
            yield return null;
        }
        transform.localScale = targetScale;
    }

    // Optional: Reset scale if the button becomes non-interactable or when disabled
    void OnDisable()
    {
        if (_animationCoroutine != null)
        {
            StopCoroutine(_animationCoroutine);
        }
        transform.localScale = _originalScale; // Reset scale when disabled
    }
}
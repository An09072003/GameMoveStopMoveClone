using UnityEngine;
using UnityEngine.EventSystems;

public class JoystickInputHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public bool isMoving { get; private set; } = false;

    public void OnPointerDown(PointerEventData eventData)
    {
        isMoving = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isMoving = false;
    }
}

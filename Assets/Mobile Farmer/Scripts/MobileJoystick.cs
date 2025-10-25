using UnityEngine;
using UnityEngine.Serialization;

public class MobileJoystick : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private RectTransform joystickOutline;
    [SerializeField] private RectTransform joystickKnob;

    [Header("Setting")] 
    [SerializeField] private float moveFactor;
    private Vector3 clickPosition;
    private Vector3 move;
    private bool canControl = false;
    private void Update()
    {
        if(canControl)
            ControlJoystick();
    }

    public void ClickedOnJoystockZoneCallback()
    {
        clickPosition = Input.mousePosition;
        joystickOutline.position = clickPosition;
        ShownJoystick();
    }

    void ShownJoystick()
    {
        joystickOutline.gameObject.SetActive(true);
        canControl = true;
    }

    void HideJoystick()
    {
        joystickOutline.gameObject.SetActive(false);
        canControl = false;
    }

    void ControlJoystick()
    {
        Vector3 currentPosition = Input.mousePosition;
        Vector3 direction = currentPosition - clickPosition;

        float moveMagnitude = direction.magnitude * moveFactor / Screen.width;

        moveMagnitude = Mathf.Min(moveMagnitude, joystickOutline.rect.width / 2);
        move = direction.normalized * moveMagnitude;
        Vector3 targetPosition = clickPosition + move;

        joystickKnob.position = targetPosition;
        
        if(Input.GetMouseButtonUp(0))
            HideJoystick();
    }

    public Vector3 GetMovementVector() => move;
}

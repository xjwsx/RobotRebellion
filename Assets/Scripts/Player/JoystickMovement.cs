using UnityEngine;
using UnityEngine.EventSystems;

public class JoystickMovement : Singleton<JoystickMovement>
{
    public GameObject bigStick;
    public GameObject smallStick;
    Vector3 stickFirstPosition;
    Vector3 joystickFirstPosition;
    public Vector3 joyVec;
    float stickRadius;

    public bool isPlayerMoving = false;

    private void Start()
    {
        stickRadius = bigStick.gameObject.GetComponent<RectTransform>().sizeDelta.y / 2;
        joystickFirstPosition = bigStick.transform.position;
    }

    public void PointDown()
    {
        bigStick.transform.position = Input.mousePosition;
        smallStick.transform.position = Input.mousePosition;
        stickFirstPosition = Input.mousePosition;
    }

    public void Drag(BaseEventData baseEventData)
    {
        PointerEventData pointerEventData = baseEventData as PointerEventData;
        Vector3 DragPosition = pointerEventData.position;
        joyVec = (DragPosition - stickFirstPosition).normalized;
        float stickDistance = Vector3.Distance(DragPosition, stickFirstPosition);
        isPlayerMoving = true;

        if (stickDistance < stickRadius)
        {
            smallStick.transform.position = stickFirstPosition + joyVec * stickDistance;
        }
        else
        {
            smallStick.transform.position = stickFirstPosition + joyVec * stickRadius;
        }
    }

    public void Drop()
    {
        joyVec = Vector3.zero;
        GameManager.instance.playerController.Rigidbody.velocity = new Vector3(0, GameManager.instance.playerController.Rigidbody.velocity.y, 0);
        bigStick.transform.position = joystickFirstPosition;
        smallStick.transform.position = joystickFirstPosition;
        isPlayerMoving = false;
    }
}

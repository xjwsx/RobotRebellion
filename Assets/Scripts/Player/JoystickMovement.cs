using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class JoystickMovement : MonoBehaviour
{
    public static JoystickMovement instance;
    public GameObject bigStick;
    public GameObject smallStick;
    Vector3 stickFirstPosition;
    public Vector3 joyVec;
    Vector3 joystickFirstPosition;
    float stickRadius;

    public bool isPlayerMoving = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }


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
        PlayerTargeting.instance.getATarget = false;
    }

    public void Drag(BaseEventData baseEventData)
    {
        PointerEventData pointerEventData = baseEventData as PointerEventData;
        Vector3 DragPosition = pointerEventData.position;
        joyVec = (DragPosition - stickFirstPosition).normalized;
        float stickDistance = Vector3.Distance(DragPosition, stickFirstPosition);
        PlayerController.instance.animator.SetBool("Shot", false);
        PlayerController.instance.animator.SetBool("Run", true);
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
        bigStick.transform.position = joystickFirstPosition;
        smallStick.transform.position = joystickFirstPosition;
        PlayerController.instance.animator.SetBool("Run", false);
        isPlayerMoving = false;
    }
}

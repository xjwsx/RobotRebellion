using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public static CameraMovement instance;
    public GameObject player;

    public float offsetY = 45f;
    public float offsetZ = -40f;

    Vector3 cameraPosition;
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

    private void LateUpdate()
    {
        cameraPosition.y = player.transform.position.y + offsetY;
        cameraPosition.z = player.transform.position.z + offsetZ;

        transform.position = cameraPosition;
    }
    public void MoveXPosition()
    {
        cameraPosition.x = player.transform.position.x;
    }
}

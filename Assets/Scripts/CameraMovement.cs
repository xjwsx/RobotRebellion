using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public GameObject player;

    public float offsetY = 45f;
    public float offsetZ = -40f;

    Vector3 camaraPosition;

    private void LateUpdate()
    {
        camaraPosition.y = player.transform.position.y + offsetY;
        camaraPosition.z = player.transform.position.z + offsetZ;

        transform.position = camaraPosition;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraMovement : MonoBehaviour
{
    public static CameraMovement instance;
    public GameObject player;
    public Image fadeInOutImg;

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
        StartCoroutine(nameof(FadeInOut));
        cameraPosition.x = player.transform.position.x;
    }
    IEnumerator FadeInOut()
    {
        float a = 1;
        fadeInOutImg.color = new Vector4(1, 1, 1, a);
        yield return new WaitForSeconds(0.3f);

        while(a >= 0)
        {
            fadeInOutImg.color = new Vector4(1, 1, 1, a);
            a -= 0.02f;
            yield return null;
        }

    }
}

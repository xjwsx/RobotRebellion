using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CameraMovement : Singleton<CameraMovement>
{
    public GameObject player;

    public float offsetY = 45f;
    public float offsetZ = -40f;

    Vector3 cameraPosition;

    private void LateUpdate()
    {
        cameraPosition.y = player.transform.position.y + offsetY;
        cameraPosition.z = player.transform.position.z + offsetZ;

        transform.position = cameraPosition;
    }
    public void MoveXPosition()
    {
        StartCoroutine(nameof(ActivateFadeInOutUI));
        cameraPosition.x = player.transform.position.x;
    }
    IEnumerator ActivateFadeInOutUI()
    {
        var menuTask = UIManager.instance.GetUI<FadeInOut>();
        while (!menuTask.IsCompleted)
        {
            yield return null;
        }
    }
}

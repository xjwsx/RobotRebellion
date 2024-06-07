using UnityEngine;
using UnityEngine.UI;

public class PauseUI : MonoBehaviour
{
    public GameObject[] buttons;
    public Image[] images;
    public int buttonIndex = -1;

    public void ChangeImage(Image image)
    {
        if(buttonIndex == -1)
        {
            buttonIndex = 0;
        }
        for(int i = 0; i < images.Length; i++)
        {
            if (images[i].sprite != null)
            {
                buttonIndex = i + 1;
            }
        }
        images[buttonIndex].sprite = image.sprite;
    }
    public void ActiveButtons()
    {
        if(buttonIndex == -1)
        {
            return;
        }
        else
        {
            for(int i = 0; i <= buttonIndex; i++)
            {
                buttons[i].SetActive(true);
            }
        }
    }

}

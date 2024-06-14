using UnityEngine;
using UnityEngine.UI;

public class Pause : MonoBehaviour
{
    [SerializeField] private Button playButton;
    public GameObject[] buttons;
    public Image[] images;
    public int buttonIndex = -1;
    private void OnEnable()
    {
        ActiveButtons();
    }
    private void Start()
    {
        playButton.onClick.AddListener(PlayButton);
    }
    public void PlayButton()
    {
        //AudioManager.instance.IsButtonClick = true;
        Time.timeScale = 1f;
        gameObject.SetActive(false);
        JoystickMovement.instance.gameObject.SetActive(true);
    }
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

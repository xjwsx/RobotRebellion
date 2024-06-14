using UnityEngine;
using UnityEngine.UI;

public class Option : MonoBehaviour
{
    [SerializeField] private Button closeButton;
    [SerializeField] private Slider musicSlider;

    private void Start()
    {
        closeButton.onClick.AddListener(CloseButton);
        if (musicSlider != null)
        {
            musicSlider.onValueChanged.AddListener(delegate { SetBackgroundMusicVolume(); });
            musicSlider.value = AudioManager.instance.BGM.volume;
        }
    }

    public void CloseButton()
    {
        UIManager.instance.CloseUI();
    }
    private void SetBackgroundMusicVolume()
    {
        if (AudioManager.instance.BGM != null && musicSlider != null)
        {
            AudioManager.instance.BGM.volume = musicSlider.value;
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public enum SkillType
{
    BounceAttack,
    PlusBullet,
    DoubleBullet,
    TripleBullet,
    BoundWallBullet,
    UpgradeBulletSpeed,
    UpgradeBulletDamage,
    BackAttack,
    SideAttack
}

public class SlotMachine : MonoBehaviour
{
    public RectTransform skillPanel;
    public GameObject SlotSkillObject;
    public Button Slot;
    public Image DisplayResultImage;

    public List<Sprite> SkillSprite = new List<Sprite>();
    public List<Image> SlotSprite = new();

    private List<Sprite> shuffledSprites = new List<Sprite>();
    private void OnEnable()
    {
        InitializeSlots();
    }
    void InitializeSlots()
    {
        shuffledSprites = new List<Sprite>(SkillSprite);
        Shuffle(shuffledSprites);

        for (int i = 0; i < SlotSprite.Count; i++)
        {
            SlotSprite[i].sprite = shuffledSprites[i];
        }
    }
    IEnumerator StartSlot()
    {
        for (int i = 0; i < 40; i++)
        {
            SlotSkillObject.transform.localPosition -= new Vector3(0, 50f, 0);
            if (SlotSkillObject.transform.localPosition.y < 50f)
            {
                SlotSkillObject.transform.localPosition += new Vector3(0, 800f, 0);
            }
            yield return new WaitForSeconds(0.02f);
        }
        SelectStoppingImage();
    }
    void SelectStoppingImage()
    {
        float panelCenterY = skillPanel.position.y;

        Image selectedImage = null;
        float minDistance = float.MaxValue;

        foreach (Image img in SlotSprite)
        {
            RectTransform imgRect = img.rectTransform;
            float distance = Mathf.Abs(imgRect.position.y - panelCenterY);

            if (distance < minDistance)
            {
                selectedImage = img;
                minDistance = distance;
            }
        }
        if (selectedImage != null)
        {
            DisplayResultImage.sprite = selectedImage.sprite;
            SkillType selectedSkill = GetSkillFromIndex(SkillSprite.IndexOf(selectedImage.sprite));
            GameManager.instance.playerController.AddSkill(selectedSkill);
        }

        StartCoroutine(nameof(OffSlotMachineUI));
    }
    private void Shuffle<T>(List<T> list)
    {
        System.Random rng = new System.Random();
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
    IEnumerator OffSlotMachineUI()
    {
        GameManager.instance.PauseUI.ChangeImage(DisplayResultImage);

        yield return new WaitForSeconds(2f);
        GameManager.instance.joystickCanvasUI.SetActive(true);
        gameObject.SetActive(false);
        DisplayResultImage.enabled = false;
    }
    public void StartButton()
    {
        Time.timeScale = 1f;
        StartCoroutine(StartSlot());
    }
    private SkillType GetSkillFromIndex(int index)
    {
        return (SkillType)index;
    }
}

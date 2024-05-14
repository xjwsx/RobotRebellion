using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SlotMachine : MonoBehaviour
{
    public GameObject[] SlotSkillObject;
    public Button[] Slot;

    public Sprite[] SkillSprite;

    [System.Serializable]
    public class DisplayItemSlot
    {
        public List<Image> SlotSprite = new();
    }
    public DisplayItemSlot[] DisplayItemSlots;

    public Image DisplayResultImage;

    public List<int> StartList = new();
    public List<int> ResultIndexList = new();
    private int ItemCnt = 3;
    private readonly int[] answer = { 2, 3, 1 };

    void InitializeSlots()
    {
        for (int i = 0; i < ItemCnt * Slot.Length; i++)
        {
            StartList.Add(i);
        }
        for (int i = 0; i < Slot.Length; i++)
        {
            InitializeSlot(i);
        }
    }
    public void InitializeSlot(int slotIndex)
    {
        for (int j = 0; j < ItemCnt; j++)
        {
            Slot[slotIndex].interactable = false;
            int randomIndex = Random.Range(0, StartList.Count);
            if (IsResultIndex(slotIndex, j))
            {
                ResultIndexList.Add(StartList[randomIndex]);
            }
            DisplayItemSlots[slotIndex].SlotSprite[j].sprite = SkillSprite[StartList[randomIndex]];

            if (j == 0)
            {
                DisplayItemSlots[slotIndex].SlotSprite[ItemCnt].sprite = SkillSprite[StartList[randomIndex]];
            }
            StartList.RemoveAt(randomIndex);
        }
    }
    private bool IsResultIndex(int slotIndex, int position)
    {
        return (slotIndex == 0 && position == 1) ||
            (slotIndex == 1 && position == 0) ||
            (slotIndex == 2 && position == 2);
    }
    private void StartSlotAnimations()
    {
        for (int i = 0; i < Slot.Length; i++)
        {
            StartCoroutine(StartSlot(i));
        }
    }
    IEnumerator StartSlot(int SlotIndex)
    {
        for (int i = 0; i < (ItemCnt * (6 + SlotIndex * 4) + answer[SlotIndex]) * 2; i++)
        {
            SlotSkillObject[SlotIndex].transform.localPosition -= new Vector3(0, 50f, 0);
            if (SlotSkillObject[SlotIndex].transform.localPosition.y < 50f)
            {
                SlotSkillObject[SlotIndex].transform.localPosition += new Vector3(0, 300f, 0);
            }
            yield return new WaitForSeconds(0.02f);
        }
        for (int i = 0; i < ItemCnt; i++)
        {
            Slot[i].interactable = true;
        }
    }

    public void ClickBtn(int index)
    {
        DisplayResultImage.sprite = SkillSprite[ResultIndexList[index]];
        DisplayResultImage.enabled = true;
    }
    public void StartButton()
    {
        InitializeSlots();
        StartSlotAnimations();
    }
}

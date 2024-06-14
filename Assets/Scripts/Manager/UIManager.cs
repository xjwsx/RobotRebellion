using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class UIManager : Singleton<UIManager>
{
    private Dictionary<string, GameObject> uiDictionary = new();
    private Stack<GameObject> uiStack = new Stack<GameObject>();

    public async Task<T> GetUI<T>() where T : MonoBehaviour
    {
        string uiName = typeof(T).Name;

        if (uiDictionary.TryGetValue(uiName, out GameObject existingUI) && existingUI != null)
        {
            existingUI.SetActive(true);
            PushToStack(existingUI);
            return existingUI.GetComponent<T>();
        }
        else
        {
            return await CreateUI<T>();
        }
    }
    public async Task<T> CreateUI<T>() where T : MonoBehaviour
    {
        string uiName = typeof(T).Name;
        //GameObject prefab = Resources.Load<GameObject>("Prefabs/UI/" + uiName);
        AsyncOperationHandle<GameObject> handle = Addressables.LoadAssetAsync<GameObject>(uiName);

        await handle.Task;
        if(handle.Status == AsyncOperationStatus.Succeeded)
        {
            GameObject prefab = handle.Result;
            GameObject newUI = Instantiate(prefab);
            T comp = newUI.GetComponent<T>() ?? newUI.AddComponent<T>();

            uiDictionary[uiName] = newUI;
            PushToStack(newUI);
            return comp;
        }
        else
        {
            return null;
        }
    }

    private void PushToStack(GameObject ui)
    {
        if(uiStack.Contains(ui))
        {
            Stack<GameObject> tempStack = new Stack<GameObject>();
            while(uiStack.Count > 0 && uiStack.Peek() != null)
            {
                tempStack.Push(uiStack.Pop());
            }
            uiStack.Pop();
            while(tempStack.Count > 0)
            {
                uiStack.Push(tempStack.Pop());
            }
        }
        uiStack.Push(ui);
        UpdateCanvasSorting();
    }
    private void UpdateCanvasSorting()
    {
        int count = uiStack.Count;
        foreach (GameObject ui in uiStack)
        {
            Canvas canvas = ui.GetComponent<Canvas>();
            if (canvas != null)
            {
                canvas.sortingOrder = count--;
            }
        }
    }
    public void CloseUI()
    {
        if(uiStack.Count > 0)
        {
            GameObject topUI = uiStack.Pop();
            topUI.SetActive(false);
        }
    }
}

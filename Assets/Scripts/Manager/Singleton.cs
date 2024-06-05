using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T instance;

    public static T Instance
    {
        get
        {
            if(instance == null)
            {
                instance = FindObjectOfType<T>();
                if(instance == null)
                {
                    GameObject singletonObject = new GameObject(typeof(T).Name);
                    instance = singletonObject.AddComponent<T>();
                }
                DontDestroyOnLoad(instance.gameObject);
            }
            return instance;
        }
    }

    protected virtual void Awake()
    {
        if(instance == null)
        {
            instance = this as T;
            DontDestroyOnLoad (this.gameObject);
        }
        else if(instance != this)
        {
            Destroy(gameObject);
        }
    }
}

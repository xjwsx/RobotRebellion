using System.Collections;
using UnityEngine;

public class GameManager : Singleton<GameManager>   
{
    public Pause PauseUI;
    public PlayerController playerController;
    public EnemyController enemyController;
    protected override void Awake()
    {
        base.Awake();
        ObjectPoolManager.instance.InitializePool();
        StartCoroutine(nameof(ActivateMainUI));
    }
    IEnumerator ActivateMainUI()
    {
        var menuTask = UIManager.instance.GetUI<Main>();
        while (!menuTask.IsCompleted)
        {
            yield return null;
        }
    }
}

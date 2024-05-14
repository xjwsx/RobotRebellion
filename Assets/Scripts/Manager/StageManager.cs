using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class StageManager : MonoBehaviour
{
    public static StageManager instance;

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
    [Serializable]
    public class StageData
    {
        public List<Transform> startPosition;
    }
    public List<StageData> normalStages;
    public List<Transform> angelPosition;
    public List<Transform> bossPosition;
    public Transform lastBossPosition;

    public int currentStage = 0;
    private int lastStage = 20;

    public void NextStage()
    {
        if (++currentStage > lastStage)
            return;

        Transform newPosition = GetStartPositionStage(currentStage);
        if (newPosition != null)
        {
            GameManager.instance.playerController.Rigidbody.transform.position = newPosition.position;
            CameraMovement.instance.MoveXPosition();
        }
    }

    private Transform GetStartPositionStage(int stage)
    {
        if (currentStage == lastStage)
            return lastBossPosition;
        if (stage % 10 == 0)
            return GetRandomPosition(bossPosition);
        else if (stage % 5 == 0)
            return GetRandomPosition(angelPosition);
        else
        {
            int stageIndex = (stage - 1) / 10;
            if (stageIndex < normalStages.Count)
                return GetRandomPosition(normalStages[stageIndex].startPosition);
        }

        return null;
    }

    private Transform GetRandomPosition(List<Transform> positions)
    {
        if(positions.Count > 0)
        {
            int index = Random.Range(0, positions.Count);
            return positions[index];
        }
        else { return null; }
    }

}
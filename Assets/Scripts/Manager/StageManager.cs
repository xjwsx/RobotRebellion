using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class StageManager : Singleton<StageManager>
{
    [Serializable]
    public class StageData
    {
        public List<Transform> startPosition;
    }
    public List<StageData> normalStages;
    public List<Transform> angelPosition;
    public Transform bossPosition;
    public Transform lastBossPosition;

    public int currentStage = 0;
    private readonly int lastStage = 20;

    private HashSet<Transform> visitedPositions = new HashSet<Transform> ();

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
        {
            return lastBossPosition;
        }
        if (stage % 10 == 0)
        {
            return bossPosition;
        }
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
        if (positions.Count == 0) return null;

        List<Transform> unvisitedPositions = new List<Transform>();
        foreach (var position in positions)
        {
            if (!visitedPositions.Contains(position))
            {
                unvisitedPositions.Add(position);
            }
        }

        if (unvisitedPositions.Count > 0)
        {
            int randomIndex = Random.Range(0, unvisitedPositions.Count);
            Transform selectedPosition = unvisitedPositions[randomIndex];
            visitedPositions.Add(selectedPosition);
            return selectedPosition;
        }
        else
        {
            visitedPositions.Clear();
            int randomIndex = Random.Range(0, positions.Count);
            Transform selectedPosition = positions[randomIndex];
            visitedPositions.Add(selectedPosition);
            return selectedPosition;
        }
    }
}
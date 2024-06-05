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
    public List<GameObject> openDoor;
    public List<GameObject> closeDoor;
    public List<StageData> normalStages;
    public List<Transform> angelPosition;
    public Transform bossPosition;
    public Transform lastBossPosition;

    public int currentStage = 0;
    private readonly int lastStage = 20;
    private int currentRoomIndex = -1;

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
            currentRoomIndex = openDoor.Count - 1;
            return lastBossPosition;
        }
        if (stage % 10 == 0)
        {
            currentRoomIndex = openDoor.Count - 2;
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
            currentRoomIndex = positions.IndexOf(selectedPosition);
            return selectedPosition;
        }
        else
        {
            // All positions have been visited; reset and select randomly
            visitedPositions.Clear();
            int randomIndex = Random.Range(0, positions.Count);
            Transform selectedPosition = positions[randomIndex];
            visitedPositions.Add(selectedPosition);
            currentRoomIndex = positions.IndexOf(selectedPosition);
            return selectedPosition;
        }
        //if (positions.Count > 0)
        //{
        //    int index = Random.Range(0, positions.Count);
        //    if(!roomCount.Contains(index))
        //    {
        //        roomCount.Add(index);
        //        currentRoomIndex = index;
        //        return positions[index];
        //    }
        //    else
        //    {
        //        int reindex = Random.Range(0, positions.Count);
        //        return positions[reindex];
        //    }
        //}
        //else { return null; }
    }
    public void OpenDoor()
    {
        closeDoor[currentRoomIndex].SetActive(false);
        openDoor[currentRoomIndex].SetActive(true);
    }

}
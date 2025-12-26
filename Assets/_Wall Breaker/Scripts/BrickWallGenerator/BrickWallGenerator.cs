using System;
using System.Collections.Generic;
using UnityEngine;

public class BrickWallGenerator : MonoBehaviour
{
    [SerializeField] private GameObject brickPrefab;
    [SerializeField] private int rows = 8;
    [SerializeField] private int columns = 10;

    [SerializeField] private float brickWidth = 2f;
    [SerializeField] private float brickHeight = 1f;
    [SerializeField] private float space = 0.05f;

    public List<BricksHandler> bricksRecord = new List<BricksHandler>();

    private GameObject currentWall;

    public static Action OnLevelComplete;


    private void OnEnable()
    {
        BricksHandler.OnDetached += RemoveDetachedBricks;
    }

    private void OnDisable()
    {
        BricksHandler.OnDetached -= RemoveDetachedBricks;
    }


    public void GenerateWall()
    {
        // Step 1 Calculate the Offset of the Wall
        float wallWidth = columns * (brickWidth + space);
        float offsetX = -wallWidth / 2f + brickWidth / 2f;

        if (currentWall != null)
        {
            Destroy(currentWall);
            bricksRecord.Clear();
        }

        // Step 2 Create a parent object
        currentWall = new GameObject("WallParent");
        currentWall.transform.position = transform.position;

        // Step 3 Nested Loop to Instantiate Bricks
        for (int row = 0; row < rows; row++)  // Outer Loop for Rows
        {
            for (int col = 0; col < columns; col++) // Inner Loop for Columns
            {
                SpawnBrick(row, col, offsetX, currentWall.transform);
            }
        }

    }

    private void SpawnBrick(int row, int col, float offset, Transform parent)
    {
        // Step 4 Calculate Brick Position
        float x = col * (brickWidth + space) + offset;
        float y = row * (brickHeight + space);
        float z = transform.position.z;

        Vector3 brickPos = new Vector3(x, y, z);

        // Step 5 Instantiate Brick
        GameObject brick = Instantiate(
            brickPrefab,
            brickPos,
            Quaternion.identity,
            parent
            );

        Renderer rend = brick.GetComponent<Renderer>();
        if (rend != null)
        {
            Color brickColor = new Color(
                UnityEngine.Random.Range(0.2f, 1f),
                UnityEngine.Random.Range(0.2f, 1f),
                UnityEngine.Random.Range(0.2f, 1f)
                );
            rend.material.color = brickColor;
        }

        brick.name = $"Brick_R{row}_C{col}";

        BricksHandler brickHandler = brick.GetComponent<BricksHandler>();

        if (brickHandler != null)
        {
            bricksRecord.Add(brickHandler);
        }
    }

    private int GetBrickRecord()
    {
        return bricksRecord.Count;
    }

    private void RemoveDetachedBricks(BricksHandler bricksHandler)
    {
        string detachedBrickName = bricksHandler.gameObject.name;

        //Debug.Log("Removing Detached Brick: " + detachedBrickName);

        // Iterate through the bricksRecord to find and remove the detached brick
        for (int i = 0; i < bricksRecord.Count; i++)
        {
            if (bricksRecord[i].gameObject.name == detachedBrickName)
            {
                bricksRecord.RemoveAt(i);
                break;
            }
        }

        CheckForLevelComplete();
    }

    private void CheckForLevelComplete()
    {
        if (GetBrickRecord() == 0)
        {
            if (columns < 12 && rows < 8)
            {
                columns += 2;
                rows += 2;
            }
            OnLevelComplete?.Invoke();
        }
    }

    public void DestroyCurrentWall()
    {
        if (currentWall != null)
        {
            Destroy(currentWall);
            bricksRecord.Clear();
        }
    }


    private void OnDrawGizmos()
    {
        float width = columns * (brickWidth + space);
        float height = rows * (brickHeight + space);
        Gizmos.color = Color.yellow;

        Vector3 center = transform.position + new Vector3(0, height / 2, 0);
        Vector3 size = new Vector3(width, height, 1);

        Gizmos.DrawWireCube(center, size);
    }

}


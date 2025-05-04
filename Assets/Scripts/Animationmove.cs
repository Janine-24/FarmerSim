using UnityEngine;
using System.Collections.Generic;


public class Animationmove : MonoBehaviour
{
    public List<GameObject> tileList = new();
    public int gridWidth; // Number of tiles per row
    public float loadDistance = 20f;
    private Transform cameraTransform;

    private GameObject[,] landTiles;

    public int YourGridWidth { get; private set; }
    public float LoadDistance { get; private set; }

    void Start()
    {
        cameraTransform = Camera.main.transform;

        int gridWidth = YourGridWidth; //  17 from the inspector
        int gridHeight = Mathf.CeilToInt((float)tileList.Count / gridWidth);
        landTiles = new GameObject[gridWidth, gridHeight];

        for (int i = 0; i < tileList.Count; i++)
        {
            if (tileList[i] == null)
            {
                Debug.LogWarning($"Null GameObject in tileList at index {i}");
                continue;
            }

            int x = i % gridWidth;
            int y = i / gridWidth;
            landTiles[x, y] = tileList[i];
        }

    }
    void Update()
    {
        Vector2 camPos = cameraTransform.position;

        {

             for (int x = 0; x < landTiles.GetLength(0); x++)
            {
                for (int y = 0; y < landTiles.GetLength(1); y++)
                {
                    GameObject tile = landTiles[x, y];
                    if (tile == null)
                    {
                        Debug.LogWarning($"Null tile at ({x}, {y})");
                        continue;
                    }

                    float distance = Vector2.Distance(camPos, tile.transform.position);
                    bool shouldShowCloud = distance < LoadDistance;

                    Transform cloud = tile.transform.Find("Cloud");
                    if (cloud != null)
                    {
                        cloud.gameObject.SetActive(!shouldShowCloud);
                    }
                }
            }
        }
    }

}


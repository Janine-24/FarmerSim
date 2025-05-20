using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileManager : MonoBehaviour
{
    [SerializeField] private Tilemap interactableMap;

    [SerializeField] private Tile hiddenInteractableTile;
    [SerializeField] private Sprite visualCueForPlant;
    [SerializeField] private Tile interactedTile;

    private Dictionary<Vector3Int, GameObject> activeCues = new Dictionary<Vector3Int, GameObject>();


    public string GetTileName(Vector3Int position)
    {
        if (interactableMap != null)
        {
            TileBase tile = interactableMap.GetTile(position);

            if (tile != null)
            {
                return tile.name;
            }
        }

        return "";
    }

    void Start()
    {
        foreach (var position in interactableMap.cellBounds.allPositionsWithin)
        {
            interactableMap.SetTile(position, hiddenInteractableTile);
        }
    }

    public bool NoPlantOnTile(Vector3Int position)
    {
        return !activeCues.ContainsKey(position);
    }

    public bool IsInteractable(Vector3Int position)
    {
        TileBase tile = interactableMap.GetTile(position);

        if (tile != null)
        {
            if (tile.name == "Interactable")
            {
                return true;
            }
        }
        return false;
    }

    public void ReverseInteracted(Vector3Int position)
    {
        // If there's a cue on this tile, do nothing
        if (activeCues.ContainsKey(position))
        {
            Debug.Log("Cannot reverse tile: visual cue is active.");
            return;
        }

        interactableMap.SetTile(position, hiddenInteractableTile);
    }

    public void SetInteracted(Vector3Int position)
    {
        interactableMap.SetTile(position, interactedTile);
    }

    public bool IsPlantable(Vector3Int position)
    {
        TileBase tile = interactableMap.GetTile(position);

        if (tile != null)
        {
            if (tile.name == "soil")
            {
                return true;
            }
        }
        return false;
    }

    public bool ShowPlantingCue(Vector3Int tilePos)
    {
        if (activeCues.ContainsKey(tilePos))
            return false;

        Vector3 worldPos = interactableMap.GetCellCenterWorld(tilePos);

        GameObject cue = new GameObject("PlantingCue");
        SpriteRenderer sr = cue.AddComponent<SpriteRenderer>();
        sr.sprite = visualCueForPlant;
        sr.sortingOrder = 9; // Make sure it appears above tiles

        cue.transform.position = worldPos;

        activeCues[tilePos] = cue;

        return true;
    }
}
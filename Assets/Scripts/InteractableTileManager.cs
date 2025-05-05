using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class InteractableTileManager : MonoBehaviour
{
    [SerializeField] private Tilemap interactableMap;

    [SerializeField] private Tile hiddenInteractableTile;
    [SerializeField] private Sprite visualCueForPlant;
    [SerializeField] private Tile interactedTile;
   void Start()
    {
        foreach(var position in interactableMap.cellBounds.allPositionsWithin)
        {
            interactableMap.SetTile(position, hiddenInteractableTile);
        }
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

<<<<<<< HEAD
    public bool ShowPlantingCue(Vector3Int tilePos)
=======
    public void ShowPlantingCue(Vector3Int tilePos)
>>>>>>> f598a507318380309e9bf7a9f70fd9940e01adf6
    {
        Vector3 worldPos = interactableMap.CellToWorld(tilePos) + interactableMap.tileAnchor;

        GameObject cue = new GameObject("PlantingCue");
        SpriteRenderer sr = cue.AddComponent<SpriteRenderer>();
        sr.sprite = visualCueForPlant;
<<<<<<< HEAD
        sr.sortingOrder = 9; // Make sure it appears above tiles

        cue.transform.position = worldPos;

        return true;
=======
        sr.sortingOrder = 4; // Make sure it appears above tiles

        cue.transform.position = worldPos;
>>>>>>> f598a507318380309e9bf7a9f70fd9940e01adf6
    }
}

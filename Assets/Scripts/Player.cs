using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] public int numCarrotSeed = 0;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Vector3Int position = new Vector3Int((int)transform.position.x, (int)transform.position.y, 0);

            if (GameManager.instance.tileManager.IsInteractable(position)) //tile is soil
            {
                Debug.Log("Tile is interactable");
                GameManager.instance.tileManager.SetInteracted(position);
            }
            else if (GameManager.instance.tileManager.IsInteractable(position) == false && GameManager.instance.tileManager.ShowPlantingCue(position) == false)
            {
                Debug.Log("Tile is not interactable and no planting cue shown");
            }
            else if (!GameManager.instance.tileManager.NoPlantOnTile(position))
            {
                Debug.Log("Tile is already planted on");
            }
            else if (GameManager.instance.tileManager.NoPlantOnTile(position) == true && !GameManager.instance.tileManager.IsInteractable(position))
            {
                Debug.Log("Tile is not interactable");
                {
                    GameManager.instance.tileManager.ReverseInteracted(position);
                }
            }
        } else if (Input.GetKeyDown(KeyCode.P))
        {
             Debug.Log("P is clicked");
             Vector3Int positionPlant = new Vector3Int((int)transform.position.x, (int)transform.position.y, 0);

             if (GameManager.instance.tileManager.IsPlantable(positionPlant))
             {
                Debug.Log("Tile is plantable");
                GameManager.instance.tileManager.ShowPlantingCue(positionPlant);
             }
             else
             {
                Debug.Log("Tile is not plantable");
             }
        }
        
    }
}

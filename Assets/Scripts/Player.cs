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

            if (GameManager.instance.tileManager.IsInteractable(position))
            {
                Debug.Log("Tile is interactable");
                GameManager.instance.tileManager.SetInteracted(position);
            }
            else
            {
                GameManager.instance.tileManager.ReverseInteracted( position );
            }
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            Vector3Int position = new Vector3Int((int)transform.position.x, (int)transform.position.y, 0);
            
            if (GameManager.instance.tileManager.IsPlantable(position))
            {
                Debug.Log("Tile is plantable");
                GameManager.instance.tileManager.ShowPlantingCue(position);
            }
            else
            {
                Debug.Log("Tile is not plantable");
            }
        }
    }
}

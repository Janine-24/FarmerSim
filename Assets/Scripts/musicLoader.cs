using UnityEngine;

public class MusicLoader : MonoBehaviour
{
    private static MusicLoader instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); //prevent the music from being destroyed when changing scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
//this script is to prevent the music not repeat when cahnge scene, we use this script to load the music in the first scene and keep it playing
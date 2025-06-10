using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public ItemManager itemManager;
    public TileManager tileManager;
    public UI_Manager uiManager;
    public Player player;

    private void Awake()
    {
        instance = this;
        
        itemManager = GetComponent<ItemManager>();
        tileManager = GetComponent<TileManager>();
        uiManager = GetComponent<UI_Manager>();
    }

    private void Start()
    {
        player = FindFirstObjectByType<Player>();
        WorldSaveManager.LoadWorld();

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log("🧪 Manual save triggered with Q key");
            WorldSaveManager.SaveWorld();
        }
    }


    private void OnApplicationQuit()
    {
        Debug.Log("🧪 OnApplicationQuit called");
        WorldSaveManager.SaveWorld();
    }

    private void OnApplicationPause(bool pause)
    {
        Debug.Log("🧪 OnApplicationPause called");
        if (pause) WorldSaveManager.SaveWorld();
    }

    private void OnDisable()
    {
        Debug.Log("🧪 OnDisable called");
        WorldSaveManager.SaveWorld();
    }

}

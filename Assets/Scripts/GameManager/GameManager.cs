using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public ItemManager itemManager;
    public TileManager tileManager;
    public UI_Manager uiManager;
    public Player player;

    void Start()
    {
        if (GameManager.instance == null)
        {
            GameObject go = Instantiate(Resources.Load<GameObject>("Managers/GameManager"));
        }
    }

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject); // 避免重复
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject); // 保持跨场景

        itemManager = GetComponent<ItemManager>();
        tileManager = GetComponent<TileManager>();
        uiManager = GetComponent<UI_Manager>();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 每次新场景加载，重新找到场景中的 player
        player = FindFirstObjectByType<Player>();

        if (player == null)
        {
            Debug.LogWarning("Player not found in scene!");
        }

        // 这些 Manager 是持久化的，不需要重新找
    }
}

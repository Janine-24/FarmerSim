using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


// The singleton script that manage game state
public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance;
    // save data
    public MapData mapData = new();
    // prefab mapping for easy instantiation during loading
    [Header("Prefabs")]
    public List<AnimalPrefabEntry> animalPrefabs = new();
    public List<HabitatPrefabEntry> habitatPrefabs = new();
    public List<ProductPrefabEntry> productPrefabs = new();

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        Debug.Log($"Loaded {animalPrefabs.Count} animal prefabs.");
        Debug.Log($"Loaded {habitatPrefabs.Count} habitat prefabs.");
        SceneManager.sceneLoaded += (scene, mode) => StartCoroutine(DelayedLoadMapState());

    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode _)
    {
        if (scene.name == "main page") 
        {
            LoadMapState();
        }
    }

    private IEnumerator DelayedLoadMapState()
    {
        yield return null; // wait to ensure all objects already initialized
        LoadMapState();
    }

    // use to store animals types and prefabs
    [System.Serializable]
    public class AnimalPrefabEntry
    {
        public string animalType;
        public GameObject prefab;
    }

    // use to store habitat types and prefabs
    [System.Serializable]
    public class HabitatPrefabEntry
    {
        public string habitatType;
        public GameObject prefab;
    }

    [System.Serializable]
    public class ProductPrefabEntry
    {
        public string productType;
        public GameObject prefab;
    }

    // get prefab according to the aniamls tpyes
    public GameObject GetAnimalPrefab(string animalType)
    {
        foreach (var entry in animalPrefabs)
            if (entry.animalType == animalType)
                return entry.prefab;
        Debug.LogWarning($"Animal prefab not found for type: {animalType}");
        return null;
    }

    // get prefab from each habitat
    public GameObject GetHabitatPrefab(string habitatType)
    {
        foreach (var entry in habitatPrefabs)
            if (entry.habitatType == habitatType)
                return entry.prefab;
        Debug.LogWarning($"Habitat prefab not found for type: {habitatType}");
        return null;
    }

    public GameObject GetProductPrefab(string productType)
    {
        foreach (var entry in productPrefabs)
            if (entry.productType == productType)
                return entry.prefab;
        Debug.LogWarning($"Product prefab not found for type: {productType}");
        return null;
    }

    // save curretn status to mapData.cs
    public void SaveMapState()
    {
        mapData = new MapData();

        // save animals
        foreach (var animal in Object.FindObjectsByType<AnimalFood>(FindObjectsSortMode.None))
        {
            mapData.animals.Add(animal.ExportData());
        }

        //save habitat
        foreach (var habitat in Object.FindObjectsByType<Habitat>(FindObjectsSortMode.None))
        {
            mapData.habitats.Add(new HabitatData
            {
                habitatType = habitat.habitatType,
                position = (Vector2)habitat.transform.position
            });
        }

        // save cloud 
        foreach (var cloud in Object.FindObjectsByType<CloudController>(FindObjectsSortMode.None))
        {
            mapData.clouds.Add(new CloudData
            {
                position = (Vector2)cloud.transform.position,
                isUnlocked = cloud.isUnlocked
            }
            );
        }

        foreach (var product in Object.FindObjectsByType<Collectproduct>(FindObjectsSortMode.None))
        {
            mapData.products.Add(new ProductData
            {
                position = product.transform.position,
                productType = product.productType

            });
        }
        //save player level
        mapData.playerLevel = LevelSystem.Instance.level;

        // covert to JSON
        string json = JsonUtility.ToJson(mapData); //convert the data to txt.
        PlayerPrefs.SetString("Save_MapData", json); //unity built storage method, this method will write data to device
        PlayerPrefs.Save();

        Debug.Log("Map state saved.");
    }

    // load status from file and produce object
    public void LoadMapState()
    {
        if (string.IsNullOrEmpty(PlayerPrefs.GetString("Save_MapData")))
        {
            Debug.Log("No saved map data found.");
            return;
        }

        string json = PlayerPrefs.GetString("Save_MapData", "{}");
        mapData = JsonUtility.FromJson<MapData>(json);

        //clear old data, prevet duplicate
        ClearCurrentMapObjects();

        // restore habitat
        foreach (var habitatData in mapData.habitats)
        {
            var prefab = GetHabitatPrefab(habitatData.habitatType);
            if (prefab != null)
            {
                Instantiate(prefab, habitatData.position, Quaternion.identity);
            }
        }

        // restore animals and their status
        foreach (var data in mapData.animals)
        {
            GameObject prefab = GetAnimalPrefab(data.animalType);
            if (prefab != null)
            {
                GameObject instance = Instantiate(prefab, data.position, Quaternion.identity);
                if (instance.TryGetComponent<AnimalFood>(out var food))
                {
                    food.LoadFromData(data);
                }


            }
        }
        // restore cloud satus
        foreach (var cloudData in mapData.clouds)
        {
            var clouds = Object.FindObjectsByType<CloudController>(FindObjectsSortMode.None);
            foreach (var cloud in clouds)
            {
                if ((Vector2)cloud.transform.position == cloudData.position)
                {
                    if (cloudData.isUnlocked)
                        cloud.UnlockCloud(); // trigger cloud animation to dissapear
                    break;
                }
            }
        }
        // restore havent collect products
        foreach (var productData in mapData.products)
        {
            GameObject prefab = GetProductPrefab(productData.productType);
            if (prefab != null)
            {
                Instantiate(prefab, productData.position, Quaternion.identity);
            }
        }

        // restore player level
        LevelSystem.Instance.level = mapData.playerLevel;

        Debug.Log("Map state loaded.");
    }
    void ClearCurrentMapObjects()
    {
        foreach (var obj in Object.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None))
        {
            if (obj is AnimalFood or Habitat or Collectproduct)
                Destroy(obj.gameObject);
        }
    }

    private void OnApplicationQuit()
    {
        SaveMapState(); 
    }

}

//close game also can continue when start because already save in local device
//change scene will save because save in this script MapData and whne change back, will retsore back data
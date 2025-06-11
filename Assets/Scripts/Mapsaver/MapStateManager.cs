using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    public List<ProductionMachinePrefabEntry> machinePrefabs = new();

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

    [System.Serializable]
    public class ProductionMachinePrefabEntry
    {
        public string machineType;
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
    // get prefab from each production machine
    public GameObject GetProductPrefab(string productType)
    {
        foreach (var entry in productPrefabs)
            if (entry.productType == productType)
                return entry.prefab;
        Debug.LogWarning($"Product prefab not found for type: {productType}");
        return null;
    }
    // get prefab from each production machine
    public GameObject GetMachinePrefab(string machineType)
    {
        foreach (var entry in machinePrefabs)
            if (entry.machineType == machineType)
                return entry.prefab;
        Debug.LogWarning($"Machine prefab not found for type: {machineType}");
        return null;
    }


    // save curretn status to mapData.cs
    public void SaveMapState()
    {
        mapData = new MapData();

        //save habitat
        foreach (var habitat in Object.FindObjectsByType<Habitat>(FindObjectsSortMode.None))
        {
            mapData.habitats.Add(new HabitatData
            {
                habitatType = habitat.habitatType,
                position = (Vector2)habitat.transform.position
            });
        }

        // save animals
        foreach (var animal in Object.FindObjectsByType<AnimalFood>(FindObjectsSortMode.None))
        {
            mapData.animals.Add(animal.ExportData());
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
        // save products
        foreach (var product in Object.FindObjectsByType<Collectproduct>(FindObjectsSortMode.None))
        {
            mapData.products.Add(new ProductData
            {
                position = product.transform.position,
                productType = product.productType,
                isCollected = product.isCollected //save collect status
            });
        }
        // save production machines
        foreach (var machine in Object.FindObjectsByType<ProductionMachine>(FindObjectsSortMode.None))
        {
            mapData.machines.Add(machine.GetSaveData());
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
            if (productData.isCollected) continue;
            GameObject prefab = GetProductPrefab(productData.productType);
            if (prefab != null)
            {
                GameObject productInstance = Instantiate(prefab, productData.position, Quaternion.identity);
                productInstance.transform.position = new Vector3(productData.position.x, productData.position.y, -5); // set z position to -5 for 2D view to near with camera(prevent block by other objects)

                if (productInstance.TryGetComponent<Collectproduct>(out var collect))
                {
                    collect.isCollected = false;

                    // ensure the collider is enabled for collection
                    if (collect.TryGetComponent<Collider2D>(out var col))
                    {
                        collect.productType = productData.productType;
                        col.enabled = true;
                        col.isTrigger = false;
                    }
                    productInstance.layer = LayerMask.NameToLayer("Default");
                }
            }
        }

            foreach (var machineData in mapData.machines)
            {
                GameObject machinePrefab = GetMachinePrefab(machineData.machineType);
                if (machinePrefab == null)
                {
                    continue;
                }
                GameObject machineGO = Instantiate(machinePrefab, machineData.position, Quaternion.identity);
                var machine = machineGO.GetComponent<ProductionMachine>();

                var recipe = Resources.Load<MachineRecipe>($"Recipes/{machineData.currentRecipe}");
                if (recipe == null)
                {
                    Debug.LogWarning($"Missing recipe asset: {machineData.currentRecipe}");
                    continue;
                }

                machine.recipe = recipe;
                machine.machineID = machineData.machineType; // or some unique ID

                if (machineData.isProducing)
                {
                    machine.ResumeProcessing(machineData.remainingOutputCount, machineData.remainingTime);
                }
                else
                {
                    machine.RestoreIdleState(machineData.currentRecipe);
                }
            }

            // restore player level
            LevelSystem.Instance.level = mapData.playerLevel;

            Debug.Log("Map state loaded.");
        }
    
                
    private static void ClearCurrentMapObjects()
    {
        {
            foreach (var obj in Object.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None))
            {
                if (obj is AnimalFood or Habitat or Collectproduct or ProductionMachine)
                    Destroy(obj.gameObject);
            }
        }
    }

    private void OnApplicationQuit()
    {
        SaveMapState();
    }

}

//close game also can continue when start because already save in local device
//change scene will save because save in this script MapData and whne change back, will retsore back data
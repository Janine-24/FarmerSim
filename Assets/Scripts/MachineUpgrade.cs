using UnityEngine;

public class MachineUpgrade : MonoBehaviour
{
    public ShopItem shopItem;
    private bool hasUpgraded = false;
    private int lastCheckedLevel = -1;


    void Start()
    {
        CheckUpgrade(); // Check immediately after drop machine to see need to upgrade or not
    }

    void Update()
    {
        if (LevelSystem.Instance != null && LevelSystem.Instance.level != lastCheckedLevel)
        {
            lastCheckedLevel = LevelSystem.Instance.level;
            CheckUpgrade();
        }
    }

    void CheckUpgrade()
    {
        if (hasUpgraded || shopItem.machinelevelup == null)
            return;

        if (LevelSystem.Instance == null)
            return;

        int playerLevel = LevelSystem.Instance.level;

        if (playerLevel >= shopItem.requiredLevelup)
        {
            UpgradeMachine();
        }
    }

    void UpgradeMachine()
    {
        hasUpgraded = true;

        Vector3 position = transform.position;
        Quaternion rotation = transform.rotation;

        GameObject upgraded = Instantiate(shopItem.machinelevelup, position, rotation);

        if (upgraded.TryGetComponent<MachineUpgrade>(out var upgradedController))
        {
            upgradedController.shopItem = this.shopItem; // or a new ShopItem for the upgraded version
        }

        if (upgraded.TryGetComponent<ProductionMachine>(out var newMachine))
        {
            newMachine.machineID = shopItem.machinelevelup.name;

            var oldMachine = GetComponent<ProductionMachine>();
            if (oldMachine != null && oldMachine.isProcessing)
            {
                newMachine.recipe = oldMachine.recipe;
                newMachine.ResumeProcessing(oldMachine.GetRemainingOutputs(), oldMachine.currentTimer);
            }
        }

        Destroy(gameObject);
    }

}

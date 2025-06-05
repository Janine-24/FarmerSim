using UnityEngine;

public class MachineUpgrade : MonoBehaviour
{
    public ShopItem shopItem;
    private bool hasUpgraded = false;

    void Start()
    {
        CheckUpgrade(); // 放下后立即检查是否该升级
    }

    void Update()
    {
        CheckUpgrade(); // 每帧检查玩家等级是否达标
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

        // 如果新机器也需要继续自动升级，可以复制当前的 shopItem 或设置新的
        MachineUpgrade upgradedController = upgraded.GetComponent<MachineUpgrade>();
        if (upgradedController != null)
        {
            upgradedController.shopItem = this.shopItem; // 或者设为更高级的 ShopItem
        }

        Destroy(gameObject);
    }
}

using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MachineUIManager : MonoBehaviour
{
    public static MachineUIManager Instance;

    [Header("Panel Elements")]
    public GameObject panel;
    public TextMeshProUGUI titleText;
    public Button closeButton;
    public Image outputIcon;
    public Slider productSlider;
    public TextMeshProUGUI productAmountText;
    public TextMeshProUGUI durationText;

    [Header("Input Slots")]
    public Slot_UI[] inputSlots = new Slot_UI[3]; // should assign in inspector

    [Header("Buttons")]
    public Button confirmButton;

    private ProductionMachine currentMachine;
    private int maxOutput;

    private void Awake()
    {
        Instance = this;
        panel.SetActive(false);
        closeButton.onClick.AddListener(() =>
        {
            panel.SetActive(false);
        });
    }

    public void OpenMachineUI(ProductionMachine machine)
    {
        // Always reset the panel
        panel.SetActive(false);  // Force close to reset listeners, etc.
        currentMachine = null;

        currentMachine = machine;

        if (currentMachine == null)
        {
            Debug.LogWarning("⚠️ Tried to open machine UI, but machine was null.");
            return;
        }

        panel.SetActive(true);

        MachineRecipe recipe = machine.recipe;

        titleText.text = recipe.recipeName;
        outputIcon.sprite = recipe.outputItem.icon;

        // Show ingredients
        for (int i = 0; i < inputSlots.Length; i++)
        {
            if (i < recipe.inputItems.Length)
            {
                var data = recipe.inputItems[i];
                inputSlots[i].SetItem(new Inventory.Slot
                {
                    itemName = data.item.itemName,
                    icon = data.item.icon,
                    itemData = data.item,
                    count = data.amountPerOutput
                });
            }
            else
            {
                inputSlots[i].SetEmpty(); // Clear any extra slots
            }
        }

        maxOutput = CalculateMaxOutput(recipe);
        productSlider.maxValue = maxOutput;
        productSlider.minValue = 0;
        productSlider.value = maxOutput;

        UpdateUIFeedback((int)productSlider.value);

        productSlider.onValueChanged.RemoveAllListeners();
        productSlider.onValueChanged.AddListener(val => UpdateUIFeedback((int)val));

        confirmButton.onClick.RemoveAllListeners();
        confirmButton.onClick.AddListener(() => ConfirmProduction((int)productSlider.value));
    }

    private int CalculateMaxOutput(MachineRecipe recipe)
    {
        var backpack = GameManager.instance.player.inventoryManager.backpack;
        var toolbar = GameManager.instance.player.inventoryManager.toolbar;

        int max = int.MaxValue;

        foreach (var ingredient in recipe.inputItems)
        {
            int owned = 0;

            // Count in backpack
            foreach (var slot in backpack.slots)
            {
                if (slot.itemName == ingredient.item.itemName)
                {
                    owned += slot.count;
                }
            }

            // Count in toolbar
            foreach (var slot in toolbar.slots)
            {
                if (slot.itemName == ingredient.item.itemName)
                {
                    owned += slot.count;
                }
            }

            int possible = owned / ingredient.amountPerOutput;
            max = Mathf.Min(max, possible);

            Debug.Log($"[MaxOutput] {ingredient.item.itemName}: total owned = {owned}, per output = {ingredient.amountPerOutput}, max = {possible}");
        }

        return max;
    }


    private void UpdateUIFeedback(int outputCount)
    {
        productAmountText.text = $"{outputCount} / {maxOutput}";
        float totalTime = currentMachine.recipe.processingTimePerBatch * outputCount;
        durationText.text = $"{totalTime:0.0}s";
    }

    private void ConfirmProduction(int amount)
    {
        if (amount <= 0) return;

        var backpack = GameManager.instance.player.inventoryManager.backpack;
        var toolbar = GameManager.instance.player.inventoryManager.toolbar;

        foreach (var input in currentMachine.recipe.inputItems)
        {
            int needed = input.amountPerOutput * amount;

            // First remove from backpack
            for (int i = 0; i < backpack.slots.Count && needed > 0; i++)
            {
                if (backpack.slots[i].itemName == input.item.itemName)
                {
                    int remove = Mathf.Min(needed, backpack.slots[i].count);
                    backpack.Remove(i, remove);
                    needed -= remove;
                }
            }

            // Then remove remaining from toolbar
            for (int i = 0; i < toolbar.slots.Count && needed > 0; i++)
            {
                if (toolbar.slots[i].itemName == input.item.itemName)
                {
                    int remove = Mathf.Min(needed, toolbar.slots[i].count);
                    toolbar.Remove(i, remove);
                    needed -= remove;
                }
            }

            if (needed > 0)
            {
                Debug.LogWarning($"[ConfirmProduction] Unexpected: Could not remove enough of {input.item.itemName}");
            }
        }

        if (currentMachine == null || currentMachine.gameObject == null)
        {
            Debug.LogWarning("⚠️ Machine is missing/destroyed.");
            panel.SetActive(false);
            return;
        }


        currentMachine.StartProcessing(amount);
        GameManager.instance.uiManager.RefreshAll();
        panel.SetActive(false);
    }

}

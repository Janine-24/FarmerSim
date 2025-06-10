using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class PlantWateringUI : MonoBehaviour
{
    public static PlantWateringUI Instance;

    public GameObject panel;
    public TextMeshProUGUI hintText;
    public Slider progressBar;
    private PlantInstance currentPlant;

    private void Awake()
    {
        Instance = this;
        panel.SetActive(false);
        progressBar.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (panel.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                currentPlant.TryWater();
            }
            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                ClosePanel();
            }
        }
    }

    public void OpenPanel(PlantInstance plant)
    {
        Debug.Log($"🌱 OpenPanel() CALLED for plant: {plant.name}");

        currentPlant = plant;

        if (panel == null)
        {
            Debug.LogError("❌ Panel is NULL in PlantWateringUI!");
            return;
        }

        panel.SetActive(true);
        hintText.text = "Press F to water";
    }


    public void ClosePanel()
    {
        panel.SetActive(false);
        hintText.text = "";
    }

    public void ShowHint(string message)
    {
        hintText.text = message;
        CancelInvoke(nameof(ClearHint));
        Invoke(nameof(ClearHint), 2f);
    }

    private void ClearHint()
    {
        hintText.text = "";
    }

    public void StartProgress(float duration, System.Action onComplete)
    {
        StartCoroutine(ProgressRoutine(duration, onComplete));
    }

    private IEnumerator ProgressRoutine(float duration, System.Action onComplete)
    {
        progressBar.gameObject.SetActive(true);
        float time = 0f;
        while (time < duration)
        {
            time += Time.deltaTime;
            progressBar.value = time / duration;
            yield return null;
        }
        progressBar.gameObject.SetActive(false);
        onComplete?.Invoke();
    }
}

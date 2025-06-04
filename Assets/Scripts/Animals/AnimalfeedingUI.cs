using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AnimalFeedingUI : MonoBehaviour
{
    public static AnimalFeedingUI Instance;
    public GameObject panel;
    public TextMeshProUGUI hintText;
    public Slider progressBar;
    private AnimalFood currentAnimal;

    private void Awake()
    {
        Instance = this;
        panel.SetActive(false);
        progressBar.gameObject.SetActive(false);
        ClearHint();
    }

    private void Update()
    {
        if (panel.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                currentAnimal.TryFeedAnimal();
            }
            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                ClosePanel();
            }
        }
    }

    public void OpenPanel(AnimalFood animal)
    {
        currentAnimal = animal;
        panel.SetActive(true);
        hintText.text = "Press F for feeding $10";
    }

    public void ClosePanel()
    {
        panel.SetActive(false);
        ClearHint();
    }

    public void ShowHint(string message)
    {
        hintText.text = message;
        CancelInvoke(nameof(ClearHint));
        Invoke(nameof(ClearHint), 2f); // 2 sec 
    }

    private void ClearHint()
    {
        hintText.text = "";
    }

    public void StartProgress(float duration, System.Action onComplete)
    {
        StartCoroutine(ProgressCoroutine(duration, onComplete));
    }

    private System.Collections.IEnumerator ProgressCoroutine(float duration, System.Action onComplete)
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
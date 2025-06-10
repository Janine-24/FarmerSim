using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AnimalFeedingUI : MonoBehaviour
{
    public GameObject panel;
    public TextMeshProUGUI hintText;
    public Slider progressBar;
    private AnimalFood currentAnimal;
    private Coroutine progressCoroutine;
    public Image feedImageUI; 


    private void Awake()
    {
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
        return;
    }

    public void OpenPanel(AnimalFood animal)
    {
        currentAnimal = animal;
        panel.SetActive(true);
        hintText.text = $"Press F for feeding ${animal.feedCost}";
        if (feedImageUI != null && animal.feedImage != null)
        {
            feedImageUI.sprite = animal.feedImage;
        }
        if (animal.IsProducing())
        {
            progressBar.gameObject.SetActive(true);
        }
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
        if (progressCoroutine != null)
        {
            StopCoroutine(progressCoroutine);
        }
        progressCoroutine = StartCoroutine(ProgressCoroutine(duration, onComplete));
    }
    public void UpdateProgressBar(float normalizedValue)
    {
        progressBar.value = Mathf.Clamp01(normalizedValue);
    }

    private System.Collections.IEnumerator ProgressCoroutine(float duration, System.Action onComplete)
    {
        progressBar.gameObject.SetActive(true);
        progressBar.value = 0f;
        float time = 0f;
        while (time < duration)
        {
            time += Time.deltaTime;
            progressBar.value = time / duration;
            yield return null; //wait for next frame to continue repeat loop 
        }
        progressBar.gameObject.SetActive(false);
        progressCoroutine = null; //clear the reference to coroutine
        onComplete?.Invoke(); // invoke the callback when progress complete
    }
}
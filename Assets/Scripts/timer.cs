using UnityEngine;
using TMPro;

public class MachineTimer : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    public float processingTime = 10f; // Time in seconds
    private float remainingTime;
    private bool isProcessing = false;

    void Update()
    {
        if (isProcessing)
        {
            remainingTime -= Time.deltaTime;
            timerText.text = FormatTime(remainingTime);

            if (remainingTime <= 0)
            {
                remainingTime = 0;
                isProcessing = false;
                timerText.text = "Done!";
                // Trigger any machine-complete logic here
            }
        }
    }

    public void StartMachine()
    {
        remainingTime = processingTime;
        isProcessing = true;
    }

    private string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);
        return $"{minutes:00}:{seconds:00}";
    }
}


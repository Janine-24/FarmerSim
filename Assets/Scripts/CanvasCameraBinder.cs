using UnityEngine;

/// <summary>
/// 自动将当前场景中的主摄像机绑定到 Canvas 上（适用于 Screen Space - Camera 模式）
/// </summary>
[RequireComponent(typeof(Canvas))]
public class CanvasCameraBinder : MonoBehaviour
{
    void Start()
    {
        Canvas canvas = GetComponent<Canvas>();
        if (canvas.renderMode == RenderMode.ScreenSpaceCamera)
        {
            Camera mainCam = Camera.main;

            if (mainCam != null)
            {
                canvas.worldCamera = mainCam;
                Debug.Log("CanvasCameraBinder: 成功绑定主摄像机到 Canvas！");
            }
            else
            {
                Debug.LogWarning("CanvasCameraBinder: 找不到主摄像机（Tag 为 MainCamera）！");
            }
        }
    }
}

using UnityEngine;
using TMPro;

public class FPS_display : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI FpsText;
    float pollingTime = 1f, time;
    int frameCount;

    void Update()
    {
        time += Time.deltaTime;
        frameCount++;
        if (time >= pollingTime)
        {
            int frameRate = Mathf.RoundToInt(frameCount / time);
            FpsText.text = frameRate.ToString() + " FPS";
            time -= pollingTime;
            frameCount = 0;
        }
    }
}

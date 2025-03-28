using UnityEngine;
using TMPro;

public class FrameRateCounter : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI display;

    [SerializeField, Range(0.1f, 2f)]
    float sampleDuration = 1f;

    public enum DisplayMode { FPS, MS }

    [SerializeField]
    DisplayMode displayMode = DisplayMode.FPS;

    /// <summary>
    /// Keeps track of the number of frames rendered
    /// </summary>
    int frames;

    /// <summary>
    /// Total duration that the scenes have been renered
    /// </summary>
    float duration;

    float bestDuration = float.MaxValue;
    float worstDuration = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float frameDuration = Time.unscaledDeltaTime;
        frames += 1;
        duration += frameDuration;
        if (frameDuration < bestDuration)
        {
            bestDuration = frameDuration;
        }

        if (frameDuration > worstDuration)
        {
            worstDuration = frameDuration;
        }

        if (duration >= sampleDuration)
        {
            if (displayMode == DisplayMode.FPS)
            {
                display.SetText(
                    "FPS\nBest: {0:0}\nAverage: {0:0}\nWorst: {0:0}",
                    1f / bestDuration,
                    frames / duration,
                    1f / worstDuration
                );
            }
            else
            {
                display.SetText(
                    "MS\nBest: {0:1}\nAverage: {0:1}\nWorst: {0:1}",
                    1000f * bestDuration,
                    1000f * duration / frames,
                    1000f * worstDuration
                );
            }

            frames = 0;
            duration = 0f;
            bestDuration = float.MaxValue;
            worstDuration = 0f;
        }
    }
}

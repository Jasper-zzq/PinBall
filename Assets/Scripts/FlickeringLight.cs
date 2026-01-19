using UnityEngine;

public class FlickeringLight : MonoBehaviour
{
    [Header("Light Settings")]
    [SerializeField] private Light targetLight;
    [SerializeField] private float baseIntensity = 2.0f;
    [SerializeField] private float flickerIntensity = 0.5f;

    [Header("Flicker Pattern")]
    [SerializeField] private float flickerSpeed = 0.1f;
    [SerializeField] private float flickerVariance = 0.05f;
    [SerializeField] private float blackoutChance = 0.02f;
    [SerializeField] private float blackoutDuration = 0.3f;

    [Header("Color Variation")]
    [SerializeField] private Color baseColor = new Color(1f, 0.8f, 0.6f);
    [SerializeField] private Color dimColor = new Color(0.8f, 0.4f, 0.2f);

    private float nextFlickerTime;
    private bool isBlackout;
    private float blackoutEndTime;

    private void Start()
    {
        if (targetLight == null)
        {
            targetLight = GetComponent<Light>();
        }

        if (targetLight != null)
        {
            targetLight.intensity = baseIntensity;
            targetLight.color = baseColor;
        }
    }

    private void Update()
    {
        if (targetLight == null) return;

        // Handle blackout state
        if (isBlackout)
        {
            if (Time.time >= blackoutEndTime)
            {
                isBlackout = false;
                targetLight.intensity = baseIntensity;
                targetLight.color = baseColor;
            }
            else
            {
                targetLight.intensity = 0f;
                return;
            }
        }

        // Check for blackout trigger
        if (Random.value < blackoutChance * Time.deltaTime)
        {
            TriggerBlackout();
            return;
        }

        // Regular flickering
        if (Time.time >= nextFlickerTime)
        {
            float intensityMultiplier = Random.Range(0.3f, 1.2f);
            float flickerDuration = flickerSpeed + Random.Range(-flickerVariance, flickerVariance);

            targetLight.intensity = baseIntensity * intensityMultiplier * Random.Range(flickerIntensity, 1.0f);

            // Color variation during flicker
            if (intensityMultiplier < 0.7f)
            {
                targetLight.color = Color.Lerp(dimColor, baseColor, intensityMultiplier);
            }
            else
            {
                targetLight.color = baseColor;
            }

            nextFlickerTime = Time.time + flickerDuration;
        }
    }

    private void TriggerBlackout()
    {
        isBlackout = true;
        blackoutEndTime = Time.time + blackoutDuration;
        targetLight.intensity = 0f;
    }

    // Public method to manually trigger blackout
    public void TriggerEmergencyBlackout(float duration = -1f)
    {
        if (duration > 0)
        {
            blackoutDuration = duration;
        }
        TriggerBlackout();
    }

    // Method to change flicker pattern dynamically
    public void SetFlickerPattern(float speed, float variance, float blackoutProb)
    {
        flickerSpeed = speed;
        flickerVariance = variance;
        blackoutChance = blackoutProb;
    }
}

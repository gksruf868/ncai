using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class ObstacleGlow : MonoBehaviour
{
    [SerializeField] private float minIntensity = 0.15f;
    [SerializeField] private float maxIntensity = 1.2f;
    [SerializeField] private float pulseSpeed = 2.5f;

    private Renderer rend;
    private Material materialInstance;
    private bool isGlowing;
    private float phaseOffset;
    private Color glowColor;

    private void Awake()
    {
        rend = GetComponent<Renderer>();
        phaseOffset = Random.Range(0f, Mathf.PI * 2f);
        glowColor = Color.HSVToRGB(Random.value, 0.75f, 1f);
    }

    private void Update()
    {
        if (!isGlowing)
        {
            if (GameManager.Instance != null && GameManager.Instance.IsGameOver)
            {
                StartGlow();
            }
            return;
        }

        float t = (Mathf.Sin(Time.time * pulseSpeed + phaseOffset) + 1f) * 0.5f;
        float intensity = Mathf.Lerp(minIntensity, maxIntensity, t);
        materialInstance.SetColor("_EmissionColor", glowColor * intensity);
    }

    private void StartGlow()
    {
        materialInstance = rend.material;
        materialInstance.EnableKeyword("_EMISSION");
        materialInstance.globalIlluminationFlags = MaterialGlobalIlluminationFlags.RealtimeEmissive;
        isGlowing = true;
    }
}

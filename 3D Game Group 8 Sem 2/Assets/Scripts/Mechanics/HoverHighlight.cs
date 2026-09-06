using UnityEngine;

public class HoverHighlight : MonoBehaviour
{
    [Header("UI Prompt")]
    [SerializeField] private GameObject floatingCanvas;

    [Header("Glow Settings")]
    [SerializeField] private Renderer meshRenderer;
    [ColorUsage(true, true)][SerializeField] private Color glowColor = Color.pink;

    private Material targetMaterial;
    private bool isHighlighted = false;

    private void Awake()
    {
        if (meshRenderer == null)
            meshRenderer = GetComponentInChildren<Renderer>();

        if (meshRenderer != null)
            targetMaterial = meshRenderer.material;

        SetHighlight(false);
    }

    public void SetHighlight(bool enable)
    {
        if (isHighlighted == enable) return;
        isHighlighted = enable;

        // Toggle Floating 'E'
        if (floatingCanvas != null)
        {
            floatingCanvas.SetActive(enable);
            // Make floating prompt face the camera
            if (enable && Camera.main != null)
            {
                floatingCanvas.transform.rotation = Quaternion.LookRotation(floatingCanvas.transform.position - Camera.main.transform.position);
            }
        }

        // Toggle Material Emission Glow
        if (targetMaterial != null)
        {
            if (enable)
            {
                targetMaterial.EnableKeyword("_EMISSION");
                targetMaterial.SetColor("_EmissionColor", glowColor * 2f); // Glow multiplier
            }
            else
            {
                targetMaterial.DisableKeyword("_EMISSION");
            }
        }
    }
}

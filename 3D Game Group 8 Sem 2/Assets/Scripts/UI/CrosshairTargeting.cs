using UnityEngine;
using UnityEngine.UI;
public class CrosshairTargeting : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Camera playerCamera;
    [SerializeField] private FrogBlepController blepController;

    [Header("Crosshair Visuals")]
    [SerializeField] private Image crosshairImage;
    [SerializeField] private Color defaultColor = Color.white;
    [SerializeField] private Color targetInReachColor = Color.purple;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
   private void Start()
    {
        if (playerCamera == null)
            playerCamera = Camera.main;

        if (crosshairImage == null)
            crosshairImage = GetComponent<Image>();

        if (blepController == null)
            blepController = Object.FindAnyObjectByType<FrogBlepController>();
    }

    // Update is called once per frame
   private void Update()
    {
        if (GameManager.instance != null && GameManager.instance.IsPaused)
            return;

        CheckForBlepTarget();
    }

    private void CheckForBlepTarget()
    {
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

        float reachDistance = blepController != null ? blepController.maxReachDistance : 10f;
        LayerMask itemLayer = blepController != null ? blepController.collectableLayer : ~0;

        if (Physics.Raycast(ray, out RaycastHit hit, reachDistance, itemLayer))
        {
            crosshairImage.color = targetInReachColor;
        }
        else
        {
            crosshairImage.color = defaultColor;
        }
    }
}

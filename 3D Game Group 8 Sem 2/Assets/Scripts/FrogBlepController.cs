using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogBlepController : MonoBehaviour
{
    [Header("Blep Settings")]
    [SerializeField] private float maxReachDistance = 10f;
    [SerializeField] private float extendSpeed = 25f;
    [SerializeField] private float retractSpeed = 30f;
    [SerializeField] private LayerMask collectableLayer;

    [Header("Visual Components")]
    [SerializeField] private LineRenderer tongueLine;
    [SerializeField] private Transform mouthOrigin; //childed to camera and place slightly below camera

    private bool isBlepping = false;
    private HoverHighlight currentHighlightedItem;

    private void Update()
    {
        CheckForAimHighlight();

        if (Input.GetKeyDown(KeyCode.E) && !isBlepping)
        {
            StartCoroutine(PerformBlep());
        }
    }

    private void CheckForAimHighlight()
    {
        // Raycast straight out from camera center
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, maxReachDistance, collectableLayer))
        {
            if (hit.collider.TryGetComponent<HoverHighlight>(out HoverHighlight highlightScript))
            {
                if (currentHighlightedItem != highlightScript)
                {
                    // Clear previous highlight if aiming at a different item
                    if (currentHighlightedItem != null) currentHighlightedItem.SetHighlight(false);

                    currentHighlightedItem = highlightScript;
                    currentHighlightedItem.SetHighlight(true);
                }
                return;
            }
        }

        // If raycast hits nothing or out of range, clear highlight
        if (currentHighlightedItem != null)
        {
            currentHighlightedItem.SetHighlight(false);
            currentHighlightedItem = null;
        }
    }

    private IEnumerator PerformBlep()
    {
        isBlepping = true;
        tongueLine.enabled = true;

        Vector3 startPosition = mouthOrigin.position;
        Vector3 targetPosition = transform.position + (transform.forward * maxReachDistance);
        Transform grabbedItem = null;

        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, maxReachDistance, collectableLayer))
        {
            targetPosition = hit.point;

            if (hit.collider.TryGetComponent<ICollectable>(out _))
            {
                grabbedItem = hit.transform;
            }
        }

        // Phase 1: Tongue Extends
        float progress = 0f;
        Vector3 currentTipPos = startPosition;

        while (progress <1f)
        {
            progress += Time.deltaTime * extendSpeed;
            currentTipPos = Vector3.Lerp(startPosition, targetPosition, progress);

            tongueLine.SetPosition(0, mouthOrigin.position);
            tongueLine.SetPosition(1, currentTipPos);
            yield return null;
        }

        //Phase 2: Attach Object to Tongue
        if (grabbedItem != null)
        {
            if (grabbedItem.TryGetComponent<Rigidbody>(out Rigidbody rb))
            {
                rb.isKinematic = true;
            }
            grabbedItem.SetParent(mouthOrigin);
        }

        //Phase 3: Tongue Retracts
        progress = 0f;
        Vector3 reachPoint = currentTipPos;

        while (progress < 1f)
        {
            progress += Time.deltaTime * retractSpeed;
            Vector3 retractPos = Vector3.Lerp(reachPoint, mouthOrigin.position, progress);

            tongueLine.SetPosition(0, mouthOrigin.position);
            tongueLine.SetPosition(1, retractPos);

            if (grabbedItem != null)
            {
                grabbedItem.position = retractPos;
            }
            yield return null;
        }

        //Phase 4: Item Gets Placed into Inventory
        if (grabbedItem !=null)
        {
            if (grabbedItem.TryGetComponent<ICollectable>(out ICollectable item))
            {
                item.OnCollected();
            }
        }

        tongueLine.enabled = false;
        isBlepping = false;
    }
}

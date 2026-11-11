using System.Collections.Generic;
using UnityEngine;

public class Crafting : MonoBehaviour
{
    private readonly HashSet<Collider> playersInRange = new();

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponentInParent<FPController>() != null &&
            playersInRange.Add(other) && playersInRange.Count == 1)
        {
            Inventory2.instance?.SetCauldronInRange(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (playersInRange.Remove(other) && playersInRange.Count == 0)
            Inventory2.instance?.SetCauldronInRange(false);
    }

    private void OnDisable()
    {
        if (playersInRange.Count > 0 && Inventory2.instance != null)
            Inventory2.instance.SetCauldronInRange(false);
        playersInRange.Clear();
    }
}

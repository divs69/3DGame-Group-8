using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public interface ICollectable
{
    void OnCollected();
}

public class Collector : MonoBehaviour 
{
    public Transform CollectorSource;
    public float CollectRange;
    public LayerMask collectableLayer;

    private List<ICollectable> inventory = new List<ICollectable>();


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            CheckCollect();
        }
    }

    private void CheckCollect()
    {
        Collider[] colliders = Physics.OverlapSphere(CollectorSource.position, CollectRange, collectableLayer);
        foreach (Collider collider in colliders)
        {

            if (collider.TryGetComponent<ICollectable>(out var collectObj))
            { 

                float distance = Vector3.Distance(CollectorSource.position, collider.transform.position);
                 if (distance <= CollectRange && !inventory.Contains(collectObj))
                 {
                    inventory.Add(collectObj);
                    collectObj.OnCollected();
                
                 }
            }
        
            
        }
    }
}

using UnityEngine;
using System.Collections.Generic;

public class ObjectTracker : MonoBehaviour
{
    public ObjectTrackerData objectTrackerData;
    public GameObject[] objects;
    

    private void Start()
    {
        objects = GameObject.FindGameObjectsWithTag("Collect");

        foreach (GameObject obj in objects) {
           

            if (objectTrackerData.collectedObjects.Contains(obj.gameObject.name))
            {
                obj.SetActive(false);
            }
        
        
        } 

    }




}

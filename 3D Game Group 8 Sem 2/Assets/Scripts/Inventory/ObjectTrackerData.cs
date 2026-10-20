using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

[CreateAssetMenu(fileName = "ObjectTrackerData", menuName = "Scriptable Objects/ObjectTrackerData")]
public class ObjectTrackerData : ScriptableObject
{

    [SerializeField]
    public List<string> collectedObjects;


    public void AddCollectedObjectName(string name)
    {
        collectedObjects.Add(name);
    }
    

}

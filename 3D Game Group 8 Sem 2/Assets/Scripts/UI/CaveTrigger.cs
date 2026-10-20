using UnityEngine;
using UnityEngine.SceneManagement;

public class CaveTrigger : MonoBehaviour
{

    public BoxCollider Cavetrigger;
    public Transform playerTransform;
    public Vector3 targetSpawnPoint;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {

            PlayerPositionTransfer.NextSpawnPosition = targetSpawnPoint;
            PlayerPositionTransfer.UseCustomPosition = true;


            SceneManager.LoadScene("CaveScene");

            Debug.Log("it works?");
        }
    }
    
}

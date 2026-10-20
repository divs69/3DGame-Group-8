using UnityEngine;
using UnityEngine.SceneManagement;

public class ForestTrigger : MonoBehaviour
{

    public BoxCollider Foresttrigger;
    public Transform playerTransform;
    public Vector3 targetSpawnPoint;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {


            PlayerPositionTransfer.NextSpawnPosition = targetSpawnPoint;
            PlayerPositionTransfer.UseCustomPosition = true;

            SceneManager.LoadScene("LevelOne");
        }
    }

}

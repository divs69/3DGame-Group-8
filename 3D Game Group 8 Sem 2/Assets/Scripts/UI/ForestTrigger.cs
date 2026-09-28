using UnityEngine;
using UnityEngine.SceneManagement;

public class ForestTrigger : MonoBehaviour
{

    public BoxCollider Foresttrigger;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene("LevelOne");
        }
    }

}

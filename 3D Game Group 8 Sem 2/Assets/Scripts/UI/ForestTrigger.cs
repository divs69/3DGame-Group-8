using UnityEngine;
using UnityEngine.SceneManagement;

public class ForestTrigger : MonoBehaviour
{

    public BoxCollider Foresttrigger;


    [SerializeField]

    public BoxCollider foresttrigger;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene("NewLevelOne");
        }
    }

}

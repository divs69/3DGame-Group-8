using UnityEngine;
using UnityEngine.SceneManagement;

public class CaveTrigger : MonoBehaviour
{

    public BoxCollider Cavetrigger;

    
        [SerializeField]
     
    public BoxCollider cavetrigger;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene("Cave");
        }
    }
    
}

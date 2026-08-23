using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CaveTrigger : MonoBehaviour
{
       

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene("Cave");
        }
    }
}
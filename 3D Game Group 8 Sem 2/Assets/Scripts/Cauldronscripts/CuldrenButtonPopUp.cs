using UnityEngine;

public class CuldrenButtonPopup : MonoBehaviour
{
    //ui button
    [SerializeField] private GameObject uiButton;

    //playerTag
    [SerializeField] private string targetTag = "Player";

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        //hidden button
        if(uiButton != null)
        {
            uiButton.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //object entering is plaayer check
        if (other.CompareTag("targetTag"))
        {
            if (uiButton != null)
            {
              uiButton.SetActive(true);
                Debug.Log("Button is showing up");
            }
        }     
    }


    // Update is called once per frame
    private void OnTriggerExit(Collider other)
    {
        //Hide button after
        if (other.CompareTag("targetTag"))
        {
          if (uiButton !=null)
            {
                uiButton.SetActive(false);
            }
        }
    }
}

using UnityEngine;

public class CauldronOpen : MonoBehaviour
{
    [Header("Cauldron UI")]
    public GameObject cauldronpanel;
    [Header("Player")]
    public MonoBehaviour playerMovement;
    private bool inrange = false;
    private bool cauldroninteract = false;

    private void Start()
    {
        cauldronpanel.SetActive = false;

    }
    private void Update()
    {
        if (inrange && Input.GetKeyDown(KeyCode.Q))
        {
            ToggleCauldron();
        }
    }

    private void ToggleCauldron()
    {
        cauldroninteract = !cauldroninteract;
        cauldronpanel.SetActive(cauldroninteract);
        if (cauldroninteract)
        {
            playerMovement.enabled = false;
            Cursor.visible = true;

        }
        else
        {
            playerMovement = true;
            Cursor.visible = false;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inrange = true;
        }
    }
}

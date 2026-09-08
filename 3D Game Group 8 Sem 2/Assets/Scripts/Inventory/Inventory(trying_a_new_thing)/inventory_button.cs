using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

public class inventory_button : MonoBehaviour
{
    public Button inventorybutton;
    public GameObject Maininventorygroup;

    public bool IsPaused { get; private set; }

    void Start()
    {
        inventorybutton.onClick.AddListener(DisplayMainInventory);

    }

    private void DisplayMainInventory()
    {
        IsPaused = true;
        Maininventorygroup.SetActive(true);
        Time.timeScale = 0f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }



}

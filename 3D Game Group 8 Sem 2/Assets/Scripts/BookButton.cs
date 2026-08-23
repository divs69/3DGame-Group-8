using UnityEngine;
using UnityEngine.UI;


public class BookButton : MonoBehaviour
{
    public Button bookbutton;
    public GameObject UIPanel;

    void Start()
    {
        bookbutton.onClick.AddListener(OpenSettings);
        UIPanel.SetActive(false);
    }

    // Update is called once per frame
    void OpenSettings()
    {
        UIPanel.SetActive(true);
    }
}

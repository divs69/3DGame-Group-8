using UnityEngine;
using TMPro;

// "WASD to move" on the bedroom mirror.
public class PromptUI : MonoBehaviour
{
    public static PromptUI Instance;
    public TMP_Text hintText;

    void Awake()
    {
        Instance = this;
        ClearHint();
    }

    public void SetHint(string text) { hintText.gameObject.SetActive(true); hintText.text = text; }
    public void ClearHint() { hintText.gameObject.SetActive(false); }
}
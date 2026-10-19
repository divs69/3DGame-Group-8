using System.Collections;
using UnityEngine;
using TMPro;

public class DialogueUI : MonoBehaviour
{
    public static DialogueUI Instance;

    public GameObject panel;
    public TMP_Text speakerText;
    public TMP_Text bodyText;
    public GameObject continueIcon;
    public float charsPerSecond = 40f;
    public KeyCode advanceKey = KeyCode.Return;

    void Awake()
    {
        Instance = this;
        panel.SetActive(false);
    }

    // autoAdvanceSeconds > 0: closes itself after that many seconds.
    // autoAdvanceSeconds = 0: waits for the player to press Enter / click.
    public IEnumerator Say(string speaker, string line, float autoAdvanceSeconds = 0f)
    {
        panel.SetActive(true);
        speakerText.text = speaker;
        bodyText.text = "";
        if (continueIcon) continueIcon.SetActive(false);

        for (int i = 1; i <= line.Length; i++)
        {
            bodyText.text = line.Substring(0, i);
            if (Skip()) { bodyText.text = line; break; }
            yield return new WaitForSeconds(1f / charsPerSecond);
        }

        yield return null; // so the same key press doesn't also close the line

        if (autoAdvanceSeconds > 0f)
        {
            yield return new WaitForSeconds(autoAdvanceSeconds);
        }
        else
        {
            if (continueIcon) continueIcon.SetActive(true);
            while (!Skip()) yield return null;
        }

        if (continueIcon) continueIcon.SetActive(false);
    }

    public void Hide() { panel.SetActive(false); }

    bool Skip() { return Input.GetKeyDown(advanceKey) || Input.GetMouseButtonDown(0); }
}
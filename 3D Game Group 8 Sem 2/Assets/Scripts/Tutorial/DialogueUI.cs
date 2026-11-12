using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


[System.Serializable]
public class SpeakerPortrait
{
    public string speakerName;
    public Sprite portrait;
}

public class DialogueUI : MonoBehaviour
{
    public static DialogueUI Instance;

    public GameObject panel;
    public TMP_Text speakerText;
    public TMP_Text bodyText;
    public GameObject continueIcon;
    public float charsPerSecond = 40f;
    public KeyCode advanceKey = KeyCode.Return;
    public bool clickToContinueOnly = true;   // Ticked = every line waits for a click

    [Header("Portraits")]
    public Image portraitImage;
    public List<SpeakerPortrait> portraits = new List<SpeakerPortrait>();

    void Awake()
    {
        Instance = this;
        if (portraitImage != null) portraitImage.preserveAspect = true;
        panel.SetActive(false);
    }

    public IEnumerator Say(string speaker, string line, float autoAdvanceSeconds = 0f)
    {
        panel.SetActive(true);
        speakerText.text = speaker;
        bodyText.text = "";
        SetPortrait(speaker);
        if (continueIcon) continueIcon.SetActive(false);

        for (int i = 1; i <= line.Length; i++)
        {
            bodyText.text = line.Substring(0, i);
            if (Skip()) { bodyText.text = line; break; }
            yield return new WaitForSeconds(1f / charsPerSecond);
        }

        yield return null;

        if (autoAdvanceSeconds > 0f && !clickToContinueOnly)
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

    void SetPortrait(string speaker)
    {
        if (portraitImage == null) return;

        foreach (SpeakerPortrait p in portraits)
        {
            if (p.portrait != null && string.Equals(p.speakerName, speaker, System.StringComparison.OrdinalIgnoreCase))
            {
                portraitImage.sprite = p.portrait;
                portraitImage.enabled = true;
                return;
            }
        }

        portraitImage.enabled = false;
    }

    public void Hide() { panel.SetActive(false); }

    bool Skip() { return Input.GetKeyDown(advanceKey) || Input.GetMouseButtonDown(0); }
}
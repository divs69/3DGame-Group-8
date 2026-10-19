using System;
using System.Collections;
using UnityEngine;

public class TutorialDirector : MonoBehaviour
{
    [Header("Scene references")]
    public Transform player;
    public FrogBlepController blep; // blep script on the player
    public GuideTrail trail; // sparkle line
    public GuideNPC tuna;

    [Header("Tutorial objects")]
    public HoverHighlight grimoireHighlight; // your highlight script on the grimoire on the table
    public TutorialInteractable grimoirePlaceholder; // for testing only
    public HoverHighlight cauldronHighlight; // highlight on the cauldron

    [Header("Places (empty objects you place in the scene)")]
    public Transform forestSpot; // where sparkle trail leads outside
    public Transform insideSpot; // where the trail leads when the player back in
    public Transform cauldronSpot; // where Tuna stands beside the cauldron
    public Transform[] caveRoute; // Tuna's walking route

    [Header("Cave")]
    public Collider caveEntrance;

    [Header("Settings")]
    public bool runEvenIfCompleted = false; // Tick this while testing to replay the tutorial
    public int ingredientsToCollect = 3;
    public string grimoireOpenHint = "Press ESC to open your grimoire";

    int collected;
    bool potionCrafted;
    bool grimoireCollected;

    DialogueUI Dlg { get { return DialogueUI.Instance; } }
    PromptUI Ui { get { return PromptUI.Instance; } }

    void OnEnable()
    {
        CollectableIngredient.OnAnyCollected += CountCollected;
        TutorialEvents.PotionCrafted += MarkCrafted;
        TutorialEvents.GrimoireCollected += MarkGrimoireCollected;
        TutorialInteractable.OnUsed += PlaceholderUsed;
    }
    void OnDisable()
    {
        CollectableIngredient.OnAnyCollected -= CountCollected;
        TutorialEvents.PotionCrafted -= MarkCrafted;
        TutorialEvents.GrimoireCollected -= MarkGrimoireCollected;
        TutorialInteractable.OnUsed -= PlaceholderUsed;
    }
    void CountCollected(CollectableIngredient c) { collected++; }
    void MarkCrafted() { potionCrafted = true; }
    void MarkGrimoireCollected() { grimoireCollected = true; }
    void PlaceholderUsed(TutorialInteractable i) { if (i == grimoirePlaceholder) grimoireCollected = true; }

    IEnumerator Start()
    {
        ResolveReferences();

        // Coming back from the cave? tutorial has been done already.
        if (TutorialEvents.TutorialCompleted && !runEvenIfCompleted) yield break;

        // Block the cave entrance until Tuna leads the player there.
        if (caveEntrance != null) caveEntrance.enabled = false;

        yield return new WaitForSeconds(1f);

        // ---- 1. MIRROR ----
        yield return Dlg.Say("Petunia", "First day on the job! I should head downstairs and see what Tuna is up to.", 3.5f);
        Dlg.Hide();
        Ui.SetHint("WASD to move");
        yield return new WaitUntil(() => Input.GetAxis("Horizontal") != 0f || Input.GetAxis("Vertical") != 0f);
        yield return new WaitForSeconds(2f);
        Ui.ClearHint();

        // ---- 2. TUNA INTRO + GRIMOIRE ----
        yield return WaitUntilNear(tuna.transform, 4f);
        yield return Dlg.Say("Tuna", "Hey Petunia! Are you excited to get started?");
        yield return Dlg.Say("Tuna", "First, grab your grimoire off the table. It's where all your potion recipes live!");
        Dlg.Hide();

        Highlight(grimoireHighlight, true);
        yield return new WaitUntil(() => grimoireCollected);
        Highlight(grimoireHighlight, false);

        yield return Dlg.Say("Tuna", "Perfect! Now press ESC to open it and see which ingredients you'll need.");
        Dlg.Hide();

        Ui.SetHint(grimoireOpenHint);
        yield return new WaitUntil(GamePaused);
        Ui.ClearHint();
        yield return new WaitUntil(() => !GamePaused()); // wait for player to close it again
        yield return new WaitForSeconds(0.5f);

        yield return Dlg.Say("Tuna", "Got it? Great! Head into the forest and gather those ingredients so I can show you how to use the cauldron.");
        Dlg.Hide();

        // ---- 3. LEAD OUTSIDE ----
        trail.SetTarget(forestSpot);
        yield return WaitUntilNear(forestSpot, 3f);
        trail.Clear();

        // ---- 4. FIRST BLEP ----
        yield return new WaitUntil(() => blep.IsAimingAtCollectable);
        Ui.SetHint("Press E to BLEP! Petunia's tongue grabs things for you");
        yield return new WaitUntil(() => collected >= 1);
        Ui.ClearHint();

        yield return new WaitUntil(() => collected >= ingredientsToCollect);

        // ---- 5. BACK INSIDE ----
        Ui.SetHint("Head back inside");
        trail.SetTarget(insideSpot);
        yield return WaitUntilNear(insideSpot, 3f);
        trail.Clear();
        Ui.ClearHint();

        // ---- 6. CRAFTING LESSON ----
        yield return Dlg.Say("Tuna", "Great! Those are some high quality ingredients. Now we can try and craft a potion!");
        Dlg.Hide();
        yield return tuna.GoTo(cauldronSpot);

        Highlight(cauldronHighlight, true);
        Ui.SetHint("Press E at the cauldron to start crafting");
        yield return new WaitUntil(InventoryOpen);
        Highlight(cauldronHighlight, false);

        Ui.SetHint("Click a recipe to craft your first potion");
        yield return new WaitUntil(() => potionCrafted);

        Ui.SetHint("Nice! Press TAB to close the menu");
        yield return new WaitUntil(() => !InventoryOpen());
        Ui.ClearHint();

        // ---- 7. GRIMOIRE REMINDER ----
        yield return Dlg.Say("Tuna", "Now that you've figured out how to craft potions, remember you can check your grimoire any time to see all your recipes!");
        Dlg.Hide();

        // ---- 8. TUNA LEADS TO THE CAVE ----
        yield return Dlg.Say("Tuna", "Great! Now that you know how to collect stuff and make potions, there's something I want to show you.");
        Dlg.Hide();
        yield return tuna.LeadAlong(caveRoute);

        // ---- 9. THE CAVE ----
        TutorialEvents.TutorialCompleted = true;   // remembered even after the cave scene loads
        if (caveEntrance != null) caveEntrance.enabled = true;
        yield return Dlg.Say("Tuna", "This is the cave! Go on in and see what you can find. Watch out for gnomes!");
        Dlg.Hide();
        Ui.SetHint("Enter the cave");
        // CaveTrigger loads the cave scene when player walks in, so the tutorial ends here.
        yield return new WaitForSeconds(6f);
        Ui.ClearHint();
        Debug.Log("Tutorial complete!");
    }

    // Fills in any empty slots automatically, so it still works if the player is created or moved between scenes.
    void ResolveReferences()
    {
        if (player == null)
        {
            GameObject p = GameObject.FindWithTag("Player");
            if (p != null) player = p.transform;
        }
        if (blep == null && player != null) blep = player.GetComponentInChildren<FrogBlepController>();
        if (blep == null) blep = FindAnyObjectByType<FrogBlepController>();
        if (trail != null && trail.player == null) trail.player = player;
        if (tuna != null && tuna.player == null) tuna.player = player;
    }

    bool GamePaused()
    {
        return GameManager.instance != null && GameManager.instance.IsPaused;
    }

    bool InventoryOpen()
    {
        return Inventory2.instance != null && Inventory2.instance.container.activeInHierarchy;
    }

    void Highlight(HoverHighlight h, bool on)
    {
        if (h != null) h.SetHighlight(on);
    }

    IEnumerator WaitUntilNear(Transform target, float distance)
    {
        yield return new WaitUntil(() => Vector3.Distance(player.position, target.position) < distance);
    }
}
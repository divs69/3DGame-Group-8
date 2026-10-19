using System;
using System.Collections;
using UnityEngine;

public class TutorialDirector : MonoBehaviour
{
    [Header("Scene references")]
    public Transform player;
    public FrogBlepController blep;
    public GuideTrail trail;
    public GuideNPC tuna;

    [Header("Tutorial objects")]
    public HoverHighlight grimoireHighlight;
    public TutorialInteractable grimoirePlaceholder; // OPTIONAL: for testing only, until the real grimoire pickup script exists
    public HoverHighlight cauldronHighlight;

    [Header("Places (empty objects you place in the scene)")]
    public Transform forestSpot;
    public Transform insideSpot;
    public Transform cauldronSpot;
    public Transform[] caveRoute;

    [Header("Cave")]
    public Collider caveEntrance;

    [Header("Settings")]
    public bool runEvenIfCompleted = false; // Tick this while testing to replay the tutorial
    public bool skipCraftingLesson = false;  // Tick this until the cauldron crafting is connected, so the tutorial can carry on
    public float talkDistance = 6f;    // How close the player must get to Tuna to start the conversation
    public float markerDistance = 3f;  // How close the player must get to the forest / inside markers
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
        EnableCave(); // never leave the cave blocked if this script stops for any reason
        CollectableIngredient.OnAnyCollected -= CountCollected;
        TutorialEvents.PotionCrafted -= MarkCrafted;
        TutorialEvents.GrimoireCollected -= MarkGrimoireCollected;
        TutorialInteractable.OnUsed -= PlaceholderUsed;
    }
    void CountCollected(CollectableIngredient c) { collected++; }
    void MarkCrafted() { potionCrafted = true; }
    void MarkGrimoireCollected()
    {
        Debug.Log("[Tutorial] Grimoire picked up (real pickup script)");
        grimoireCollected = true;
    }
    void PlaceholderUsed(TutorialInteractable i)
    {
        // If the Grimoire Placeholder slot is empty, accept any TutorialInteractable (there's only one).
        if (grimoirePlaceholder == null || i == grimoirePlaceholder)
        {
            Debug.Log("[Tutorial] Grimoire picked up (placeholder: " + i.name + ")");
            grimoireCollected = true;
        }
        else
        {
            Debug.LogWarning("[Tutorial] Used " + i.name + ", but the Grimoire Placeholder slot holds a different object (" + grimoirePlaceholder.name + ").");
        }
    }

    IEnumerator Start()
    {
        ResolveReferences();

        // Coming back from the cave? The tutorial has already been done.
        if (TutorialEvents.TutorialCompleted && !runEvenIfCompleted) yield break;

        if (!ReferencesOk())
        {
            Debug.LogError("[Tutorial] Setup problem found (see messages above), so the tutorial was cancelled and the cave is left open.");
            yield break;
        }

        // Block the cave entrance until Tuna leads the player there.
        if (caveEntrance != null) caveEntrance.enabled = false;

        yield return new WaitForSeconds(1f);

        // ---- 1. MIRROR ----
        Step("1: Mirror");
        yield return Dlg.Say("Petunia", "First day on the job! I should head downstairs and see what Tuna is up to.", 3.5f);
        Dlg.Hide();
        Ui.SetHint("WASD to move");
        yield return new WaitUntil(() => Input.GetAxis("Horizontal") != 0f || Input.GetAxis("Vertical") != 0f);
        yield return new WaitForSeconds(2f);
        Ui.ClearHint();

        // ---- 2. TUNA INTRO + GRIMOIRE ----
        Step("2: Tuna Intro + Grimoire");
        yield return WaitUntilNear(tuna.transform, talkDistance);
        yield return Dlg.Say("Tuna", "Hey Petunia! Are you excited to get started?");
        yield return Dlg.Say("Tuna", "First, grab your grimoire off the table. It's where all your potion recipes live!");
        Dlg.Hide();

        Highlight(grimoireHighlight, true);
        if (!grimoireCollected) Debug.Log("[Tutorial] Waiting for the grimoire to be picked up...");
        yield return new WaitUntil(() => grimoireCollected);
        Highlight(grimoireHighlight, false);

        yield return Dlg.Say("Tuna", "Perfect! Now press ESC to open it and see which ingredients you'll need.");
        Dlg.Hide();

        Ui.SetHint(grimoireOpenHint);
        yield return new WaitUntil(GamePaused);
        Ui.ClearHint();
        yield return new WaitUntil(() => !GamePaused()); // wait for them to close it again
        yield return new WaitForSeconds(0.5f);

        yield return Dlg.Say("Tuna", "Got it? Great! Head into the forest and gather those ingredients so I can show you how to use the cauldron.");
        Dlg.Hide();

        // ---- 3. LEAD OUTSIDE ----
        Step("3: Lead Outside");
        trail.SetTarget(forestSpot);
        yield return WaitUntilNear(forestSpot, markerDistance);
        trail.Clear();

        // ---- 4. FIRST BLEP ----
        Step("4: First Blep");
        yield return new WaitUntil(() => blep.IsAimingAtCollectable);
        Ui.SetHint("Press E to BLEP! Petunia's tongue grabs things for you");
        yield return new WaitUntil(() => collected >= 1);
        Ui.ClearHint();

        yield return new WaitUntil(() => collected >= ingredientsToCollect);

        // ---- 5. BACK INSIDE ----
        Step("5: Back Inside");
        Ui.SetHint("Head back inside");
        trail.SetTarget(insideSpot);
        yield return WaitUntilNear(insideSpot, markerDistance);
        trail.Clear();
        Ui.ClearHint();

        // ---- 6. CRAFTING LESSON ----
        Step("6: Crafting Lesson");
        yield return Dlg.Say("Tuna", "Great! Those are some high quality ingredients. Now we can try and craft a potion!");
        Dlg.Hide();
        yield return tuna.GoTo(cauldronSpot);

        if (!skipCraftingLesson)
        {
            Highlight(cauldronHighlight, true);
            Ui.SetHint("Press TAB at the cauldron to start crafting");
            yield return new WaitUntil(InventoryOpen);
            Highlight(cauldronHighlight, false);

            Ui.SetHint("Click a the ingredients to craft your first potion");
            yield return new WaitUntil(() => potionCrafted);

            Ui.SetHint("Nice! Press TAB again to close the menu");
            yield return new WaitUntil(() => !InventoryOpen());
            Ui.ClearHint();
        }

        // ---- 7. GRIMOIRE REMINDER ----
        Step("7: Grimoire Reminder");
        yield return Dlg.Say("Tuna", "Now that you've figured out how to craft potions, remember you can check your grimoire any time to see all your recipes!");
        Dlg.Hide();

        // ---- 8. TUNA LEADS TO THE CAVE ----
        Step("8: Tuna Leads To The Cave");
        yield return Dlg.Say("Tuna", "Alrighty. Now that you know how to collect stuff and make potions, there's something I want to show you...");
        Dlg.Hide();
        yield return tuna.LeadAlong(caveRoute);

        // ---- 9. THE CAVE ----
        Step("9: The Cave");
        TutorialEvents.TutorialCompleted = true;   // remembered even after the cave scene loads
        if (caveEntrance != null) caveEntrance.enabled = true;
        yield return Dlg.Say("Tuna", "This here is the cave. I wont be joining you past this point... I'm too scared. ");
        yield return Dlg.Say("Tuna", "Go on in and see what you can find. Goodluck.");
        Dlg.Hide();
        Ui.SetHint("Enter the cave");

        yield return new WaitForSeconds(6f);
        Ui.ClearHint();
        Debug.Log("Tutorial complete!");
    }

    void Update()
    {
        // Developer shortcut: press left alt to skip the tutorial and unlock everything.
        if (Input.GetKeyDown(KeyCode.LeftAlt)) SkipTutorial();
    }

    void SkipTutorial()
    {
        Debug.Log("[Tutorial] Skipped with left alt");
        StopAllCoroutines();
        TutorialEvents.TutorialCompleted = true;
        EnableCave();
        if (Ui != null) Ui.ClearHint();
        if (Dlg != null) Dlg.Hide();
        if (trail != null) trail.Clear();
        Highlight(grimoireHighlight, false);
        Highlight(cauldronHighlight, false);
    }

    void EnableCave()
    {
        if (caveEntrance != null) caveEntrance.enabled = true;
    }

    void Step(string message)
    {
        Debug.Log("[Tutorial] Step " + message);
    }

    bool Need(UnityEngine.Object thing, string name)
    {
        if (thing != null) return true;
        Debug.LogError("[Tutorial] Missing: " + name);
        return false;
    }

    bool ReferencesOk()
    {
        bool ok = true;
        ok &= Need(player, "Player (no object with FPController or the Player tag was found)");
        ok &= Need(blep, "Blep (no FrogBlepController found)");
        ok &= Need(tuna, "Tuna slot (drag Tuna in)");
        ok &= Need(trail, "Trail slot (drag the GuideTrail object in)");
        ok &= Need(Dlg, "DialogueUI (is the TutorialUI object in the scene, with its slots filled?)");
        ok &= Need(Ui, "PromptUI (is it on the TutorialUI object?)");
        ok &= Need(forestSpot, "Forest Spot marker");
        ok &= Need(insideSpot, "Inside Spot marker");
        ok &= Need(cauldronSpot, "Cauldron Spot marker");
        if (caveRoute == null || caveRoute.Length == 0)
        {
            Debug.LogError("[Tutorial] Missing: Cave Route (set its Size and drag in the points)");
            ok = false;
        }
        return ok;
    }

    // Fills in any empty slots automatically, so it still works if the player is created or moved between scenes.
    void ResolveReferences()
    {
        // The object with FPController is the one that actually walks around, so measure from that.
        FPController fp = null;
        if (Camera.main != null) fp = Camera.main.GetComponentInParent<FPController>();
        if (fp == null) fp = FindAnyObjectByType<FPController>();
        if (fp != null) player = fp.transform;

        FPController[] allPlayers = FindObjectsByType<FPController>(FindObjectsSortMode.None);
        if (allPlayers.Length > 1)
            Debug.LogWarning("[Tutorial] Found " + allPlayers.Length + " players (FPController). " +
                             "There should only be one. This is the duplicate-player problem from DontDestroy.");
        if (player == null)
        {
            GameObject p = GameObject.FindWithTag("Player");
            if (p != null) player = p.transform;
        }
        if (player != null) Debug.Log("[Tutorial] Measuring distances from: " + player.name);
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

    float DistanceTo(Transform target)
    {
        Renderer[] renderers = target.GetComponentsInChildren<Renderer>();
        if (renderers.Length == 0) return Vector3.Distance(player.position, target.position);

        Bounds b = renderers[0].bounds;
        for (int i = 1; i < renderers.Length; i++) b.Encapsulate(renderers[i].bounds);
        return Vector3.Distance(player.position, b.ClosestPoint(player.position));
    }

    IEnumerator WaitUntilNear(Transform target, float distance)
    {
        float nextLog = 0f;
        while (DistanceTo(target) >= distance)
        {
            if (Time.time >= nextLog)
            {
                nextLog = Time.time + 3f;
                Debug.Log("[Tutorial] Waiting: " + player.name + " at " + player.position +
                          " is " + Mathf.Round(DistanceTo(target)) + " from " + target.name +
                          " at " + target.position + " (needs to be under " + distance + ")");
            }
            yield return null;
        }
    }
}
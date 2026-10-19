using System;

public static class TutorialEvents
{
    public static Action PotionCrafted;     // call when a potion is successfully crafted (Inventory2.Craft)
    public static Action GrimoireCollected; // call when player picks the grimoire up off the table

    // Stays true across scene loads, so the tutorial doesn't replay when player returns from the cave.
    public static bool TutorialCompleted;
}
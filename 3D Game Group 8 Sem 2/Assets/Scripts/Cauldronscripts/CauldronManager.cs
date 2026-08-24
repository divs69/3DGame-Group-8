using UnityEngine;
using UnityEngine.UI;


public class CauldronManager : MonoBehaviour
{
    public static CauldronManager Instance;
    [Header("Cauldron Slots")]
    public SlotsCauldron[] slots;
    [Header("Recipe Result")]
    public Image resultimage;
    [Header("Result Sprite")]
    public Sprite resultsprite;

    public void Awake()
    {
        Instance = this;
        resultimage.enabled = false;

    }

    public void CheckRecipe()
    {
        foreach (SlotsCauldron slot in slots)
        {
            if (!slot.isfilled)
            {
                return;

            }
        }

        CreatedResult();

        
    }

    private void CreatedResult()
    {
        Debug.Log("Recipe Complete");
        resultimage.sprite = resultsprite;
        resultimage.enabled = true;

        foreach (SlotsCauldron slot in slots)
        {
            slot.slotimage.enabled = false;
        }

    }
}

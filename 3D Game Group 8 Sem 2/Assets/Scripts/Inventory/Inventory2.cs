using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;



public class Inventory2 : MonoBehaviour
{
    public static Inventory2 instance;
    private bool isPaused;
    public FPController FPControllerScript;

    //objects in game world. collectables 
    public IngredientData MushroomItem;
    public IngredientData CrystalItem;
    public IngredientData BeehiveItem;
    public IngredientData FlowerItem;
    public IngredientData Flower1Item;
    public IngredientData FungusItem;
    public IngredientData Eyeballs;
    public IngredientData SpeedpotiongreenItem;
    public IngredientData ReachpotionblueItem;
    public IngredientData JumppotionpinkItem;


    //Slot Containers
    public GameObject hotbarObj;
    public GameObject inventorySlotParent;
    public GameObject container;
    public GameObject craftingMenu;

    public Image dragIcon;

    //PickUp
    public float pickupRange;
    public Material highlightMaterial;
    private Material originalMaterial;
    private Renderer lookedAtRenderer = null;

    private Slot draggedSlot = null;
    private readonly List<RaycastResult> pointerHits = new();
    private bool nearCauldron;
    public bool IsOpen => container.activeInHierarchy;

    private int equippedHotBarIndex = 0; //0 - 5 numpad keys in the hotbar
    public float equippedOpacity = 0.9f;
    public float normalOpacity = 0.58f;
    
    //Item Descriptions
    public GameObject itemdescriptionParent;
    public Image itemdescriptionimage;
    public TextMeshProUGUI descriptionitemNameTxt;
    public TextMeshProUGUI itemdescriptionTxt;

    //crafting 
    public List<Recipes> allRecipes = new List<Recipes>();
    public Transform craftingGrid;
    public GameObject craftingBTNprefab;
    public GameObject itemneededUIprefab;

    //slot lists
    private List<Slot> inventorySlots = new List<Slot>();
    private List<Slot> hotbarSlots = new List<Slot>();
    private List<Slot> allSlots = new List<Slot>();

    private void Awake()
    {
        instance = this;
        inventorySlots.AddRange(inventorySlotParent.GetComponentsInChildren<Slot>(true));
        hotbarSlots.AddRange(hotbarObj.GetComponentsInChildren<Slot>(true));

        allSlots.AddRange(inventorySlots);
        allSlots.AddRange(hotbarSlots);

        dragIcon.raycastTarget = false;
        CancelDrag();
        craftingMenu.SetActive(false);
        PopulateCraftingGrid();
    }
    void Update()
    {

        if (Time.timeScale == 0f)
            return;

        if (Input.GetKeyDown(KeyCode.Tab))
            SetInventoryOpen(!IsOpen);

        if (IsOpen)
        {
            StartDrag();
            UpdateDragItemPosition();
            EndDrag();
            UpdateItemDescription();
        }
        else
        {
            DetectLookedAtItem();
            Pickup();
            HandleHotBarSelection();
            HandleDropEquippedItem();
        }

        UpdateHotBarOpacity();
    }

    public void ToggleInventory()
    {
        if (Time.timeScale != 0f)
            SetInventoryOpen(!IsOpen);
    }

    public void SetInventoryOpen(bool open)
    {
        CancelDrag();
        itemdescriptionParent.SetActive(false);
        container.SetActive(open);
        craftingMenu.SetActive(open && nearCauldron);
        Cursor.lockState = open ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = open;

        isPaused = true;

        // Disable mouse look script
        if (FPControllerScript != null)
        {
            FPControllerScript.enabled = false;
        }

       
        
            
        

    }

    public void SetCauldronInRange(bool inRange)
    {
        nearCauldron = inRange;
        if (inRange && Time.timeScale != 0f)
            SetInventoryOpen(true);
        else
            craftingMenu.SetActive(false);
    }

    public bool Additem(IngredientData itemToAdd, int amount)
    {
        if (itemToAdd == null || amount <= 0)
            return false;

        int freeSlots = 0;
        foreach (Slot slot in allSlots)
        {
            if (!slot.HasItem())
                freeSlots++;
        }

        if (freeSlots < amount)
            return false;

        foreach (Slot slot in allSlots)
        {
            if (slot.HasItem())
                continue;

            slot.SetItem(itemToAdd);
            if (--amount == 0)
                break;
        }

        RefreshCraftingButtons();
        return true;
    }

    private void StartDrag()
    {
        if (!Input.GetMouseButtonDown(0))
            return;

        Slot hovered = GetPointerSlot();
        if (hovered == null || !hovered.HasItem())
            return;

        draggedSlot = hovered;
        dragIcon.sprite = hovered.GetItem().itemIcon;
        dragIcon.color = new Color(1, 1, 1, 0.5f);
        dragIcon.enabled = true;
    }

    private void EndDrag()
    {
        if (Input.GetMouseButtonUp(0) && draggedSlot != null)
        {
            Slot hovered = GetPointerSlot();

            if (hovered != null)
                HandleDrop(draggedSlot, hovered);

            CancelDrag();
        }
    }

    private void CancelDrag()
    {
        draggedSlot = null;
        dragIcon.enabled = false;
        dragIcon.sprite = null;
    }

    private void OnApplicationFocus(bool focused)
    {
        if (!focused)
            CancelDrag();
    }

    private Slot GetPointerSlot()
    {
        EventSystem eventSystem = EventSystem.current;
        if (eventSystem == null)
            return null;

        var pointer = new PointerEventData(eventSystem) { position = Input.mousePosition };
        eventSystem.RaycastAll(pointer, pointerHits);
        if (pointerHits.Count == 0)
            return null;

        Slot slot = pointerHits[0].gameObject.GetComponentInParent<Slot>();
        return slot != null && slot.isActiveAndEnabled && allSlots.Contains(slot) ? slot : null;
    }

    private Slot GetHoveredSlot()
    {
        foreach (Slot s in allSlots)
        {
            if (s.isActiveAndEnabled && s.hovering)
                return s;
        }

        return null;
    }

    private void HandleDrop(Slot from, Slot to)
    {
        if (from == to) return;

        //Different item
        if (to.HasItem())
        {
            IngredientData tempitem = to.GetItem();
            int tempAmount = to.GetAmount();

            to.SetItem(from.GetItem(), from.GetAmount());
            from.SetItem(tempitem, tempAmount);
            return;

        }

        //empty slot
        to.SetItem(from.GetItem(), from.GetAmount());
        from.ClearSlot();
    }

    private void UpdateDragItemPosition()
    {
        if (draggedSlot != null)
        {
            dragIcon.transform.position = Input.mousePosition;
        }
    }

    private void Pickup()
    {
        if (lookedAtRenderer != null && Input.GetKeyDown(KeyCode.E))
        {
            Ingredients ingredient = lookedAtRenderer.GetComponent<Ingredients>();
            if (ingredient != null && !ingredient.TryGetComponent<ICollectable>(out _) &&
                Additem(ingredient.ingredient, ingredient.amount))
            {
                Destroy(ingredient.gameObject);
                Debug.Log("picked up item");
            }
        }
    }
    private void DetectLookedAtItem()
    {
        if (lookedAtRenderer != null)
        {
            lookedAtRenderer.material = originalMaterial;
            lookedAtRenderer = null;
            originalMaterial = null;
        }

        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, pickupRange))
        {

            Ingredients ingredient = hit.collider.GetComponent<Ingredients>();
            if (ingredient != null)
            {
                Renderer rend = ingredient.GetComponent<Renderer>();
                if (rend != null)
                {
                    originalMaterial = rend.material;
                    rend.material = highlightMaterial;
                    lookedAtRenderer = rend;
                }
            }

        }
    }

    private void UpdateHotBarOpacity()
    {
        for (int i = 0; i < hotbarSlots.Count; i++)
        {
            Image icon = hotbarSlots[i].GetComponent<Image>();
            if (icon != null)
            {
                icon.color = (i == equippedHotBarIndex) ? new Color(1, 1, 1, equippedOpacity) : new Color(1, 1, 1, normalOpacity);
            }
        }
    }

    private void HandleHotBarSelection()
    {
        for (int i = 0; i < Mathf.Min(6, hotbarSlots.Count); i++)
        {
            if (Input.GetKeyDown((i + 1).ToString()))
            {
                equippedHotBarIndex = i;
                UpdateHotBarOpacity();
            }

        }
    }

    private void HandleDropEquippedItem()
    {
        if (!Input.GetKeyDown(KeyCode.Q) || equippedHotBarIndex >= hotbarSlots.Count) return;

        Slot equippedSlot = hotbarSlots[equippedHotBarIndex];

        if (!equippedSlot.HasItem()) return;

        IngredientData ingredientData = equippedSlot.GetItem();
        GameObject prefab = ingredientData.itemPrefab;

        if (prefab == null) return;

        //spawn
        GameObject dropped = Instantiate(prefab, Camera.main.transform.position + Camera.main.transform.forward, Quaternion.identity);

        Ingredients ingredient = dropped.GetComponent<Ingredients>();
        ingredient.ingredient = ingredientData;
        ingredient.amount = equippedSlot.GetAmount();

        equippedSlot.ClearSlot(); // remove

        RefreshCraftingButtons();
    }

    private void UpdateItemDescription()
    {
        Slot hoveredSlot =GetHoveredSlot();

        if (hoveredSlot != null)
        {
            IngredientData hoveredItem = hoveredSlot.GetItem();

            if (hoveredItem != null)
            {
                itemdescriptionParent.SetActive(true);
                itemdescriptionimage.sprite = hoveredItem.itemIcon;
                itemdescriptionTxt.text = hoveredItem.description;
                descriptionitemNameTxt.text = hoveredItem.name;
                return;
            }
        }
        itemdescriptionParent.SetActive(false);
    }

    private readonly List<(Button button, Recipes recipe)> craftingButtons = new();

    private void RefreshCraftingButtons()
    {
        foreach (var (button, recipe) in craftingButtons)
            button.interactable = CanCraft(recipe);
    }

    private void PopulateCraftingGrid()
    {
        for (int i = craftingGrid.childCount - 1; i >= 0; i--)
        {
            Destroy(craftingGrid.GetChild(i).gameObject);
        }

        foreach(Recipes recipes in allRecipes)
        {
            GameObject btnObj = Instantiate(craftingBTNprefab, craftingGrid);

            Image img = btnObj.transform.GetChild(0).GetComponent<Image>();
            img.sprite = recipes.result.itemIcon;

            Button btn = btnObj.GetComponent<Button>();

            craftingButtons.Add((btn, recipes));
            btn.interactable = CanCraft(recipes);
            btn.onClick.AddListener(() => Craft(recipes));

            foreach (Elements elements in recipes.elements)
            {
                GameObject neededItem = Instantiate(itemneededUIprefab, btnObj.transform.GetChild(1));
                neededItem.GetComponent<Image>().sprite = elements.ingredient.itemIcon;
                neededItem.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "x" + elements.amount.ToString();
            }
        }
    }

    public void Craft(Recipes recipes)
    {
        if (!IsOpen || !nearCauldron || !CanCraft(recipes))
        {
            return;
        }

        ConsumeElements(recipes);
        Additem(recipes.result, recipes.resultAmount);
        TutorialEvents.PotionCrafted?.Invoke();
    }

    private void ConsumeElements(Recipes recipes)
    {
        foreach (Elements elements in recipes.elements)
        {
            int remaining = elements.amount;

            foreach (Slot slot in allSlots)
            {
                if (!slot.HasItem()) continue;
                if (slot.GetItem() != elements.ingredient) continue;

                int take = Mathf.Min(slot.GetAmount(), remaining);
                slot.RemoveAmount(take);

                remaining -= take;
                if (remaining <= 0)
                    break;

            }

        }

    }


    public bool CanCraft(Recipes recipes)
    {
        if (recipes == null || recipes.result == null || recipes.resultAmount <= 0)
            return false;

        var needed = new Dictionary<IngredientData, int>();
        foreach (Elements elements in recipes.elements)
        {
            if (elements.ingredient == null || elements.amount <= 0)
                return false;

            needed.TryGetValue(elements.ingredient, out int amount);
            needed[elements.ingredient] = amount + elements.amount;
        }

        int freeSlots = 0;
        foreach (Slot slot in allSlots)
        {
            if (!slot.HasItem())
            {
                freeSlots++;
                continue;
            }

            IngredientData item = slot.GetItem();
            if (!needed.TryGetValue(item, out int remaining))
                continue;

            int consumed = Mathf.Min(remaining, slot.GetAmount());
            if (consumed == slot.GetAmount())
                freeSlots++;

            needed[item] = remaining - consumed;
        }

        foreach (int remaining in needed.Values)
        {
            if (remaining > 0)
                return false;
        }

        return freeSlots >= recipes.resultAmount;
    }
}

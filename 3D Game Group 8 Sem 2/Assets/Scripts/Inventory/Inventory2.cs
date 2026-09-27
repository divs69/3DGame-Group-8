using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;



public class Inventory2 : MonoBehaviour
{
    public static Inventory2 instance;

    //objects in game world. collectables 
    public IngredientData MushroomItem;
    public IngredientData CrystalItem;
    public IngredientData BeehiveItem;
    public IngredientData FlowerItem;
    public IngredientData Flower1Item;
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
    private Ingredients lookedAtItem = null;
    public Material highlightMaterial;
    private Material originalMaterial;
    private Renderer lookedAtRenderer = null;

    private Slot draggedSlot = null;
    private bool isDragging = false;

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
        inventorySlots.AddRange(inventorySlotParent.GetComponentsInChildren<Slot>());
        hotbarSlots.AddRange(hotbarObj.GetComponentsInChildren<Slot>());

        allSlots.AddRange(inventorySlots);
        allSlots.AddRange(hotbarSlots);

        PopulateCraftingGrid();
    }
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Tab))
        {

            ToggleInventory();

        }

        DetectLookedAtItem();

        Pickup();

        StartDrag();
        UpdateDragItemPosition();
        EndDrag();

        HandleHotBarSelection();
        HandleDropEquippedItem();
        UpdateHotBarOpacity();

        UpdateItemDescription();
    }

    public void ToggleInventory()
    {
        container.SetActive(!container.activeInHierarchy);
        Cursor.lockState = Cursor.lockState == CursorLockMode.Locked ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = !Cursor.visible;

        craftingMenu.SetActive(true);
    }

    public void Additem(IngredientData itemToAdd, int amount)
    {
        int remaining = amount;

        //try to fill existing stacks first
        foreach (Slot slot in allSlots)
        {
            if (slot.HasItem() && slot.GetItem() == itemToAdd)
            {
                int currentAmount = slot.GetAmount();
                int maxStack = itemToAdd.maxStackSize;

                if (currentAmount < maxStack)
                {
                    int spaceLeft = maxStack - currentAmount;
                    int amountToAdd = Mathf.Min(spaceLeft, remaining);

                    slot.SetItem(itemToAdd, currentAmount + amountToAdd);
                    remaining -= amountToAdd;

                    if (remaining <= 0)
                    {
                        PopulateCraftingGrid();
                        return; // done adding everything
                    }
                    
                }
            }
        }

        //if items remian,put them in empty slots 
        foreach (Slot slot in allSlots)
        {
            if (!slot.HasItem())
            {
                int amountToPlace = Mathf.Min(itemToAdd.maxStackSize, remaining);
                slot.SetItem(itemToAdd, amountToPlace);
                remaining -= amountToPlace;

                if (remaining <= 0)
                    PopulateCraftingGrid();
                return; // done adding everything
            }
        }

        if (remaining > 0)
        {
            Debug.Log("inventory is full, could not add item" + remaining + "of" + itemToAdd.itemName);
        }
        PopulateCraftingGrid();
    }

    private void StartDrag()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Slot hovered = GetHoveredSlot();

            if (hovered != null && hovered.HasItem())
            {
                draggedSlot = hovered;
                isDragging = true;

                //show drag item
                dragIcon.sprite = hovered.GetItem().itemIcon;
                dragIcon.color = new Color(1, 1, 1, 0.5f);
                dragIcon.enabled = true;

            }
        }
    }

    private void EndDrag()
    {
        if (Input.GetMouseButtonUp(0) && isDragging)
        {
            Slot hovered = GetHoveredSlot();

            if (hovered != null)
            {
                HandleDrop(draggedSlot, hovered);

                dragIcon.enabled = false;

                draggedSlot = null;
                isDragging = false;
            }
        }
    }

    private Slot GetHoveredSlot()
    {
        foreach (Slot s in allSlots)
        {
            if (s.hovering)
                return s;
        }

        return null;
    }

    private void HandleDrop(Slot from, Slot to)
    {
        if (from == to) return;

        //Stacking
        if (to.HasItem() && to.GetItem() == from.GetItem())
        {
            int max = to.GetItem().maxStackSize;
            int space = max - to.GetAmount();

            if (space > 0)
            {
                int move = Mathf.Min(space, from.GetAmount());
                to.SetItem(to.GetItem(), to.GetAmount() + move);
                from.SetItem(from.GetItem(), from.GetAmount() - move);

                if (from.GetAmount() <= 0)
                    from.ClearSlot();

                return;
            }
        }

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
        if (isDragging)
        {
            dragIcon.transform.position = Input.mousePosition;
        }
    }

    private void Pickup()
    {
        if (lookedAtRenderer != null && Input.GetKeyDown(KeyCode.E))
        {
            Ingredients ingredient = lookedAtRenderer.GetComponent<Ingredients>();
            if (ingredient != null)
            {
                Additem(ingredient.ingredient, ingredient.amount);
                Destroy(ingredient.gameObject);

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
        for (int i = 0; i < 6; i++)
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
        if(!Input.GetKeyDown(KeyCode.Q))  return;

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

        PopulateCraftingGrid();
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
            itemdescriptionParent.SetActive(false);
        }
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

            btn.interactable = CanCraft(recipes);
            btn.onClick.RemoveAllListeners();
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
        if (!CanCraft(recipes))
        {
            return;
        }

        ConsumeElements(recipes);
        Additem(recipes.result, recipes.resultAmount);
        PopulateCraftingGrid();
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
                slot.SetItem(slot.GetItem(), slot.GetAmount() - take);

                if(slot.GetAmount() <= 0)
                    slot.ClearSlot();

                remaining -= take;
                if (remaining <= 0)
                    break;

            }

        }

    }


    public bool CanCraft(Recipes recipes)
    {
        foreach (Elements elements in recipes.elements)
        {
            int totalFound = 0;

            foreach (Slot slot in allSlots)
            {
                if(slot.HasItem() && slot.GetItem() == elements.ingredient)
                {
                    totalFound += slot.GetAmount();

                }
            }

            if(totalFound < elements.amount)
                return false;

        }
        return true;

    }
}
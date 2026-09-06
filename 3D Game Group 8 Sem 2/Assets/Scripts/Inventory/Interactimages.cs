using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Interactimages : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler

    
{
    public string itemName;
    private Canvas canvas;
    private CanvasGroup canvasgroup;
    private RectTransform rectTransform;
    private Vector2 originalposition;
    private Transform originalparent;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasgroup = GetComponent<CanvasGroup>();
        canvas = GetComponentInParent<Canvas>();

        if (canvasgroup == null)
        {
            canvasgroup = gameObject.AddComponent<CanvasGroup>();

        }

       
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        originalposition = rectTransform.anchoredPosition;
        originalparent = transform.parent;
        transform.SetParent(canvas.transform);
        canvasgroup.blocksRaycasts = false;

    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.position = eventData.position;

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasgroup.blocksRaycasts = true;

        if (transform.parent == canvas.transform)
        {
            transform.SetParent(originalparent);
            rectTransform.anchoredPosition = originalposition;
        }
    
    
    }

}

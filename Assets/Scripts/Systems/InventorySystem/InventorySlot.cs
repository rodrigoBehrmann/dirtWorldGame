

using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour, IDropHandler
{    
    public ItemType[] SlotType;

    public void OnDrop(PointerEventData eventData)
    {
        if (transform.childCount == 0)
        {
            InventoryItemSlot itemSlot = eventData.pointerDrag.GetComponent<InventoryItemSlot>();
            if (itemSlot != null && SlotType.Contains(itemSlot.ItemType))
            {
                itemSlot.ParentAfterDrag = transform;
                itemSlot.transform.SetParent(transform);
                itemSlot.transform.localPosition = Vector3.zero;
            }
        }
    }
}

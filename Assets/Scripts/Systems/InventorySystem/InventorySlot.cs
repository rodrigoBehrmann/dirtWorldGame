

using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour, IDropHandler
{    
    public ItemType[] ItemType;
    public ItemCategory[] ItemCategory;

    public void OnDrop(PointerEventData eventData)
    {
        if (transform.childCount == 0)
        {
            InventoryItemSlot itemSlot = eventData.pointerDrag.GetComponent<InventoryItemSlot>();

            if (ItemCategory.Length > 0)
            {
                if (itemSlot != null && ItemCategory.Contains(itemSlot.ItemCategory))
                {
                    itemSlot.ParentAfterDrag = transform;
                    itemSlot.transform.SetParent(transform);
                    itemSlot.transform.localPosition = Vector3.zero;
                }
            }else if (itemSlot != null && ItemType.Contains(itemSlot.ItemType))
            {
                itemSlot.ParentAfterDrag = transform;
                itemSlot.transform.SetParent(transform);
                itemSlot.transform.localPosition = Vector3.zero;
            }            
        }
    }
}

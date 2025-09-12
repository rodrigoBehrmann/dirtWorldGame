using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCustomizationController : MonoBehaviour
{
    [Header("Player Mesh Settings")]
    [SerializeField] private SkinnedMeshRenderer _headMesh;
    [SerializeField] private SkinnedMeshRenderer _bodyMesh;
    [SerializeField] private SkinnedMeshRenderer _legsMesh;
    [SerializeField] private SkinnedMeshRenderer _feetMesh;

    [Header("Player Customization Settings")]
    [SerializeField] private List<PlayerCustomization> _headItems = new List<PlayerCustomization>();
    [SerializeField] private List<PlayerCustomization> _bodyItems = new List<PlayerCustomization>();
    [SerializeField] private List<PlayerCustomization> _legsItems = new List<PlayerCustomization>();
    [SerializeField] private List<PlayerCustomization> _feetItems = new List<PlayerCustomization>();    


    private void OnEnable()
    {
        EventBus.Subscribe<PlayerCustomizationEvent>(OnPlayerCustomization);
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe<PlayerCustomizationEvent>(OnPlayerCustomization);
    }

    private void OnPlayerCustomization(PlayerCustomizationEvent evt)
    {
        switch (evt.ItemCategory)
        {
            case ItemCategory.Hair:
                EquipItem(_headItems, evt.ItemName, evt.ItemCategory);
                break;
            case ItemCategory.Shirt:
                EquipItem(_bodyItems, evt.ItemName, evt.ItemCategory);
                break;
            case ItemCategory.Pant:
                EquipItem(_legsItems, evt.ItemName, evt.ItemCategory);
                break;
            case ItemCategory.Shoes:
                EquipItem(_feetItems, evt.ItemName, evt.ItemCategory);
                break;
            default:
                break;
        }
    }

    private void EquipItem(List<PlayerCustomization> items, string itemName, ItemCategory itemCategory)
    {
        foreach (var item in items)
        {
            if (item.ItemName == itemName && item.ItemCategory == itemCategory)
            {
                switch (itemCategory)
                {
                    case ItemCategory.Hair:
                        _headMesh.sharedMesh = item.ItemMesh;
                        break;
                    case ItemCategory.Shirt:
                        _bodyMesh.sharedMesh = item.ItemMesh;
                        break;
                    case ItemCategory.Pant:
                        _legsMesh.sharedMesh = item.ItemMesh;
                        break;
                    case ItemCategory.Shoes:
                        Debug.Log($"Equipping Shoes Mesh: {item.ItemMesh.name}");
                        _feetMesh.sharedMesh = item.ItemMesh;                        
                        break;
                    default:
                        break;
                }
                return;
            }
        }
    }
    

}

[Serializable]
public class PlayerCustomization
{
    public string ItemName;
    public ItemCategory ItemCategory;
    public Mesh ItemMesh;
}

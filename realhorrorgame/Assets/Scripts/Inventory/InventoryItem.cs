using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItem : Interactable
{
    [SerializeField] InventoryItemData data;

    public override void Interact()
    {
        Inventory.Instance.AddToInventory(data, gameObject);
    }

    public override string GetDescription()
    {
        return data.itemName;
    }
}

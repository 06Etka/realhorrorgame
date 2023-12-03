using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class InventoryItemData : ScriptableObject
{
    public string itemName;
    public int itemID;
    public int numberKeyIndex;
    public int itemSize;
    public GameObject handPrefab;
    public ItemType itemType;
    public enum ItemType
    {
        Backpack,
        Collectable,
        Holdable
    }
}

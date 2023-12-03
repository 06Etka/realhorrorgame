using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance { get; private set; }
    public List<InventoryItemData> inventory = new List<InventoryItemData>();
    [SerializeField] Transform itemHolder;
    bool hasBackpack;
    [SerializeField] int maxBackpackSize;
    int backpackSize;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        backpackSize = maxBackpackSize;
    }

    public void AddToInventory(InventoryItemData _data, GameObject _objToDestroy) 
    {
        inventory.Add(_data);
        switch (_data.itemType)
        {
            case InventoryItemData.ItemType.Backpack:
                hasBackpack = true;
                break;
            case InventoryItemData.ItemType.Collectable:
                if(!hasBackpack) { return; }
                if(backpackSize > _data.itemSize)
                {
                    backpackSize -= _data.itemSize;
                }
                break;
            case InventoryItemData.ItemType.Holdable:
                int numberIndex = _data.numberKeyIndex;
                Transform parent = itemHolder.GetChild(numberIndex);
                if (parent != null)
                {
                    GameObject item = Instantiate(_data.handPrefab, parent.localPosition, Quaternion.identity);
                    item.transform.parent = parent;
                    item.transform.localPosition = Vector3.zero;
                    /*if(parent.gameObject.activeInHierarchy)
                    {
                        item.SetActive(true);
                    } else { item.SetActive(false); }*/
                }
                break;
            default:
                break;
        }
        GetItemIndexInInventory(_data);
        Destroy(_objToDestroy, 0.15f);
    }

    public void GetItemIndexInInventory(InventoryItemData _data)
    {
        int index = inventory.FindIndex(d => d == _data);
        print(index);
    }
}

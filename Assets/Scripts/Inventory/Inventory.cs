using UnityEngine;
using System.Collections.Generic;


[System.Serializable]
public class Inventory
{
    public int maxWeight;
    public int totalValue;
    public int totalWeight;
    public Inventory invHolder;
    public bool autoLoot;
    public float autoLootTimer = 0f;
    private float _currentAutoLootTimer = 0f;
    public List<LootTable> invItems = new List<LootTable>();

    public void Start()
    {
      //  invHolder = GameObject.FindObjectOfType<Inventory>().inventory;
        _currentAutoLootTimer = autoLootTimer;
    }

    public void Update()
    {
        if (autoLoot && invHolder.invItems.Count > 0)
        {
            if (_currentAutoLootTimer <= 0)
            {
                AddLootToInventory();
                _currentAutoLootTimer = autoLootTimer;
            }
        }
        if (_currentAutoLootTimer > 0)
            _currentAutoLootTimer -= Time.deltaTime;
        else
            _currentAutoLootTimer = 0;
        
       
    }

    public void AddLootToInventory()
    {
        if(invHolder != null)
        {
            if (OverWeight())
            {
                if (SourceHasItems())
                {
                    LootTable lootTable = invHolder.invItems[0];
                    int count = lootTable.qty;
                    int qtyLeft = lootTable.qty;
                    for (int i = 1; i <= count; i++)
                    {
                        if (ItemOverweight(lootTable))
                        {

                            if (ItemInInventory(lootTable))
                            {
                                if (CanAddToItem(lootTable))
                                {
                                    LootTable invItem = ItemFound(lootTable);
                                    invItem.IncreaseQty(1);
                                }
                                else
                                {
                                    LootTable invItem = new LootTable(lootTable.item, 1);
                                    invItems.Add(invItem);
                                }
                            }
                            else
                            {
                                LootTable invItem = new LootTable(lootTable.item, 1);
                                invItems.Add(invItem);
                            }
                            lootTable.DecreaseQty(1);
                            qtyLeft--;
                        }
                        UpdateStats();
                        invHolder.UpdateStats();
                    }
                    if (qtyLeft <= 0)
                    {

                        Debug.Log("Finished Item");
                        invHolder.invItems.Remove(lootTable);
                    }
                }
            }
            else
            {
                autoLoot = false;
                Debug.Log("inventory full");
            }
        }
    }

    public bool ItemOverweight(LootTable lootTable)
    {
        int testWeight = totalWeight + lootTable.item.weight;
        if (testWeight <= maxWeight)
            return true;
        else
            return false;
    }

    public LootTable ItemFound(LootTable compItem)
    {
        foreach (LootTable invItem in invItems)
        {
            if ((compItem.item.id == invItem.item.id) && (invItem.item.stackable && invItem.qty < invItem.item.stackSize))
            {
                return invItem;
            }
        }
        return null;
    }

    public bool CanAddToItem(LootTable compItem)
    {
        foreach (LootTable invItem in invItems)
        {
            if ((compItem.item.id == invItem.item.id) && (invItem.item.stackable && invItem.qty < invItem.item.stackSize))
            {
                return true;
            }
        }
        return false;
    }

    public bool ItemInInventory(LootTable compItem)
    {
        foreach (LootTable invItem in invItems)
        {
            if (compItem.item.id == invItem.item.id)
                return true;
        }
        return false;
    }

    public void AddItemToInventory(LootTable lootTable, int qty)
    {
        lootTable.IncreaseQty(qty);
        invItems.Add(lootTable);
    }

    public bool OverWeight()
    {
        return (totalWeight <= maxWeight) ? true : false;
    }

    public void UpdateItemInInventory(LootTable invItem, int qty)
    {
        invItem.IncreaseQty(qty);
    }

    public bool InvetoryContains(int invID, int lootID)
    {
        return (invID == lootID) ? true : false;
    }

    public bool SourceHasItems()
    {

       return (invHolder.invItems.Count > 0) ? true : false;
    }

    public void UpdateStats()
    {
        int value = 0;
        int weight = 0;
        foreach (LootTable invItem in invItems)
        {
            value += (invItem.GetQty() * invItem.item.value);
            weight += (invItem.GetQty() * invItem.item.weight);
        }
        totalValue = value;
        totalWeight = weight;
    }
}
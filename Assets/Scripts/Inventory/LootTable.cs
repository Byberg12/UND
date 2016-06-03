using System;

[System.Serializable]
public class LootTable
{
    public ItemStruct item;
    public int qty;

    public LootTable(ItemStruct item, int qty)
    {
        this.item = item;
        this.qty = qty;
    }

    public void UpdateQty(int newQty)
    {
        this.qty = newQty;
    }
    public void IncreaseQty(int qty)
    {
        this.qty += qty;
    }

    public int GetQty()
    {
        return this.qty;
    }

    internal void DecreaseQty(int qty)
    {
        this.qty -= qty;
    }
}
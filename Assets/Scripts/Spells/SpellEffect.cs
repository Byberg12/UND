public enum SpellTypes
{
    Healing = 0,
    Damage = 1
    
}
[System.Serializable]
public class SpellEffect
{
    public string name;
    public SpellTypes spelltype;
    public int amount;
    public float duration;
}
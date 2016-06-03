using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public class Spell : MonoBehaviour
{
    public enum Types
    {
        Ground = 0,
        Target = 1
    }
    public int id;
    public new string name;
    public string icon;
    public Types type;
    public float castTime;
    public string resource;
    public string sParticles;
    public string dParticles;

    private Image SpellIcon;
    private Text SpellName;
    private Text SpellLevel;
    private bool isReady = false;

    public void Start()
    {


    }

    public void Update()
    {

    }

    public void setupSpell()
    {
        SpellName = transform.FindChild("SpellName").GetComponent<Text>();
        SpellIcon = transform.FindChild("Slot/SpellIcon").GetComponent<Image>();
        SpellLevel = transform.FindChild("Slot/SpellLevel").GetComponent<Text>();

        SpellIcon.sprite = Resources.Load<Sprite>("Sprites/Skills/" + icon);
        SpellLevel.text = "1";
        SpellName.text = name;
        isReady = true;
    }

    public void OnClick()
    {
        /*CursorSlot cursorSlot = FindObjectOfType<CursorSlot>();
        foreach (Transform child in cursorSlot.transform)
        {
            Destroy(child.gameObject);
        }
        GameObject cursorSpellGO = (GameObject)Instantiate(Resources.Load<GameObject>("Spells/SpellIcon"), Vector3.zero, Quaternion.identity);
        cursorSpellGO.GetComponent<Image>().raycastTarget = false;
        cursorSpellGO.transform.SetParent(cursorSlot.transform, false);
        Spell spell = cursorSpellGO.GetComponent<Spell>();
        spell.icon = this.icon;
        spell.setupIcon();
        */
    }

    private void setupIcon()
    {
        SpellIcon = transform.GetComponent<Image>();
        SpellIcon.sprite = Resources.Load<Sprite>("Sprites/Skills/" + icon);
    }
}

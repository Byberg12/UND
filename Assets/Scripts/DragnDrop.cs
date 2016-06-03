using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using System;

public class DragnDrop : MonoBehaviour,IDragHandler,IDropHandler,IBeginDragHandler,IEndDragHandler,IPointerDownHandler, IPointerUpHandler
{
    private Image SpellIcon;

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("OnBeginDrag");
        CursorSlot cursorSlot = FindObjectOfType<CursorSlot>();
        foreach (Transform child in cursorSlot.transform)
        {
            Destroy(child.gameObject);
        }
        GameObject cursorSpellGO = (GameObject)Instantiate(Resources.Load<GameObject>("Spells/SpellIcon"), Vector3.zero, Quaternion.identity);
        cursorSpellGO.GetComponent<Image>().raycastTarget = false;
        cursorSpellGO.transform.SetParent(cursorSlot.transform, false);

        Spell spell = cursorSpellGO.GetComponent<Spell>();
        spell.id = transform.GetComponentInParent<Spell>().id;
        spell.name = transform.GetComponentInParent<Spell>().name;
        spell.icon = transform.GetComponentInParent<Spell>().icon;
        spell.castTime = transform.GetComponentInParent<Spell>().castTime;
        spell.resource = transform.GetComponentInParent<Spell>().resource;
        spell.sParticles = transform.GetComponentInParent<Spell>().sParticles;
        spell.dParticles = transform.GetComponentInParent<Spell>().dParticles;

       
        SpellIcon = cursorSpellGO.GetComponent<Image>();
        SpellIcon.sprite = Resources.Load<Sprite>("Sprites/Skills/" + spell.icon);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("OnDrag");
    }

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("OnDrop");
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("OnEndDrag");
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("OnPointerDown");
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("OnPointerUp");
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}

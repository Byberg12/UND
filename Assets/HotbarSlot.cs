using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System;

public class HotbarSlot : MonoBehaviour, IPointerDownHandler,IPointerUpHandler
{
    public float pickupDelay = 1.0f;
    private float _pickupDelay;
    private bool mouseDown;

    // Use this for initialization
    void Start()
    {
        _pickupDelay = pickupDelay;
    }
    // Update is called once per frame
    void Update()
    {
        if(mouseDown)
        {
            _pickupDelay -= Time.deltaTime;

            if (_pickupDelay <= 0)
            {
                SwapSpells();
                _pickupDelay = pickupDelay;
            }
        }
        else
        {
           
        }
    }

    private void SwapSpells()
    {
        Debug.Log("swap");
        mouseDown = false;
        Transform cursorSlot = FindObjectOfType<CursorSlot>().transform;
        if (transform.childCount > 1)
        {
            Transform hotbarSpell = transform.GetChild(0);
            hotbarSpell.SetParent(cursorSlot, false);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {

        _pickupDelay = pickupDelay;
        mouseDown = false;
        Debug.Log("up");
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        mouseDown = true;
        Debug.Log("down");
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            Transform cursorSlot = FindObjectOfType<CursorSlot>().transform;
            if (cursorSlot.childCount > 0)
            {
                Transform cursorSpell = cursorSlot.GetChild(0);
                //Has spell on cursor
                if (transform.childCount > 1)
                {
                    Transform hotbarSpell = transform.GetChild(0);
                    hotbarSpell.SetParent(cursorSlot, false);
                }

                cursorSpell.SetParent(transform, false);
                cursorSpell.SetSiblingIndex(0);
            }
            else
            {
                //No spell on cursor
                //Check if spell in slot
                //Fire of spell
            }

        }
        if (eventData.button == PointerEventData.InputButton.Middle)
        {
            //remove spell from slot
            if (transform.childCount > 1)
            {
                Transform hotbarSpell = transform.GetChild(0);
                Destroy(hotbarSpell.gameObject);
            }
        }
    }

}





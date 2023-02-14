using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionButton : MonoBehaviour
{
    public KeyCode actionKey;
    private SlotHolder currentSlotholder;
    private void Awake()
    {
        currentSlotholder = GetComponent<SlotHolder>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(actionKey) && currentSlotholder.itemUI.GetItem())
        {
            currentSlotholder.UseItem();
        }
    }
}

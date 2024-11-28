using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private bool isMeleeEquipped = true; // Initially equipped with melee attack

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            isMeleeEquipped = true;
            Debug.Log("Switched to Melee Attack");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            isMeleeEquipped = false;
            Debug.Log("Switched to Range Attack (Bow)");
        }
    }

    public bool IsMeleeEquipped()
    {
        return isMeleeEquipped;
    }
}
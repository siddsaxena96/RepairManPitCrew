using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RepairCollisionMaster : MonoBehaviour
{
    public event Action<bool> InProgress = null;
    private bool isOn = false;
    public bool IsOn => isOn;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!isOn)
        {
            if (col.gameObject.tag == "PLAYER" || col.gameObject.tag == "NPC")
            {
                isOn = true;
                InProgress?.Invoke(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "PLAYER" || col.gameObject.tag == "NPC")
        {
            isOn = false;
            InProgress?.Invoke(false);
        }
    }
}

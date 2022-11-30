using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShightRange : MonoBehaviour
{
    StatBase _owner;

    public void InitSet(StatBase owner)
    {
        _owner = owner;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StatBase sb= other.GetComponent<PlayerController>();
            if (_owner.SightOn(sb))
            {
            }
        }
    }
}

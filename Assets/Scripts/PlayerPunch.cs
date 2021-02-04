using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPunch : MonoBehaviour
{
    private int normalEnnemiContact_ = 0;

    public int NormalEnnemiContact_ => normalEnnemiContact_;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("NormalEnnemi"))
        {
            normalEnnemiContact_++;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("NormalEnnemi"))
        {
            normalEnnemiContact_--;
        }
    }
}


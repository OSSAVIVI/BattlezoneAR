﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnImpact : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnCollisionEnter(Collision collision)
    {
        print(collision.gameObject);
        print(gameObject);
        print("SHOT HAS MADE IMPACT!");
        Destroy(gameObject);
    }
}

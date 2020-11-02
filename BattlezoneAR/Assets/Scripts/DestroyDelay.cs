using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyDelay : MonoBehaviour
{

    private void Start()
    {
        Destroy(gameObject, 3);
    }
}
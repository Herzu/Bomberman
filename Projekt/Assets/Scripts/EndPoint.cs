using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndPoint : MonoBehaviour
{
    void OnTriggerEnter(Collider other) {
        if(other.GetComponentInParent<BomberAgent>()!=null)
            other.GetComponentInParent<BomberAgent>().targetFound = true;
    }
}

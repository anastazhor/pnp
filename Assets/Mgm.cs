using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mgm : MonoBehaviour
{
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Smert")
        {
            Destroy(gameObject);
        }
    }
}

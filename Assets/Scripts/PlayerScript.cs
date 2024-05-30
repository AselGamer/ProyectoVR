using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    private CharacterController cc;
    public Transform tr_Respawn;

    public void Start()
    {
        cc = GetComponent<CharacterController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag != "Untagged")
        {
            if (other.transform.tag == "Espada")
            {
                transform.position = tr_Respawn.position;
            }
        }
    }
}

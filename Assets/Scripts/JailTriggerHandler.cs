using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JailTriggerHandler : MonoBehaviour
{
    private void OnTriggerEnter(Collider other){

        PlayerJailHandler jailHandler = other.GetComponent<PlayerJailHandler>();

        if (jailHandler != null){

            jailHandler.TriggerJail();
        }
        else{

            Debug.LogWarning($"Object {other.name} entered jail trigger but has no PlayerJailHandler attached.");
        }
    }
}

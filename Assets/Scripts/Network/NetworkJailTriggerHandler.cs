using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class NetworkJailTriggerHandler : NetworkBehaviour
{
    private void OnTriggerEnter(Collider other){

        NetworkPlayerJailController jailHandler = other.GetComponent<NetworkPlayerJailController>();

        if (jailHandler != null){

            jailHandler.TriggerJail();
        }
        else{

            Debug.LogWarning($"Object {other.name} entered jail trigger but has no PlayerJailHandler attached.");
        }
    }
}

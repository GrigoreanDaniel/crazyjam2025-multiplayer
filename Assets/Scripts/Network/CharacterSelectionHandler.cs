using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelectionHandler : MonoBehaviour
{
    [Header("Character Selection UI Elements")]
    [SerializeField] private GameObject characterSelected; // Reference to the character selection UI
    [SerializeField] List<GameObject> characterList = new List<GameObject>(); // List to store character game objects

    void Awake()
    {

    }

}

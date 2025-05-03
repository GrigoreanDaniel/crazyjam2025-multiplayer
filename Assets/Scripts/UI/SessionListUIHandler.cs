using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SessionListUIHandler : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI statusText;
    [SerializeField] private GameObject sessionItemListPrefab;
    [SerializeField] private VerticalLayoutGroup verticalLayoutGroup;

    public void ClearList()
    {
        foreach (Transform child in verticalLayoutGroup.transform)
        {
            Destroy(child.gameObject);
        }
        statusText.gameObject.SetActive(false);
    }
}

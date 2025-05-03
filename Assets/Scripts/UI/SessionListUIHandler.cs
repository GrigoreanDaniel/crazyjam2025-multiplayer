using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
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

    public void AddToList(SessionInfo sessionInfo)
    {
        GameObject sessionItem = Instantiate(sessionItemListPrefab, verticalLayoutGroup.transform);
        SessionInfoListUIItem sessionInfoListUIItem = sessionItem.GetComponent<SessionInfoListUIItem>();
        sessionInfoListUIItem.SetInformation(sessionInfo);
        sessionInfoListUIItem.OnJoinSessionClicked += OnJoinSessionClicked;
    }

    private void OnJoinSessionClicked(SessionInfo info)
    {

    }

    public void SetStatusText(string text)
    {
        statusText.text = text;
        statusText.gameObject.SetActive(true);
    }
}

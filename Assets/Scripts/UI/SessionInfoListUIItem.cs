using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SessionInfoListUIItem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI sessionNameText;
    [SerializeField] private TextMeshProUGUI PlayerCountText;
    [SerializeField] private Button joinButton;
    SessionInfo sessionInfo;
    public event Action<SessionInfo> OnJoinSessionClicked;

    public void SetInformation(SessionInfo sessionInfo)
    {
        this.sessionInfo = sessionInfo;
        sessionNameText.text = sessionInfo.Name;
        PlayerCountText.text = $"{sessionInfo.PlayerCount.ToString()}/{sessionInfo.MaxPlayers.ToString()}";

        bool isJoinButtonActive = sessionInfo.PlayerCount < sessionInfo.MaxPlayers;
        joinButton.gameObject.SetActive(isJoinButtonActive);
    }

    public void OnJoinButtonClicked()
    {
        OnJoinSessionClicked?.Invoke(sessionInfo);
    }
}

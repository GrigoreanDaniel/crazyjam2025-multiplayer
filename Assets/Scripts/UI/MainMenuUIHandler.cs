using System;
using TMPro;
using UnityEngine;

public class MainMenuUIHandler : MonoBehaviour
{
    [Header("UI Panels")]
    [SerializeField] private GameObject PlayerDetailsPanel;
    [SerializeField] private GameObject SessionListPanel;
    [SerializeField] private GameObject CreateSessionPanel;
    [SerializeField] private GameObject statusPanel;

    [Space(10)]
    [Header("Player Nickname")]
    [SerializeField] private TMP_InputField inputFieldPlayerName;

    [Space(10)]
    [Header("Session Name")]
    [SerializeField] private TMP_InputField sessionNameIF;

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey("PlayerName"))
            inputFieldPlayerName.text = PlayerPrefs.GetString("PlayerName");

    }

    public void OnFindGameClicked()
    {
        SetNickname();

        NetworkRunnerHandler networkRunnerHandler = FindObjectOfType<NetworkRunnerHandler>();

        if (networkRunnerHandler != null)
        {
            networkRunnerHandler.OnJoinLobby();

            HidePanels();
            SessionListPanel.SetActive(true);

            FindObjectOfType<SessionListUIHandler>(true).SetStatusText("Looking active games...");
        }
        else
        {
            Debug.LogError("NetworkRunnerHandler not found in the scene.");
        }
    }

    public void OnCreateGameClicked()
    {
        SetNickname();
        HidePanels();
        CreateSessionPanel.SetActive(true);
    }

    public void OnStartNewSessionClicked()
    {
        NetworkRunnerHandler networkRunnerHandler = FindObjectOfType<NetworkRunnerHandler>();

        if (networkRunnerHandler != null)
        {
            networkRunnerHandler.CreateGame(sessionNameIF.text, LoadScenes.SceneName.Game.ToString());

            HidePanels();
            statusPanel.SetActive(true);
        }
        else
        {
            Debug.LogError("NetworkRunnerHandler not found in the scene.");
        }
    }

    private void HidePanels()
    {
        PlayerDetailsPanel.SetActive(false);
        SessionListPanel.SetActive(false);
        CreateSessionPanel.SetActive(false);
        statusPanel.SetActive(false);
    }

    ///
    private void SetNickname()
    {
        PlayerPrefs.SetString("PlayerName", inputFieldPlayerName.text);
        PlayerPrefs.Save();
    }

    public void OnJoiningServer()
    {
        HidePanels();
        statusPanel.SetActive(true);
    }
}

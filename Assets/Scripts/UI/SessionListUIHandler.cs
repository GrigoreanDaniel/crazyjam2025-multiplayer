using Fusion;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SessionListUIHandler : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI statusText;
    [SerializeField] private GameObject sessionItemListPrefab;
    [SerializeField] private VerticalLayoutGroup verticalLayoutGroup;

    private void Awake()
    {
        ClearList();
    }

    public void ClearList()
    {
        foreach (RectTransform child in verticalLayoutGroup.transform)
        {
            Destroy(child.gameObject);

            Debug.Log("Destroying child: " + child.gameObject.name);
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

    private void OnJoinSessionClicked(SessionInfo sessionInfo)
    {
        NetworkRunnerHandler networkRunnerHandler = FindObjectOfType<NetworkRunnerHandler>();

        if (networkRunnerHandler != null)
        {
            networkRunnerHandler.JoinGame(sessionInfo, LoadScenes.SceneName.Game.ToString());

            MainMenuUIHandler mainMenuUIHandler = FindObjectOfType<MainMenuUIHandler>();
            mainMenuUIHandler.OnJoiningServer();
        }
    }

    public void SetStatusText(string text)
    {
        ClearList();

        statusText.text = text;
        statusText.gameObject.SetActive(true);
    }
}

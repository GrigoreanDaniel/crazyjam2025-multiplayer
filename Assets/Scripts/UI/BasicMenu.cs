using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicMenu : MonoBehaviour
{

    [SerializeField] private GameObject MenuPanel;
    [SerializeField] private GameObject HowToPlayPanel;
    [SerializeField] private GameObject CreditsPanel;


    private void HidePanels()
    {
        MenuPanel.SetActive(false);
        HowToPlayPanel.SetActive(false);
        CreditsPanel.SetActive(false);
    }

    public void OnPlayButtonClicked()
    {
        HidePanels();
        LoadScenes.ChangeScene(LoadScenes.SceneName.Lobby);
    }

    public void OnHowToPlayButtonClicked()
    {
        HidePanels();
        HowToPlayPanel.SetActive(true);
    }

    public void OnCreditsButtonClicked()
    {
        HidePanels();
        CreditsPanel.SetActive(true);
    }


    public void OnReturnButtonClicked()
    {
        HidePanels();
        MenuPanel.SetActive(true);
    }




}

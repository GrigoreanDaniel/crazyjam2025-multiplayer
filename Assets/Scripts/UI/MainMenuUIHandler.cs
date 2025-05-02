using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUIHandler : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputFieldPlayerName;
    [SerializeField] private Button setNickNameButton;

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey("PlayerName"))
            inputFieldPlayerName.text = PlayerPrefs.GetString("PlayerName");

    }

    public void OnJoinGameClicked()
    {
        PlayerPrefs.SetString("PlayerName", inputFieldPlayerName.text);
        PlayerPrefs.Save();
        LoadScenes.ChangeScene(LoadScenes.Scene.Game);

    }


}

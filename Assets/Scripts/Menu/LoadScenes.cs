using UnityEngine;
using UnityEngine.SceneManagement;

public static class LoadScenes
{

    //scene names
    public enum SceneName
    {
        MainMenu,
        Lobby,
        Loading,
        Game,
    }

    public static void ChangeScene(SceneName targetScene)
    {
        SceneManager.LoadScene(targetScene.ToString());
    }

    //method to quit the game
    public static void QuitGame()
    {
        Application.Quit();
    }

}


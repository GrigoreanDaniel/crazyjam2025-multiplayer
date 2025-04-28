using UnityEngine;
using UnityEngine.SceneManagement;

public static class LoadScenes
{

    //scene names
    public enum Scene
    {
        MainMenu,
        Lobby,
        Game,
        Loading,
        CharacterSelection,
    }

    private static Scene targetScene;

    public static void ChangeScene(Scene targetScene)
    {
        SceneManager.LoadScene(targetScene.ToString());
    }

    //method to load a scene
    public static void LoadTagetScene(Scene targetScene)
    {
        //SceneManager.LoadScene(targetScene.ToString(), LoadSceneMode.Single);
    }

    public static void LoaderCallback()
    {
        SceneManager.LoadScene(targetScene.ToString());
    }

    //method to quit the game
    public static void QuitGame()
    {
        Application.Quit();
    }

}


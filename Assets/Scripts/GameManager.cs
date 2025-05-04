
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; } = null;
    private byte[] playerConnectionToken;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject); // Destroy duplicate instance
            return;
        }
        DontDestroyOnLoad(gameObject); // Keep this object alive across scenes
    }

    void Start()
    {
        if (playerConnectionToken == null)
        {
            playerConnectionToken = ConnectionTokenUtils.Newtoken(); // Initialize the token if not set
            Debug.Log($"Player connection token initialized {ConnectionTokenUtils.HashToken(playerConnectionToken)}");
        }

    }

    public void SetPlayerConnectionToken(byte[] token)
    {
        playerConnectionToken = token;
    }
    public byte[] GetPlayerConnectionToken()
    {
        return playerConnectionToken;
    }
}

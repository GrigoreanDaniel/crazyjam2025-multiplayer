using UnityEngine;
using Fusion;

public class NetworkPlayerJailController : NetworkBehaviour
{
    [Header("Jail Settings")]
    [SerializeField] private float jailDuration = 10f;

    private bool isJailed = false;
    private float jailTimer = 0f;

    private CharacterController characterController;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();

        if (characterController == null)
        {
            Debug.LogError("CharacterController not found on the player.");
        }
    }

    private void Update()
    {
        if (!isJailed) return;

        jailTimer -= Time.deltaTime;
        if (jailTimer <= 0f)
        {
            ReleaseFromJail();
        }

        if (Object.HasInputAuthority && NetworkJailUIManager.Instance != null)
        {
            NetworkJailUIManager.Instance.UpdateJailTimerText(jailTimer);
        }
    }

    public void TriggerJail()
    {
        if (isJailed) return;

        isJailed = true;
        jailTimer = jailDuration;
        characterController.enabled = false;

        if (Object.HasInputAuthority && NetworkJailUIManager.Instance != null)
        {
            NetworkJailUIManager.Instance.ShowJailUI(jailDuration);
        }
    }

    private void ReleaseFromJail()
    {
        isJailed = false;
        characterController.enabled = true;

        if (Object.HasInputAuthority && NetworkJailUIManager.Instance != null)
        {
            NetworkJailUIManager.Instance.HideJailUI();
        }
    }
}

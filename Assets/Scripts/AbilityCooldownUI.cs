using UnityEngine;
using UnityEngine.UI;

public class AbilityCooldownUI : MonoBehaviour
{
    [SerializeField] private Image fillImage;
    [SerializeField] private GameObject playerObject;

    private IAbility abilityScript;

    private void Start()
    {
        if (playerObject == null)
        {
            Debug.LogWarning("[AbilityCooldownUI] Player not assigned.");
            return;
        }

        abilityScript = playerObject.GetComponent<IAbility>();

        if (abilityScript == null)
        {
            // Fallback: try to find manually
            abilityScript = TryFindAbilityScript();
        }

        if (abilityScript == null)
            Debug.LogWarning("[AbilityCooldownUI] No ability script found.");

        if (fillImage != null)
            fillImage.fillAmount = 0f;
    }

    private void Update()
    {
        if (abilityScript == null || fillImage == null) return;

        float cooldown = abilityScript.GetCooldownNormalized(); // 0 = ready, 1 = cooling
        fillImage.fillAmount = 1f - cooldown;
    }

    private IAbility TryFindAbilityScript()
    {
        return playerObject.GetComponent<SonarPulse>() as IAbility ??
               playerObject.GetComponent<DecoySpawner>() as IAbility ??
               playerObject.GetComponent<MoleDigAbility>() as IAbility;
    }

    public void Setup(GameObject player)
    {
        this.playerObject = player;

        // Try to grab ability script
        abilityScript = player.GetComponent<IAbility>();
        if (abilityScript == null)
            abilityScript = TryFindAbilityScript();

        if (abilityScript == null)
            Debug.LogWarning("[CooldownUI] No ability found on player!");

        // OPTIONAL: make sure only the correct fill image is active
        if (fillImage != null)
        {
            // Deactivate all siblings, only show this one
            Transform parent = fillImage.transform.parent;
            foreach (Transform child in parent)
            {
                child.gameObject.SetActive(child == fillImage.transform);
            }
        }
    }


    public void SetFillImage(Image image)
    {
        fillImage = image;
    }

}

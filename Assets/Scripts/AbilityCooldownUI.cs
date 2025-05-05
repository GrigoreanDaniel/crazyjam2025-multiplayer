using UnityEngine;
using UnityEngine.UI;

public class AbilityCooldownUI : MonoBehaviour
{
    [SerializeField] private Image fillImage;
    [SerializeField] private GameObject playerObject;

    private IAbility abilityScript;

    private void Start()
    {
        if (playerObject == null) return;
        abilityScript = TryFindAbilityScript();
    }

    private void Update()
    {
        if (abilityScript == null || fillImage == null) return;
        float cooldown = abilityScript.GetCooldownNormalized();
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
        this.abilityScript = TryFindAbilityScript();
    }
}

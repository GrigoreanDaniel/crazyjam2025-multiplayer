using UnityEngine;
using UnityEngine.UI;

public class PlayerAbilityUIBinder : MonoBehaviour
{
    [Header("Ability UI Elements")]
    [SerializeField] private Image abilityFillImage;  // This fills up with team color
    [SerializeField] private Image abilityIconImage;  // This shows the static icon

    [Header("Ability Icons")]
    [SerializeField] private Sprite owlIcon;
    [SerializeField] private Sprite foxIcon;
    [SerializeField] private Sprite moleIcon;

    [Header("Team Colored Ability Icon")]
    [SerializeField] private Sprite owlBlueIcon;
    [SerializeField] private Sprite owlPurpleIcon;
    [SerializeField] private Sprite foxBlueIcon;
    [SerializeField] private Sprite foxPurpleIcon;
    [SerializeField] private Sprite moleBlueIcon;
    [SerializeField] private Sprite molePurpleIcon;

    [Header("Team Fill Images")]
    [SerializeField] private Image blueFillImage;
    [SerializeField] private Image purpleFillImage;

    [Header("Target Player")]
    [SerializeField] private GameObject playerObject;

    [SerializeField] public TeamData blueTeam;

    private IAbility activeAbility;
    private TeamData myTeam;


    private void Start()
    {
        if (playerObject == null)
        {
            Debug.LogWarning("[AbilityUI] No player object assigned.");
            return;
        }

        PlayerAbilityConfig abilityConfig = playerObject.GetComponent<PlayerAbilityConfig>();
        if (abilityConfig == null)
        {
            Debug.LogWarning("[AbilityUI] Missing PlayerAbilityConfig on player.");
            return;
        }

        AssignAbilityBasedOnType(abilityConfig.abilityType);

        TeamIdentifier teamId = playerObject.GetComponent<TeamIdentifier>();
        if (teamId != null)
        {
            myTeam = teamId.Team;
        }
        else
        {
            Debug.LogWarning("[AbilityUI] No TeamIdentifier found on player.");
            return;
        }

    }

    private void Update()
    {
        if (activeAbility == null || abilityFillImage == null) return;

        float normalizedCooldown = activeAbility.GetCooldownNormalized(); // 0 = ready, 1 = cooling
        abilityFillImage.fillAmount = 1f - normalizedCooldown;
    }

    /// <summary>
    /// Based on the assigned AbilityType, sets the icon and finds the correct ability component.
    /// </summary>
    /// <param name="type">AbilityType enum (Owl, Fox, Mole)</param>
    private void AssignAbilityBasedOnType(AbilityType type)
    {
        switch (type)
        {
            case AbilityType.Owl:
                activeAbility = (IAbility)playerObject.GetComponent<SonarPulse>();
                abilityIconImage.sprite = myTeam == blueTeam ? owlBlueIcon : owlPurpleIcon;
                break;

            case AbilityType.Fox:
                activeAbility = (IAbility)playerObject.GetComponent<DecoySpawner>();
                abilityIconImage.sprite = myTeam == blueTeam ? owlBlueIcon : owlPurpleIcon;
                break;

            case AbilityType.Mole:
                activeAbility = (IAbility)playerObject.GetComponent<MoleDigAbility>();
                abilityIconImage.sprite = myTeam == blueTeam ? owlBlueIcon : owlPurpleIcon;
                break;
        }
    }

    public Image GetBlueFillImage() => blueFillImage;
    public Image GetPurpleFillImage() => purpleFillImage;

}

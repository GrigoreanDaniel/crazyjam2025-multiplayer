using UnityEngine;
using Fusion;
using static UnityEngine.EventSystems.PointerEventData;

public class NetworkDig : NetworkBehaviour {

    [Networked] private NetworkBool IsDiggingNet { get; set; }

    [Header("Dig Parameters")]
    [SerializeField, Tooltip("How long the player stays underground.")]
    private float digDuration = 5f;

    [SerializeField, Tooltip("Cooldown after using Dig.")]
    private float cooldownDuration = 10f;

    [Header("References")]
    [SerializeField] private DigVisualController digVisuals;

    private bool isDigging = false;
    private bool isOnCooldown = false;

    private float digTimer = 0f;
    private float cooldownTimer = 0f;

    public override void FixedUpdateNetwork()
    {
        if (GetInput(out NetworkInputData input))
        {
            if (input.digPressed && Object.HasInputAuthority && !isDigging && !isOnCooldown)
            {
                BeginDig();
            }
        }

        UpdateTimers();
    }

    private void UpdateTimers() {
        float delta = Runner.DeltaTime;

        if (isDigging)
        {
            digTimer -= delta;
            if (digTimer <= 0f)
            {
                EndDig();
            }
        }

        if (isOnCooldown)
        {
            cooldownTimer -= delta;
            if (cooldownTimer <= 0f)
            {
                isOnCooldown = false;
            }
        }
    }
    private void BeginDig()
    {
        isDigging = true;
        IsDiggingNet = true;
        digTimer = digDuration;
        digVisuals.ApplyDigState(true);
    }

    private void EndDig()
    {
        isDigging = false;
        IsDiggingNet = false;
        isOnCooldown = true;
        cooldownTimer = cooldownDuration;
        digVisuals.ApplyDigState(false);
    }

    public bool IsDigging => isDigging;
    public bool IsOnCooldown => isOnCooldown;
    
    public override void Render()
    {
        digVisuals.ApplyDigState(IsDiggingNet);
    }
    
}

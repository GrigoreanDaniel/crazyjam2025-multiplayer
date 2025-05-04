using UnityEngine;

public class PlayerFlagCarrier : MonoBehaviour {
    public TeamData Team { get; private set; }

    [SerializeField] private TeamData assignedTeam;
    [SerializeField] private Transform carryPoint;

    [SerializeField] private GameObject beaconPrefab;
    [SerializeField] private float beaconYOffset;
    [SerializeField] private Vector3 flagOffset = new Vector3(0f, 2.5f, 0f);

    private GameObject activeBeacon;
    private Flag carriedFlag;

    private void Awake() {
        Team = assignedTeam;
    }
    public Vector3 GetFlagOffset() => flagOffset;

    public void AttemptFlagPickup(Flag flag) {
        if (carriedFlag != null) return;
        if (!flag.TryPickup(this)) return; // make TryPickup return bool

        carriedFlag = flag;
        FlagEvents.OnFlagPickedUp?.Invoke(flag, this);

        // Show beacon above head
        if (beaconPrefab != null) {
            activeBeacon = Instantiate(beaconPrefab, carryPoint);
            activeBeacon.transform.localPosition = Vector3.up * beaconYOffset;

            TeamIdentifier flagTeam = flag.GetComponent<TeamIdentifier>();
            if (flagTeam != null && flagTeam.Team != null) {
                var flagBeacon = activeBeacon.GetComponent<FlagBeacon>();
                flagBeacon?.SetColor(flagTeam.Team.teamColor);
            }
        }
    }

    public void DropFlag() {
        if (carriedFlag == null) return;
        carriedFlag.Drop();
        carriedFlag = null;

        if (activeBeacon != null)
            Destroy(activeBeacon);
    }

    public void ReturnFlag() {
        if (carriedFlag == null) return;
        carriedFlag.ReturnToBase();
        carriedFlag = null;

        if (activeBeacon != null)
            Destroy(activeBeacon);
    }

    private void Update() {
        // TEMP: Trigger pickup on key (replace with trigger logic later)
        if (Input.GetKeyDown(KeyCode.F)) {
            Collider[] hits = Physics.OverlapSphere(transform.position, 2f);
            foreach (var hit in hits) {
                Flag flag = hit.GetComponent<Flag>();
                if (flag != null) {
                    AttemptFlagPickup(flag);
                    break;
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.G)) DropFlag();
    }
}

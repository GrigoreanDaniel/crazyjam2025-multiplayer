using UnityEngine;

public class PlayerFlagInput : MonoBehaviour {
    [SerializeField] private FlagPickupHandler flagPickupHandler;

    private void Start() {
        string myTeam = GetComponent<TeamIdentifier>()?.TeamTag;

        // This must only run on the local player (which is easy if it's a single-player test).
        LocalPlayerTracker.Instance?.SetTeam(GetComponent<TeamIdentifier>()?.TeamTag);
        // In multiplayer you'll want a photonView.IsMine or similar check around it.
    }

    private void Update() {
        if (flagPickupHandler == null) return;

        if (Input.GetKeyDown(KeyCode.G)) {
            flagPickupHandler.DropFlag(transform.position, gameObject);
        }
    }

    public void AssignFlagReference(FlagPickupHandler handler) {
        flagPickupHandler = handler;
    }

}

using UnityEngine;

public class PlayerFlagInput : MonoBehaviour {
    [SerializeField] private FlagPickupHandler flagPickupHandler;

    private void Update() {
        if (flagPickupHandler == null) return;

        if (Input.GetKeyDown(KeyCode.G)) {
            flagPickupHandler.DropFlag(transform.position);
        }
    }

    public void AssignFlagReference(FlagPickupHandler handler) {
        flagPickupHandler = handler;
    }

}

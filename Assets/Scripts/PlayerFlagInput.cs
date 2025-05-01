using UnityEngine;

public class PlayerFlagInput : MonoBehaviour {
    [SerializeField] private FlagPickupHandler flagPickupHandler;

    private void Update() {
        if (Input.GetKeyDown(KeyCode.G)) {
            flagPickupHandler.DropFlag(transform.position);
        }
    }
}

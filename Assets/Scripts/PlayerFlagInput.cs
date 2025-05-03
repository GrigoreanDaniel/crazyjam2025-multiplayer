using UnityEngine;

public class PlayerFlagInput : MonoBehaviour {
    //[SerializeField] private FlagPickupHandler flagPickupHandler;
    public TeamData Team => GetComponent<TeamIdentifier>().Team;

    /*private void Update() {
        if (flagPickupHandler == null) return;

        if (Input.GetKeyDown(KeyCode.G)) {
            flagPickupHandler.DropFlag(transform.position, gameObject);
        }
    }*/

   /* public void AssignFlagReference(FlagPickupHandler handler) {
        flagPickupHandler = handler;
    }*/
}
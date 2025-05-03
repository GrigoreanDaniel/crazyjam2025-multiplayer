using System;

public static class FlagEvents {
    public static Action<Flag, PlayerFlagCarrier> OnFlagPickedUp;
    public static Action<Flag, PlayerFlagCarrier> OnFlagDropped;
    public static Action<Flag> OnFlagReturned;
}

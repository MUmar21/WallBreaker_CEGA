using System;

public static class EventManager
{
    public static Action Damage;
    public static Action<BricksHandler> OnDetachEvent;
}

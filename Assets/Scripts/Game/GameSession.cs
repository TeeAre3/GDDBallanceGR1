using System.Data;
using UnityEngine;

public static class GameSession
{
    public const int MaxLives = 3;
    public static int Lives = MaxLives;

    public static void ResetLives() => Lives = MaxLives;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    static void Init() => ResetLives();
}

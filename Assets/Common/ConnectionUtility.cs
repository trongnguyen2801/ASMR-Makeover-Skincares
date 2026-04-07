using UnityEngine;

public class ConnectionUtility
{
    public static bool InternetAvailable => Application.internetReachability != NetworkReachability.NotReachable;
}
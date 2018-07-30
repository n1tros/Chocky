using System.Collections.Generic;
using UnityEngine;

public class DeathManager : MonoBehaviour
{
    public static List<Agent> Enemies;
    public static Agent Player;

    public static void ProcessDeath(Agent agent)
    {
        Enemies.Remove(agent);
        Player.GetComponent<XP>().AddXP(agent.Settings.XpOnDeath);
    }
}

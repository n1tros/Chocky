public class AgentStats
{
    /* holds current stats on player based on settings and skills */
    public float MoveSpeed { get; set; }
    public float ReloadSpeed { get; set; }
    public float FireRate { get; set; }
    public float MeleeFireRate { get; internal set; }

    public AgentStats(Agent agent)
    {
        MoveSpeed = agent.Settings.MoveSpeed;
        //TODO: Possible link these to initial settings.
        ReloadSpeed = 1;
        FireRate = 1;
        MeleeFireRate = 1;
    }
}
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour
{
    IEnumerator Hit(float damageTime, Weapon weapon, AgentController agent)
    {
        agent.AgentTakeDamage(weapon.BaseDamage);
        yield return new WaitForSeconds(damageTime);
        agent.AgentPreviousState();
    }

}

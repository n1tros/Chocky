using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eyesight : MonoBehaviour 
{
    private AIController _ai = null;
    [SerializeField] private LayerMask _intercepts;

    private void Start()
    {
        _ai = GetComponentInParent<AIController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_ai.GetComponent<AgentController>().IsDead)
            return;

        if (collision.CompareTag("Player"))
        {
            TargetPlayerIfInSight(collision);
        }
    }

    private void TargetPlayerIfInSight(Collider2D collision)
    {
        var player = collision.GetComponent<AgentController>();
        bool sightBlocked = Physics2D.Linecast(transform.position, player.transform.position + new Vector3(0, 1f), _intercepts);

        Debug.DrawLine(transform.position, player.transform.position + new Vector3(0, 1f), Color.red, 5f);
        if (!sightBlocked)
            _ai.TargetAgent(player);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            _ai.TargetLoss();
    }
}

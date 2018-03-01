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
        Debug.Log(transform.root.gameObject.name + " trigger enter " + collision.name);
        if (collision.CompareTag("Player"))
        {
            var player = collision.GetComponent<AgentController>();
            bool sightBlocked = Physics2D.Linecast(transform.position, player.transform.position + new Vector3(0,1f), _intercepts);
            var layerhit = Physics2D.Linecast(transform.position, player.transform.position + new Vector3(0,1f), _intercepts);

            Debug.DrawLine(transform.position, player.transform.position + new Vector3(0, 1f), Color.red, 5f);
            //TODO: Currently If an intercept is hit it doesn't calculate if it is in front or behind.

            if (!sightBlocked)
                _ai.TargetAgent(player);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
       // Debug.Log(transform.root.gameObject.name + " trigger Exit " + collision.name);

        if (collision.CompareTag("Player"))
            _ai.TargetLoss();
    }
}

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
            Debug.Log("player Collision true");
            var player = collision.GetComponent<AgentController>();
            Debug.Log("Player: " + player.ToString());
            bool sightBlocked = Physics2D.Linecast(transform.position, player.transform.position + new Vector3(0,1f), _intercepts);
            Debug.Log("Sightblocked: " + sightBlocked);
            var layerhit = Physics2D.Linecast(transform.position, player.transform.position + new Vector3(0,1f), _intercepts);
            if (layerhit)
                Debug.Log("Layer hit: " + layerhit.collider.name);
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

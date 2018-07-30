using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eyesight : MonoBehaviour
{
    [SerializeField] private LayerMask _intercepts;

    public bool HasTarget { get; private set; }
    public bool LostTarget { get; private set; }
    public Agent Target { get; private set; }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            TargetPlayerIfInSight(collision);
        }
    }

    private void TargetPlayerIfInSight(Collider2D collision)
    {
        var player = collision.GetComponent<Agent>();
        bool sightBlocked = Physics2D.Linecast(transform.position + new Vector3(0, 2f), player.transform.position + new Vector3(0, 2.5f), _intercepts);

        Debug.DrawLine(transform.position + new Vector3(0, 2f), player.transform.position + new Vector3(0, 2.5f), Color.red, 5f);
        if (!sightBlocked)
        {
            HasTarget = true;
            Target = player;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            HasTarget = false;
            StartCoroutine(LostTargetTimer());
        }
    }

    private IEnumerator LostTargetTimer()
    {
        LostTarget = true;
        yield return new WaitForSeconds(10f);
        LostTarget = false;
    }
}

using UnityEngine;

public class FollowChild : MonoBehaviour
{
    private Transform _child;

    private void Awake()
    {
        _child = transform.parent.GetComponentInChildren<Agent>().transform;
    }
    void Update ()
    {
        transform.position = _child.position;
	}
}

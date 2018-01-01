using UnityEngine;

/// <summary>
/// Class to check if an agent is grounded or not
/// </summary>
public class GroundCheck : MonoBehaviour
{
    [Header("Ground Check Properties")]
    [SerializeField] private Transform _groundCheckChild = null;
    [SerializeField] private float _groundCheckRadius = 0.0f;
    [SerializeField] private LayerMask _groundLayer;

    private AgentController _agent = null;

    private void Start()
    {
        _agent = GetComponent<AgentController>();
    }

    private void FixedUpdate()
    {
        _agent.IsGrounded = Physics2D.OverlapCircle(_groundCheckChild.position, _groundCheckRadius, _groundLayer);
    }


}

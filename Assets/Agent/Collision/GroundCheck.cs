using System.Collections;
using UnityEngine;

/// <summary>
/// Class to check if an agent is grounded or not
/// </summary>
public class GroundCheck : MonoBehaviour
{
    [Header("Ground Check Properties")]

    [SerializeField] private Transform _groundCheckChild;
    [SerializeField] private float _groundCheckRadius;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private bool _isGrounded;
    [SerializeField] private float _disableGroundCheckTimer;
    private Agent _agent;
    private bool _timerActive;

    public bool IsGrounded
    {
        get
        {
            return _isGrounded;
        }
        set
        {
            if (value == false && _timerActive == false)
            {
                _isGrounded = false;
                _timerActive = true;
                StartCoroutine(GroundedTimer());
            }
            else if (value == true && _timerActive == false)
                _isGrounded = true;
        }
    }

    private void Start()
    {
        _agent = GetComponent<Agent>();
    }

    IEnumerator GroundedTimer()
    {
        _timerActive = true;
        yield return new WaitForSeconds(_disableGroundCheckTimer);
        _timerActive = false;
    }

    private void FixedUpdate()
    {
        IsGrounded = Physics2D.OverlapCircle(_groundCheckChild.position, _groundCheckRadius, _groundLayer);
    }
}

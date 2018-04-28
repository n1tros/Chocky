using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowAgent : MonoBehaviour
{
    [SerializeField] private GameObject _target = null;
    [SerializeField] private float _followAhead = 0f;
    [SerializeField] private float _lerpSpeed = 0f;

    public bool FollowTarget { get; set; }

    private Vector3 _targetPostion;

    private void Start()
    {
        FollowTarget = true;
        _target = GameObject.FindGameObjectWithTag("Player");
    }

    private void FixedUpdate()
    {
        if (FollowTarget)
        {
            _targetPostion = new Vector3(_target.transform.position.x, transform.position.y, transform.position.z);

            if (_target.transform.localScale.x > 0f)
            {
                _targetPostion = new Vector3(_targetPostion.x + _followAhead, _targetPostion.y, _targetPostion.z);
            }
            else
            {
                _targetPostion = new Vector3(_targetPostion.x - _followAhead, _targetPostion.y, _targetPostion.z);
            }

            transform.position = Vector3.Lerp(transform.position, _targetPostion, _lerpSpeed * Time.deltaTime);
        }
    }
}

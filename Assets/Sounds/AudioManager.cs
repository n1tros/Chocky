using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private AgentController _agent;
    private AudioSource _audio;
    private WeaponController _weaponController;

    private void Awake()
    {
        _agent = GetComponent<AgentController>();
        _audio = GetComponent<AudioSource>();
        _weaponController = GetComponent<WeaponController>();
    }

    private void Attack()
    {
        if (_weaponController.IsFiring)
        {
            var weaponSoundsArray = _weaponController.Current.WeaponSounds;
            if (weaponSoundsArray.Length > 0)
            {
                _audio.clip = weaponSoundsArray[Random.Range(0, weaponSoundsArray.Length)];
                _audio.Play();
            }
        }
    }

    private void OnEnable()
    {
        _agent.OnAttack += Attack;
    }

    private void OnDisable()
    {
        _agent.OnAttack -= Attack;
    }
}

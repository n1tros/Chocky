using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField] private AudioClip _levelMusic;
    private AudioSource _audio;

    private void Awake()
    {
        _audio = GetComponent<AudioSource>();
        _audio.clip = _levelMusic;
        _audio.Play();
    }
}

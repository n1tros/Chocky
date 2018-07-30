using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _pauseScreen;

    private bool _isPaused;
    public bool IsPaused
    {
        get { return _isPaused; }
        set
        {
            _isPaused = value;
            if (_isPaused == true)
            {
                Time.timeScale = 0f;
                _pauseScreen.SetActive(true);
            }
            else
            {
                _pauseScreen.SetActive(false);
                Time.timeScale = 1f;
            }
        }
    }
	// Use this for initialization
	void Start ()
    {
        IsPaused = false;
	}
	
	void Update ()
    {
        if (Input.GetButtonDown("Cancel"))
            IsPaused = !IsPaused;
	}
}

using UnityEngine;
using UnityEngine.UI;

public class GetPlayerUIData : MonoBehaviour
{
    [SerializeField] private Text _totalAmmo;
    [SerializeField] private Text _maxClip;
    [SerializeField] private Text _currentClip;
    [SerializeField] private Image _weaponSprite;

    public Agent Player { get; set; }
    private Weapons _weapons;

    private void Awake()
    {
        var test = FindObjectsOfType<Agent>();
        for (int i = 0; i < test.Length; i++)
        {
            if (test[i].CompareTag("Player"))
            {
                Player = test[i];
                break;
            }
        }
    }

    void Start()
    {
        _weapons = Player.GetComponent<Weapons>();
        Player.Body.OnSwitchWeapons += ChangeImage;
        ChangeImage();
    }

    void Update()
    {
        _totalAmmo.text = _weapons.CurrentWeapon.CurrentAmmo.ToString();
        _maxClip.text = _weapons.CurrentWeapon.CurrentMaxClip.ToString();
        _currentClip.text = _weapons.CurrentWeapon.CurrentClip.ToString();
        
    }

    private void ChangeImage()
    {
        _weaponSprite.sprite = _weapons.CurrentWeapon.Icon;
    }

    private void OnDisable()
    {
        Player.Body.OnSwitchWeapons -= ChangeImage;
    }

}

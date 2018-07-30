using System.Collections;
using TMPro;
using UnityEngine;

//TODO : Needs refactor so that text can be generated with different sizes, colours etc
public class CombatText : MonoBehaviour
{
    [SerializeField]
    private float _speed, _fadeTime;
    private TextMeshPro _tmp;

    private void Awake()
    {
        _tmp = GetComponent<TextMeshPro>();
    }

    private void Update()
    {
        var distance = _speed * Time.deltaTime;
        transform.Translate(Vector2.up * distance);
    }

    public void Init(float fontSize, Color fontColour)
    {
        StartCoroutine(FadeOut());
        _tmp.fontSize = fontSize;
        _tmp.color = fontColour;
    }

    private IEnumerator FadeOut()
    {
        var startAlpha = _tmp.color.a;
        var fadeRate = 1f / _fadeTime;

        var progressTimer = 0f;
        while (progressTimer < 1f)
        {
            var color = _tmp.color;
            _tmp.color = new Color(color.r, color.g, color.b, Mathf.Lerp(startAlpha, 0, progressTimer));

            progressTimer += fadeRate * Time.deltaTime;
            yield return null;
        }
    }
}

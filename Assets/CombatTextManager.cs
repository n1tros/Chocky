using UnityEngine;

public class CombatTextManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _textPrefab;

    private void CreateText(Vector2 position)
    {
        var textGameObject = Instantiate(_textPrefab, position, Quaternion.identity);
    }
}

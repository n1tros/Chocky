using UnityEngine;

public class EnemyUI : MonoBehaviour {

    // Works around Unity 5.5's lack of nested prefabs
    [Tooltip("The UI canvas prefab")]
    [SerializeField]
    GameObject enemyCanvasPrefab = null;

    // Use this for initialization 
    void Start()
    {
        Instantiate(enemyCanvasPrefab, transform.position, Quaternion.identity, transform);
    }
}
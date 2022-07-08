using UnityEngine;

public class SpriteSorter : MonoBehaviour
{
    [SerializeField] private float _offset;

    private int _sortingOrderBase = 0;
    private Renderer _renderer;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
    }

    private void LateUpdate()
    {
        _renderer.sortingOrder = (int)(_sortingOrderBase - transform.position.y + _offset);
    }
}

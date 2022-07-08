using UnityEngine;

public class Clouds : MonoBehaviour
{
    [SerializeField] private GameObject[] _clouds;
    [SerializeField] private float _spawnPositionX;
    [SerializeField] private float _spawnRangeY;

    private void Start()
    {
        InvokeRepeating("SpawnCloud", 0, 10);
    }

    private void SpawnCloud()
    {
        var index = Random.Range(0, _clouds.Length);
        var position = new Vector3(_spawnPositionX, Random.Range(-_spawnRangeY, _spawnRangeY + 10), -9);
        Instantiate(_clouds[index], position, Quaternion.identity);
    }
}

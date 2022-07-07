using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject[] _spawnPositions;
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private float _spawnDelay;

    private void Start()
    {
        InvokeRepeating("SpawnEnemy", 0f, _spawnDelay);
    }

    private void SpawnEnemy()
    {
        var index = Random.Range(0, _spawnPositions.Length);
        var position = _spawnPositions[index].transform.position;

        Instantiate(_enemyPrefab, position, Quaternion.identity);
    }
}

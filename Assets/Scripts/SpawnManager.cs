using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject[] _spawnPositions;
    [SerializeField] private GameObject[] _enemyPrefab;
    [SerializeField] private float _spawnDelay;

    private void Start()
    {
        InvokeRepeating("SpawnEnemy", 0f, _spawnDelay);
    }

    private void SpawnEnemy()
    {
        var posIndex = Random.Range(0, _spawnPositions.Length);
        var enemyIndex = Random.Range(0, _enemyPrefab.Length);
        var position = _spawnPositions[posIndex].transform.position;

        Instantiate(_enemyPrefab[enemyIndex], position, Quaternion.identity);
    }
}

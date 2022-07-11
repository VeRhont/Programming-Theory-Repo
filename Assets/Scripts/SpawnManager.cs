using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject[] _spawnPositions;
    [SerializeField] private GameObject[] _enemyPrefabs;
    [SerializeField] private float _spawnDelay;

    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();    
    }

    private void Start()
    {
        InvokeRepeating("SpawnEnemy", 0f, _spawnDelay);
    }

    private void SpawnEnemy()
    {
        var posIndex = Random.Range(0, _spawnPositions.Length);
        var enemyIndex = Random.Range(0, _enemyPrefabs.Length);
        var position = _spawnPositions[posIndex].transform.position;

        _audioSource.Play();
        Instantiate(_enemyPrefabs[enemyIndex], position, Quaternion.identity);
    }
}
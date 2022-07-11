using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    [Header("Player Stats")]
    [SerializeField] private float _speed;
    [SerializeField] private float _maxHealth;
    [SerializeField] private float _countToWin;
    [SerializeField] private float _heal;

    [Header("Attack")]
    [SerializeField] private float _startTimeBetweenAttack;
    [SerializeField] private Transform _attackPosition;
    [SerializeField] private LayerMask _enemy;
    [SerializeField] private float _attackRange;
    [SerializeField] private int _damage;
    private float _timeBetweenAttack = 1f;

    [Header("UI")]
    [SerializeField] private Image _healthBarImage;
    [SerializeField] private TextMeshProUGUI _diamantCount;

    [Header("Sound")]
    [SerializeField] private AudioClip _attackSound;
    [SerializeField] private AudioClip _deathSound;
    [SerializeField] private AudioClip _diamantSound;
    [SerializeField] private AudioClip _healSound;
    [SerializeField] private AudioClip _openedPortalSound;

    [Header("Particles")]
    [SerializeField] private ParticleSystem _redParticles;
    [SerializeField] private ParticleSystem _groundParticles;

    private AudioSource _audioSource;

    private GameManager _gameManager;

    private Animator _playerAnimator;
    [SerializeField] private Animator _gatesAnimator;

    private SpriteRenderer _spriteRenderer;

    private float _health;
    private Rigidbody2D _playerRb;
    private Vector2 _direction;

    public void SavePlayer()
    {
        SaveSystem.SavePlayer(this);
        SceneManager.LoadScene(1);
    }

    public void LoadPlayer()
    {
        var data = SaveSystem.LoadPlayer();
        if (data == null)
            return;

        _health = data.Health;
        _countToWin = data.MaxCount;
        Wallet.Count = (int)data.Count;
        _diamantCount.SetText($"{Wallet.GetCount()}/{_countToWin}");
        transform.position = new Vector2(data.XPosition, data.YPosition);
    }

    private void Awake()
    {
        _gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        _audioSource = GetComponent<AudioSource>();

        _health = _maxHealth;
        _playerRb = GetComponent<Rigidbody2D>();
        _playerAnimator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();

        _diamantCount.SetText($"{Wallet.GetCount()}/{_countToWin}");
        LoadPlayer();

        UpdateHealth();
    }

    private void Update()
    {
        _direction.x = Input.GetAxis("Horizontal");
        _direction.y = Input.GetAxis("Vertical");

        if (_direction == new Vector2(0, 0))
        {
            _groundParticles.gameObject.SetActive(false);
            _playerAnimator.SetBool("IsMoving", false);
        }
        else
        {
            _groundParticles.gameObject.SetActive(true);
            //
            _spriteRenderer.flipX = _direction.x < 0 ? true : false; 
            //
            _playerAnimator.SetBool("IsMoving", true);
        }

        if (_timeBetweenAttack <= 0)
        {
            if (Input.GetMouseButton(0))
            {
                Attack();
            }
        }
        else
        {
            _timeBetweenAttack -= Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        _playerRb.MovePosition(_playerRb.position + _direction * _speed * Time.deltaTime);
    }

    private void UpdateHealth()
    {
        _healthBarImage.fillAmount = _health / _maxHealth;

        if (_health == 0)
        {
            _audioSource.PlayOneShot(_deathSound);
            _playerAnimator.SetBool("IsAlive", false);

            StartCoroutine("LoseGame");
        }
    }

    private IEnumerator LoseGame()
    {
        yield return new WaitForSeconds(1);
        _gameManager.IsGameActive = false;
        _gameManager.LoseGame();
    }

    public void GetDamage(float damage)
    {
        _health = Mathf.Max(0, _health - damage);
        UpdateHealth();
    }

    private void Attack()
    {
        _audioSource.PlayOneShot(_attackSound);
        _playerAnimator.SetTrigger("Attack");

        Collider2D[] enemies = Physics2D.OverlapCircleAll(_attackPosition.position, _attackRange, _enemy);

        if (enemies == null)
        {
            _timeBetweenAttack = _startTimeBetweenAttack;
            return;
        }

        foreach (var enemy in enemies)
        {
            try { enemy.GetComponent<Slime>().TakeDamage(_damage); }
            catch { }

            try { enemy.GetComponent<OrangeSlime>().TakeDamage(_damage); }
            catch { }
        }

        _timeBetweenAttack = _startTimeBetweenAttack;
    }

    private void Heal()
    {
        _audioSource.PlayOneShot(_healSound);
        _health = Mathf.Min(100, _health + _heal);
        UpdateHealth();
    }

    public float[] GetPlayerData()
    {
        return new float[] { _health, Wallet.GetCount(), _countToWin, transform.position.x, transform.position.y };
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Diamant"))
        {
            Destroy(collision.gameObject);
            Instantiate(_redParticles, collision.transform.position, _redParticles.transform.rotation);
            _audioSource.PlayOneShot(_diamantSound);

            Wallet.AddDiamant();
            _diamantCount.SetText($"{Wallet.GetCount()}/{_countToWin}");

            if (Wallet.GetCount() >= _countToWin)
            {
                _audioSource.PlayOneShot(_openedPortalSound);
                _gatesAnimator.SetBool("AreCollected", true);
            }
        }
        else if (collision.CompareTag("Heart"))
        {
            Destroy(collision.gameObject);
            Instantiate(_redParticles, collision.transform.position, _redParticles.transform.rotation);
            Heal();
        }
        else if (collision.CompareTag("Gates"))
        {
            if (Wallet.GetCount() >= _countToWin)
            {
                _gameManager.IsGameActive = false;
                _gameManager.WinGame();
            }
        }
    }
}

[System.Serializable] 
public class PlayerData
{
    public PlayerData(PlayerController player)
    {
        var data = player.GetPlayerData();
        Health = data[0];
        Count = data[1];
        MaxCount = data[2];
        XPosition = data[3];
        YPosition = data[4];
    }

    public float Health;
    public float Count;
    public float MaxCount;
    public float XPosition;
    public float YPosition;
}

public static class Wallet
{
    private static int _count;
    public static int Count
    {
        get { return _count; }
        set { _count = value; }
    }

    public static void AddDiamant()
    {
        _count++;
    }

    public static int GetCount() => _count;
}
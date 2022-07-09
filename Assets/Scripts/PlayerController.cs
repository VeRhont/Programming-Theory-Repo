using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

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

    [SerializeField] private GameManager _gameManager;

    private Animator _animator;
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
        _health = data.Health;
        _countToWin = data.MaxCount;
        Wallet.Count = (int)data.Count;
        transform.position = new Vector2(data.XPosition, data.YPosition);
    }

    private void Awake()
    {
        LoadPlayer();
        UpdateHealth();

        _health = _maxHealth;
        _playerRb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();

        _diamantCount.SetText($"{Wallet.GetCount()}/{_countToWin}");
    }

    private void Update()
    {
        _direction.x = Input.GetAxis("Horizontal");
        _direction.y = Input.GetAxis("Vertical");

        if (_direction == new Vector2(0, 0))
        {
            _animator.SetBool("IsMoving", false);
        }
        else
        {
            //
            _spriteRenderer.flipX = _direction.x < 0 ? true : false; 
            //
            _animator.SetBool("IsMoving", true);
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
    }

    public void GetDamage(float damage)
    {
        _health = Mathf.Max(0, _health - damage);
        UpdateHealth();
    }

    private void Attack()
    {
        _animator.SetTrigger("Attack");

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
        _health = Mathf.Min(100, _health + _heal);
        UpdateHealth();
    }

    public float[] GetPlayerData()
    {
        return new float[] { _health, Wallet.GetCount(), _countToWin, transform.position.x, transform.position.y };
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(_attackPosition.position, _attackRange);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Diamant"))
        {
            Destroy(collision.gameObject);

            Wallet.AddDiamant();
            _diamantCount.SetText($"{Wallet.GetCount()}/{_countToWin}");

            if (Wallet.GetCount() == _countToWin)
            {
                _gameManager.IsGameActive = false;
            }
        }
        else if (collision.CompareTag("Heart"))
        {
            Destroy(collision.gameObject);
            Heal();
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

    public static void AddDiamant() => _count++;

    public static int GetCount() => _count;
}
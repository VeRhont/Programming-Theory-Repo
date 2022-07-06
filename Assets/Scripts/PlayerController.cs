using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerController : MonoBehaviour
{
    [Header("Player Stats")]
    [SerializeField] private float _speed;
    [SerializeField] private float _maxHealth;
    [SerializeField] private int _countToWin;

    [Header("Attack")]
    [SerializeField] private float _startTimeBetweenAttack;
    [SerializeField] private Transform _attackPosition;
    [SerializeField] private LayerMask _enemy;
    [SerializeField] private float _attackRange;
    [SerializeField] private int _damage;
    private float _timeBetweenAttack;

    [Header("UI")]
    [SerializeField] private Image _healthBarImage;
    [SerializeField] private TextMeshProUGUI _diamantCount;

    private Animator _animator;
    private SpriteRenderer _spriteRenderer;

    private float _health;
    private Rigidbody2D _playerRb;
    private Vector2 _direction;

    private void Awake()
    {
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
            _spriteRenderer.flipX = _direction.x < 0 ? true : false;
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

        foreach (var enemy in enemies)
        {
            enemy.GetComponent<Enemy>().TakeDamage(_damage);
        }

        _timeBetweenAttack = _startTimeBetweenAttack;
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
                Debug.Log("You won!");
            }
        }
    }
}

public static class Wallet
{
    private static int _count;

    public static void AddDiamant() => _count++;

    public static int GetCount() => _count;
}
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Stats")]
    [SerializeField] private float _maxHealth;
    [SerializeField] private float _speed;
    [SerializeField] private float _damage;

    [SerializeField] private ParticleSystem _groundParticles;

    protected GameObject _player;
    protected Rigidbody2D _enemyRb;

    private float _health;

    protected Animator _animator;
    protected AudioSource _audioSource;

    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _health = _maxHealth;
        _enemyRb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _audioSource = GetComponent<AudioSource>();
    }

    private void FixedUpdate()
    {
        if (_player != null)
        {
            ChasePlayer();
        }
        else
        {
            _groundParticles.gameObject.SetActive(false);
        }
    }

    public virtual void ChasePlayer()
    {
        _groundParticles.gameObject.SetActive(true);

        var playerPosition = _player.transform.position;
        var enemyPosition = transform.position;

        var direction = -(enemyPosition - playerPosition).normalized;

        _enemyRb.MovePosition(enemyPosition + direction * _speed * Time.deltaTime);
    }

    public void TakeDamage(int damage)
    {
        _health = Mathf.Max(0, _health - damage);
        UpdateHealth();
    }

    private void UpdateHealth()
    {
        Debug.Log(_health);

        if (_health == 0)
        {
            Die();
        }
    }

    public virtual void Die()
    {
        Destroy(gameObject);
    }

    public virtual void Attack(Collision2D collision)
    {
        collision.gameObject.GetComponent<PlayerController>().GetDamage(_damage);
    }

    #region PlayerDetection
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _player = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _player = null;
        }
    }
    #endregion

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Attack(collision);
        }
    }
}
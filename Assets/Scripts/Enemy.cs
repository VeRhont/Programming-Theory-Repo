using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Stats")]
    [SerializeField] private float _maxHealth;
    [SerializeField] private float _speed;
    [SerializeField] private float _damage;

    protected GameObject _player;
    protected Rigidbody2D _enemyRb;

    private float _health;
    private Animator _animator;

    private void Awake()
    {
        _health = _maxHealth;
        _enemyRb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        if (_player != null)
        {
            ChasePlayer();
        }
    }

    public virtual void ChasePlayer()
    {
        if (_player != null)
        {
            _animator.SetBool("BoolJump", true);
        }
        else
        {
            _animator.SetBool("BoolJump", false);
        }

        var playerPosition = _player.transform.position;
        var enemyPosition = transform.position;

        var direction = -(enemyPosition - playerPosition).normalized;

        _enemyRb.MovePosition(enemyPosition + direction * _speed * Time.deltaTime);
    }

    private void GetDamage()
    {

    }

    private void UpdateHealth()
    {

    }

    private void Die()
    {

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
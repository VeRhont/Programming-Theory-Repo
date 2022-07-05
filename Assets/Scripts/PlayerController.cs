using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("Player Stats")]
    [SerializeField] private float _speed;
    [SerializeField] private float _maxHealth;

    [Header("UI")]
    [SerializeField] private Image _healthBarImage;

    private float _health;
    private Rigidbody2D _playerRb;
    private Vector2 _direction;

    private void Awake()
    {
        _health = _maxHealth;
        _playerRb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        _direction.x = Input.GetAxis("Horizontal");
        _direction.y = Input.GetAxis("Vertical");
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
}

using UnityEngine;

public class OrangeSlime : Enemy
{
    [SerializeField] private GameObject _dropObject;
    [SerializeField] private ParticleSystem _deathParticles;
    [SerializeField] private AudioClip _deathSound;

    public override void ChasePlayer()
    {
        base.ChasePlayer();

        if (_player != null)
        {
            _animator.SetBool("IsMoving", true);
        }
        else
        {
            _animator.SetBool("IsMoving", false);
        }
    }

    public override void Attack(Collision2D collision)
    {
        _animator.SetTrigger("Attack");
        _enemyRb.velocity = new Vector2(0, 0);
        base.Attack(collision);
    }

    public override void Die()
    {
        var spawnPosition = transform.position;
        Instantiate(_dropObject, spawnPosition, Quaternion.identity);
        Instantiate(_deathParticles, spawnPosition, _deathParticles.transform.rotation);
        _audioSource.PlayOneShot(_deathSound);

        base.Die();
    }
}

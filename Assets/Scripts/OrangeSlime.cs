using UnityEngine;

public class OrangeSlime : Enemy
{
    [SerializeField] private GameObject _dropObject;

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

    public override void Die()
    {
        var spawnPosition = transform.position;
        Instantiate(_dropObject, spawnPosition, Quaternion.identity);
        base.Die();
    }
}

using UnityEngine;

public class Slime : Enemy
{
    [SerializeField] private GameObject _dropObject;

    public override void Attack(Collision2D collision)
    {
        base.Attack(collision);
    }

    public override void Die()
    {
        var spawnPosition = transform.position;
        Instantiate(_dropObject, spawnPosition, Quaternion.identity);
        base.Die();
        
    }
}

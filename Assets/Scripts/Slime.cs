using UnityEngine;

public class Slime : Enemy
{
    public override void Attack(Collision2D collision)
    {
        Debug.Log("override");
        base.Attack(collision);
        transform.Translate(new Vector3(-0.5f, 0f, 0f));
    }
}

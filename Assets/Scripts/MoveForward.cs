using UnityEngine;

public class MoveForward : MonoBehaviour
{
    [SerializeField] private float _speed;

    private void FixedUpdate()
    {
        transform.Translate(new Vector2(_speed * Time.deltaTime, 0));

        if (transform.position.x >= 50)
        {
            Destroy(gameObject);
        }
    }
}

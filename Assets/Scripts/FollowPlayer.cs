using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField] private GameObject _player;

    private void LateUpdate()
    {
        transform.position = _player.transform.position + new Vector3(0, 0, -10);
    }
}

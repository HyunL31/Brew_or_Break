using UnityEngine;

public class CameraMoving : MonoBehaviour
{
    public GameObject _player;

    private void LateUpdate()
    {
        if (_player == null)
        {
            return;
        }

        Vector3 targetPos = new Vector3(_player.transform.position.x, _player.transform.position.y, -10f);
        this.transform.position = targetPos;
    }

    private void SetTarget(GameObject player)
    {
        _player = player;
    }
}
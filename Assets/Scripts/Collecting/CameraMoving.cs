using UnityEngine;

public class CameraMoving : MonoBehaviour
{
    private GameObject _player;

    private void Start()
    {
        CollectingManager.Inst.SetCamera(this);
    }

    private void LateUpdate()
    {
        if (_player == null)
        {
            return;
        }

        Vector3 targetPos = new Vector3(_player.transform.position.x, _player.transform.position.y, -10f);
        this.transform.position = targetPos;
    }

    public void SetTarget(GameObject player)
    {
        _player = player;
    }
}
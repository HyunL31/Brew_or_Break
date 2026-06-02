using UnityEngine;

/// <summary>
/// 사냥 콘텐츠 카메라 (플레이어 따라가기)
/// </summary>

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
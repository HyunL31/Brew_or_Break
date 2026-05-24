using UnityEngine;

public class CollectingManager : MonoBehaviour
{
    public static CollectingManager Inst;

    private CameraMoving _camera;
    private PlayerMoving _player;

    private void Awake()
    {
        Inst = this;
    }

    public void SetCamera(CameraMoving camera)
    {
        _camera = camera;
    }

    public PlayerMoving GetPlayer()
    {
        return _player;
    }

    public void SetCollectingMap()
    {
        string playerPath = "Prefabs/Collecting/Player";
        string mapPath = "Prefabs/Collecting/Map";

        ResourceManager.Inst.InstantiatePrefab(playerPath, null, (player) =>
        {
            player.transform.position = Vector3.zero;
            SetCameraTarget(player);

            PlayerMoving playerMoving = player.GetComponent<PlayerMoving>();
            _player = playerMoving;
        });

        ResourceManager.Inst.InstantiatePrefab(mapPath, null, (map) =>
        {
            map.transform.position = Vector3.zero;
        });
    }

    private void SetCameraTarget(GameObject player)
    {
        _camera.SetTarget(player);
    }
}

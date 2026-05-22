using UnityEngine;

public class CollectingManager : MonoBehaviour
{
    [SerializeField] private CameraMoving Camera;
    [SerializeField] public PlayerMoving Player;

    public static CollectingManager Inst;

    private void Awake()
    {
        Inst = this;
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
            Player = playerMoving;
        });

        ResourceManager.Inst.InstantiatePrefab(mapPath, null, (map) =>
        {
            map.transform.position = Vector3.zero;
        });
    }

    private void SetCameraTarget(GameObject player)
    {
        Camera.SetTarget(player);
    }
}

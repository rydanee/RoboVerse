using UnityEngine;

public class SpawnDetail : MonoBehaviour
{
  public GameObject detailToSpawn;
  public void spawnObject()
  {
    Camera camera = CanvasController.Instance.camera;

    Vector3 spawnPos = camera.transform.position;
    spawnPos.y = 2.0f;

    Instantiate(detailToSpawn, spawnPos, detailToSpawn.transform.rotation * Quaternion.Euler(0, 0, 180f));

    CanvasController.Instance.toggleCanvas(CanvasController.Instance.defaultButtonText);

  }
}

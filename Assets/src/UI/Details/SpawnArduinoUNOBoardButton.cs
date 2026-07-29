using UnityEngine;

public class SpawnArduinoUNOBoardButton : MonoBehaviour
{
  [SerializeField] private GameObject arduinoUNOBoardObject;

  public void spawnArduinoUNOBoard()
  {
    Camera camera = CanvasController.Instance.camera;

    Vector3 spawnPos = camera.transform.position + (camera.transform.forward * 2f);

    Instantiate(arduinoUNOBoardObject, spawnPos, arduinoUNOBoardObject.transform.rotation * Quaternion.Euler(0, 0, 180f));

    CanvasController.Instance.toggleCanvas(CanvasController.Instance.defaultButtonText);

  }
}

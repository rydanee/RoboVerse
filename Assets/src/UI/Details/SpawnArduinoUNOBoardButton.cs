using UnityEngine;

public class SpawnArduinoUNOBoardButton : MonoBehaviour
{
  [SerializeField] private GameObject arduinoUNOBoardObject;

  public void spawnArduinoUNOBoard()
  {
    Camera camera = CanvasController.Instance.camera;

    Vector3 spawnPos = camera.transform.position;
    spawnPos.y = 2.0f;

    Instantiate(arduinoUNOBoardObject, spawnPos, arduinoUNOBoardObject.transform.rotation * Quaternion.Euler(0, 0, 180f));

    Debug.Log(arduinoUNOBoardObject.transform.position.y);

    CanvasController.Instance.toggleCanvas(CanvasController.Instance.defaultButtonText);

  }
}

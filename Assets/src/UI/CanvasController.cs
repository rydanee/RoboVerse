using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CanvasController : MonoBehaviour
{
  public static CanvasController Instance { get; private set; }
  public string defaultButtonText { get; private set; }
  public Camera camera { get; private set; }

  [SerializeField] private Camera mainCamera;
  [SerializeField] private Canvas canvas;
  [SerializeField] private TextMeshProUGUI buttonText;

  private bool isEnabled = false;

  private void Awake()
  {
    if (Instance == null)
    {
      Instance = this;
      this.camera = mainCamera;
    }
    else
    {
      Destroy(gameObject);
    }
  }

  void Start()
  {
    canvas.enabled = isEnabled;
  }

  public void toggleCanvas(string defaultButtonText)
  {
    isEnabled = !isEnabled;
    canvas.enabled = isEnabled;

    this.defaultButtonText = defaultButtonText;

    buttonText.text = isEnabled ? "Close" : defaultButtonText;
  }
}

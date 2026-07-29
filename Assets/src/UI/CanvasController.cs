using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CanvasController : MonoBehaviour
{
  public static CanvasController Instance { get; private set; }

  [SerializeField] private Canvas canvas;
  [SerializeField] private TextMeshProUGUI buttonText;

  private bool isEnabled = false;

  private void Awake()
  {
    if (Instance == null)
    {
      Instance = this;
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

    buttonText.text = isEnabled ? "Close" : defaultButtonText;
  }
}

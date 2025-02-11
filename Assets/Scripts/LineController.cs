using UnityEngine;
using UnityEngine.UI;

public class LineController : MonoBehaviour
{
    public bool isHorizontal;
    public int row, col;

    private Toggle toggle;
    private GameManager gameManager;

    void Start()
    {
        toggle = GetComponent<Toggle>();
        gameManager = FindObjectOfType<GameManager>();

        // Listen for toggle changes
        toggle.onValueChanged.AddListener(OnToggleChanged);
    }

    void OnToggleChanged(bool isOn)
    {
        if (isOn)
        {
            gameManager.HandleClick(row, col, isHorizontal);
        }
    }
}

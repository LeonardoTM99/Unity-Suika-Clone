using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject startMenuPanel;



    private void Start()
    {
        startMenuPanel.SetActive(true);
    }

    public void StartGame()
    {
        startMenuPanel.SetActive(false);
        GameStateManager.Instance.SetState(GameStateManager.GameState.Aiming);
    }


}

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuNavigation : MonoBehaviour
{
    public Button[] menuButtons;
    public RectTransform arrow;

    public GameObject mainMenuPanel;
    public GameObject optionsPanel;

    private int currentIndex = 0;

    void Start()
    {
        ShowMainMenu();
        UpdateSelection();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            currentIndex++;
            if (currentIndex >= menuButtons.Length)
                currentIndex = 0;

            UpdateSelection();
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            currentIndex--;
            if (currentIndex < 0)
                currentIndex = menuButtons.Length - 1;

            UpdateSelection();
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            menuButtons[currentIndex].onClick.Invoke();
        }
    }

    void UpdateSelection()
    {
        menuButtons[currentIndex].Select();
        arrow.position = menuButtons[currentIndex].transform.position;
    }

    // MENU STATES
    public void ShowMainMenu()
    {
        mainMenuPanel.SetActive(true);
        optionsPanel.SetActive(false);
    }

    public void ShowOptions()
    {
        mainMenuPanel.SetActive(false);
        optionsPanel.SetActive(true);
    }

    // BUTTON FUNCTIONS
    public void StartGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game");
        Application.Quit();
    }
}
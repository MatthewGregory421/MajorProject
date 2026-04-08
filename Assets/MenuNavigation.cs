using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("Panels")]
    public GameObject mainMenuPanel;
    public GameObject optionsPanel;

    [Header("Main Menu Buttons")]
    public Button startButton;
   // public Button optionsButton;
    public Button quitButton;

    [Header("Options Menu UI")]
    public Slider masterVolumeSlider;
    public TMP_Text volumeText;
    public Toggle fullscreenToggle;
    public Toggle screenShakeToggle;
    public Button backButton;
    public Image sliderHighlight;

    [Header("Arrow Indicator")]
    public RectTransform arrow;
    public Vector2 arrowOffset = new Vector2(-50, 0);

    private Selectable[] mainButtons;
    private Selectable[] optionButtons;
    private int currentIndex = 0;
    private bool inOptions = false;
    private bool isEditingSlider = false;

    private float inputDelay = 0.15f;
    private float inputTimer = 0f;

    private bool waitForEnterRelease = false;

    void Start()
    {
        mainButtons = new Selectable[] { startButton, quitButton };
        optionButtons = new Selectable[] { masterVolumeSlider, fullscreenToggle, screenShakeToggle, backButton };

        // Load saved values
        masterVolumeSlider.value = PlayerPrefs.GetFloat("MasterVolume", 1f);
        fullscreenToggle.isOn = PlayerPrefs.GetInt("Fullscreen", 1) == 1;
        screenShakeToggle.isOn = PlayerPrefs.GetInt("ScreenShake", 1) == 1;

        UpdateVolumeText(masterVolumeSlider.value);
        ShowMainMenu();
    }

    void Update()
    {
        if (inputTimer > 0f)
        {
            inputTimer -= Time.deltaTime;
            return;
        }

        if (!inOptions)
        {
            if (waitForEnterRelease)
            {
                if (!Input.GetKey(KeyCode.Return)) waitForEnterRelease = false;
                else return;
            }

            // Main menu navigation
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                currentIndex = (currentIndex + 1) % mainButtons.Length;
                UpdateArrow();
                inputTimer = inputDelay;
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                currentIndex--;
                if (currentIndex < 0) currentIndex = mainButtons.Length - 1;
                UpdateArrow();
                inputTimer = inputDelay;
            }

            if (Input.GetKeyDown(KeyCode.Return))
            {
                Button currentButton = mainButtons[currentIndex] as Button;
                if (currentButton == startButton) StartGame();
                //else if (currentButton == optionsButton) ShowOptionsMenu();
                else if (currentButton == quitButton) QuitGame();

                inputTimer = inputDelay;
            }
        }
        else
        {
            Selectable current = optionButtons[currentIndex];

            // Navigation
            if (!isEditingSlider)
            {
                if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    currentIndex = (currentIndex + 1) % optionButtons.Length;
                    UpdateArrow();
                    inputTimer = inputDelay;
                }
                else if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    currentIndex--;
                    if (currentIndex < 0) currentIndex = optionButtons.Length - 1;
                    UpdateArrow();
                    inputTimer = inputDelay;
                }
            }

            // Enter key for toggles, slider, back button
            if (Input.GetKeyDown(KeyCode.Return))
            {
                if (current == masterVolumeSlider)
                {
                    isEditingSlider = !isEditingSlider;
                    sliderHighlight.enabled = isEditingSlider;
                }
                else if (current is Toggle toggle)
                {
                    toggle.isOn = !toggle.isOn;
                    if (toggle == fullscreenToggle) Screen.fullScreen = toggle.isOn;
                    PlayerPrefs.SetInt(toggle == fullscreenToggle ? "Fullscreen" : "ScreenShake", toggle.isOn ? 1 : 0);
                    PlayerPrefs.Save();
                }
                else if (current == backButton)
                {
                    CloseOptionsMenu();
                }

                inputTimer = inputDelay;
            }

            // Slider adjustment
            if (isEditingSlider && current == masterVolumeSlider)
            {
                float step = 0.05f;
                if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    masterVolumeSlider.value = Mathf.Clamp01(masterVolumeSlider.value + step);
                    UpdateVolumeText(masterVolumeSlider.value);
                    PlayerPrefs.SetFloat("MasterVolume", masterVolumeSlider.value);
                    PlayerPrefs.Save();
                    inputTimer = inputDelay;
                }
                else if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    masterVolumeSlider.value = Mathf.Clamp01(masterVolumeSlider.value - step);
                    UpdateVolumeText(masterVolumeSlider.value);
                    PlayerPrefs.SetFloat("MasterVolume", masterVolumeSlider.value);
                    PlayerPrefs.Save();
                    inputTimer = inputDelay;
                }
            }
        }
    }

    void UpdateArrow()
    {
        Selectable current = inOptions ? optionButtons[currentIndex] : mainButtons[currentIndex];
        arrow.position = (Vector2)current.transform.position + arrowOffset;
        EventSystem.current.SetSelectedGameObject(current.gameObject);
    }

    void UpdateVolumeText(float value)
    {
        volumeText.text = Mathf.RoundToInt(value * 100f).ToString();
    }

    public void ShowMainMenu()
    {
        mainMenuPanel.SetActive(true);
        optionsPanel.SetActive(false);
        inOptions = false;
        currentIndex = 0;
        UpdateArrow();
        waitForEnterRelease = true;
    }

    public void ShowOptionsMenu()
    {
        mainMenuPanel.SetActive(false);
        optionsPanel.SetActive(true);
        inOptions = true;
        currentIndex = 0;
        isEditingSlider = false;
        sliderHighlight.enabled = false;
        UpdateArrow();
    }

    public void CloseOptionsMenu()
    {
        ShowMainMenu();
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Sadness Proto");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
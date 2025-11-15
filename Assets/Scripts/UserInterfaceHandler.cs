using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UserInterfaceHandler : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private AudioSource ballAudio;
    [SerializeField] private AudioSource arCameraAudio;
    [SerializeField] private TextMeshProUGUI soundButtonText;
    [SerializeField] private AudioClip testSound;
    [SerializeField] private SaveSystem saveSystem;
    [SerializeField] private GameObject startInfo;

    private bool _startInfoEnabledOldState;

    private void Start()
    {
        soundButtonText.text = saveSystem.gameData.isMute ? "Звук: выкл." : "Звук: вкл.";
        arCameraAudio.mute = ballAudio.mute = saveSystem.gameData.isMute;
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        _startInfoEnabledOldState = startInfo.gameObject.activeSelf;
        startInfo.gameObject.SetActive(false);
        pauseMenu.gameObject.SetActive(true);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        pauseMenu.gameObject.SetActive(false);
        startInfo.gameObject.SetActive(_startInfoEnabledOldState);
    }

    public void RestartGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoToMainMenuScene()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }

    public void ToggleMute()
    {
        saveSystem.gameData.isMute = !saveSystem.gameData.isMute;
        saveSystem.Save();
        soundButtonText.text = saveSystem.gameData.isMute ? "Звук: выкл." : "Звук: вкл.";
        arCameraAudio.mute = ballAudio.mute = saveSystem.gameData.isMute;
        if (!saveSystem.gameData.isMute) arCameraAudio.PlayOneShot(testSound, 0.5F);
    }
}
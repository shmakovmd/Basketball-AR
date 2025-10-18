using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuBehaviour : MonoBehaviour
{
    public void GoToGame()
    {
        SceneManager.LoadScene("Game");
    }
}
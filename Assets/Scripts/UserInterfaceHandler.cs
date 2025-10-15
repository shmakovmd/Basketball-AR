using UnityEngine;
using UnityEngine.SceneManagement;

public class UserInterfaceHandler : MonoBehaviour
{
    public void RestartGameScene()
    {
        SceneManager.LoadScene("Game");
    }
}
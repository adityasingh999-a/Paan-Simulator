using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
public static GameManager Instance;


public bool isGameRunning = true;
public bool isGameOver = false;
public int customersServed = 0;

void Awake()
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
    Time.timeScale = 1f;
    isGameRunning = true;
    isGameOver = false;
}

void Update()
{
    if (KeyboardEscapePressed())
    {
        if (isGameRunning)
        {
            PauseGame();
        }
        else if (!isGameOver)
        {
            ResumeGame();
        }
    }
}

bool KeyboardEscapePressed()
{
    return UnityEngine.InputSystem.Keyboard.current != null &&
           UnityEngine.InputSystem.Keyboard.current.escapeKey.wasPressedThisFrame;
}

public void PauseGame()
{
    isGameRunning = false;
    Time.timeScale = 0f;
    Cursor.visible = true;
    Cursor.lockState = CursorLockMode.None;
}

public void ResumeGame()
{
    if (isGameOver)
        return;

    isGameRunning = true;
    Time.timeScale = 1f;
    Cursor.visible = false;
    Cursor.lockState = CursorLockMode.Confined;
}

public void AddCustomerServed()
{
    customersServed++;
}

public void GameOver()
{
    isGameOver = true;
    isGameRunning = false;
    Time.timeScale = 0f;
    Cursor.visible = true;
    Cursor.lockState = CursorLockMode.None;
}

public void RestartGame()
{
    Time.timeScale = 1f;

    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
}

public void QuitGame()
{
    Time.timeScale = 1f;

    Application.Quit();
}


}

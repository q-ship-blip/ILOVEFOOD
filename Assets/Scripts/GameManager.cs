using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public string levelBeforeDeath;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Keep between scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void GoToDeathScene()
    {
        levelBeforeDeath = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene("Death"); // Change to your death scene name
    }

    public void Respawn()
    {
        SceneManager.LoadScene(levelBeforeDeath);
    }
}

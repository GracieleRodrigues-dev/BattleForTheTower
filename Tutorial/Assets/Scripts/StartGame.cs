
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    public void ReiniciarJogo(){
        SceneManager.LoadScene(1);
    }
    public void SairJogo()
    {
        Application.Quit();
    }
}
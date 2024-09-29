
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndGame : MonoBehaviour
{
    public Text textScore,textStatus;

    private void Start()
    {
        textScore.text = "SEU SCORE: " + ScoreManager.Instance.getScore().ToString();
        textStatus.text = ScoreManager.Instance.getStatus().ToString();
    }
    public void ReiniciarJogo(){
        SceneManager.LoadScene(1);
    }
    public void SairJogo()
    {
        Application.Quit();
    }
}
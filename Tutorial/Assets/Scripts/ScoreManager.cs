using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using SunTemple;
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    public Text textScore;
    public Text notificationText; 
    public AudioClip doorUnlockSound; 
    private AudioSource audioSource; 
    private int score = 0;
    private int inimigosMortos = 0;
    private bool BossMorto = false;
    private NavMeshObstacle navMeshObstacle;
    public AudioClip[] ambientSounds; 

    private AudioSource ambientAudioSource;

    private string statusEndGame;

    public Door finalDoor;

    public static ScoreManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        textScore.text = "SCORE: " + score.ToString();
        audioSource = GetComponent<AudioSource>(); 
        navMeshObstacle = FindObjectOfType<NavMeshObstacle>();
        navMeshObstacle.enabled = true; //tive que adicionar um obstaculo para os inimigos não sairem do templo   
        ambientAudioSource = gameObject.AddComponent<AudioSource>();
        ambientAudioSource.loop = true; 
        ambientAudioSource.volume = 0.5f; 
        TocarSomAmbiente(0);
        
    }

    void Update()
    {
        if (inimigosMortos >= 10)
        {
            DestrancarPorta("Door1","Nova sala desbloqueada! Mate o boss para desbloquear outra sala");
        }
        if (BossMorto)
        {   
            DestrancarPorta("MainDoor","");
            DestrancarPorta("Door2","Nova sala desbloqueada! Capture seu mascote e alcance a torre para vencer o jogo!");
            navMeshObstacle.enabled = false; //removendo o obstáculo para o mascote conseguir sair do templo
            ambientAudioSource.volume = 1.5f; 
            TrocarSomAmbiente(1);
        }

        if (!finalDoor.DoorClosed){

            TrocarSomAmbiente(2); //som vitória!
            ScoreManager.Instance.setStatus("VOCÊ VENCEU!");
            Invoke("endGame", 5f);
        }
        
    }

    public void AdicionarPontos(int pontos, int inimigoMorto)
    {
        score += pontos;
        inimigosMortos += inimigoMorto;
        textScore.text = "SCORE: " + score.ToString();
    }

    public void AtualizaBossMorto(bool estaMorto)
    {
        BossMorto = estaMorto;
    }

    void DestrancarPorta(string tagPorta, string msg)
    {
        Door[] portas = FindObjectsOfType<Door>();

        foreach (Door porta in portas)
        {
            if (porta.CompareTag(tagPorta) && porta.IsLocked)
            {
                porta.IsLocked = false;
                MostrarMensagem(msg);
                TocarSomPorta();
            }
        }
    }

    void MostrarMensagem(string mensagem)
    {
        notificationText.text = mensagem;
        StartCoroutine(LimparMensagem());
    }

    IEnumerator LimparMensagem()
    {
        yield return new WaitForSeconds(5); 
        notificationText.text = "";
    }

    void TocarSomPorta()
    {
        if (doorUnlockSound != null)
        {
            audioSource.PlayOneShot(doorUnlockSound);
        }
    }

    void TrocarSomAmbiente(int index)
    {
        if (index >= 0 && index < ambientSounds.Length)
        {
            ambientAudioSource.Stop(); // Para o som atual
            TocarSomAmbiente(index); // Toca o novo som
        }
    }    
    void TocarSomAmbiente(int index)
    {
        if (ambientSounds != null && ambientSounds.Length > index)
        {
            ambientAudioSource.clip = ambientSounds[index];
            ambientAudioSource.Play();
        }
    }    

    public int getScore(){
        return score;
    }

    public string getStatus(){
        return statusEndGame;
    }

    public void setStatus(string text){
        statusEndGame = text;
    }

    private void endGame()
    {
        SceneManager.LoadScene(2);
    }
}

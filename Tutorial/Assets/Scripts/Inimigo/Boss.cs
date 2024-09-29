using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using SunTemple;

public class Boss : MonoBehaviour, ILevarDano
{
    private NavMeshAgent agente;
    private GameObject player;
    private Animator anim;
    private AudioSource audioSrc;
    public AudioClip somMorte,somDano,somPasso,somGrunhir;
    public float distanciaDoAtaque = 3.5f;
    private int vida = 100;
    private PatrulharAleatorio pal;
    public int pointValue = 30;
    private FieldOfView fov;

    private ScoreManager scoreManager;
    public float velocidade = 7f;

    private Door portas;
    
    // Start is called before the first frame update
    void Start()
    {   

        agente = GetComponent<NavMeshAgent>();
        player = GameObject.FindWithTag("Player");
        anim = GetComponent<Animator>();
        audioSrc = GetComponent<AudioSource>();
        fov = GetComponent<FieldOfView>();
        pal = GetComponent<PatrulharAleatorio>();
        scoreManager = GameObject.FindObjectOfType<ScoreManager>();
        agente.speed = velocidade;
        agente.enabled = false;
        anim.SetBool("podeAndar",false);
    }

    // Update is called once per frame
    void Update()
    {
        //VaiAtrasJogador();
        //OlharParaJogador();
        PortaBossIsClosed();        
        if (agente.enabled){ 
            if(vida <=0){
                Morrer();
                return;
            }

            if (fov.podeVerPlayer)
            {
                VaiAtrasJogador();
                // Tocar som de grunhir ao ver o jogador
                if (audioSrc.clip != somGrunhir || !audioSrc.isPlaying){
                    Grunhir();
                }    
            }else 
            {
                anim.SetBool("pararAtaque",true);
                CorrigirRigidSair();
                agente.isStopped= false;
                pal.Andar();
            }
        }
    }

    private void VaiAtrasJogador(){
        float distanciaDoPlayer = Vector3.Distance(transform.position,player.transform.position);

        if(distanciaDoPlayer < distanciaDoAtaque)
        {
            agente.isStopped = true;
            anim.SetTrigger("ataque");
            anim.SetBool("podeAndar",false);
            anim.SetBool("pararAtaque",false);
            CorrigirRigidEntrar();
        }

        if(distanciaDoPlayer >= distanciaDoAtaque +1)
        {
            anim.SetBool("pararAtaque",true);
            CorrigirRigidSair();
        }
        if(anim.GetBool("podeAndar")){
            agente.isStopped = false;
            agente.SetDestination(player.transform.position);
            anim.ResetTrigger("ataque");
        }
    }

    private void OlharParaJogador(){
        Vector3 direcaoOlhar = player.transform.position-transform.position;
        Quaternion rotacao = Quaternion.LookRotation(direcaoOlhar);
        transform.rotation = Quaternion.RotateTowards(transform.rotation,rotacao, Time.deltaTime*300);
    }

    private void CorrigirRigidEntrar(){
        GetComponent<Rigidbody>().isKinematic = true;
    }
    private void CorrigirRigidSair(){
        GetComponent<Rigidbody>().isKinematic = false;
    }

    public void LevarDano(int dano){
        vida -= dano;
        agente.isStopped = true;
        anim.SetTrigger("levouTiro");
        anim.SetBool("podeAndar",false);
        audioSrc.clip = somDano;
        audioSrc.Play();
    }

    private void Morrer(){
        audioSrc.clip = somMorte;
        audioSrc.Play();
        agente.isStopped = true;
        anim.SetBool("podeAndar",false);
        anim.SetBool("pararAtaque",true);
        anim.SetBool("morreu", true);
        scoreManager.AdicionarPontos(pointValue,0);
        scoreManager.AtualizaBossMorto(true);
        this.enabled = false;
    }

    public void DarDano(){
        player.GetComponent<MovimentarPersonagem>().AtualizarVida(-20);
    }

    public void Passo()
    {
        audioSrc.PlayOneShot(somPasso,0.5f);
    }

    private void Grunhir()
    {
        if (somGrunhir != null) 
        {
            audioSrc.clip = somGrunhir;
            audioSrc.Play(); // Toca o som de grunhir
        }
    }    

    private void PortaBossIsClosed(){
       
       //vamos verificar se a porta do boss está destrancada
        Door[] portas = FindObjectsOfType<Door>();
        foreach (Door porta in portas){
            if (porta.CompareTag("Door1"))
            {
                if (porta.DoorClosed)
                {
                   agente.enabled = false;
                   anim.SetBool("podeAndar",false);
                } else
                {
                    agente.enabled = true;
                    anim.SetBool("podeAndar",true);
                }
            }
        }
    }
}

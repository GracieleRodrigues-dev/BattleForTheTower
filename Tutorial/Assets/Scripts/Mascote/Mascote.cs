using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using SunTemple;

public class Mascote : MonoBehaviour, ILevarDano
{
    public int vida = 100;  // Vida inicial do mascote
    public int danoAtaque = 3;  // Dano causado ao inimigo
    public float distanciaVisao = 30.0f;  // Distância de visão do mascote
    public float distanciaAtaque = 2.0f;  // Distância para começar o ataque
    public float campoVisao = 180.0f;  // Campo de visão em graus
    public Slider sliderVida;  // Referência à barra de vida no HUD
    public AudioClip somMorte, somDano, somAtaque;  // Sons

    private NavMeshAgent agente;
    private GameObject heroi;
    private GameObject inimigoAtual;
    private Animator anim;
    private AudioSource audioSrc;
    private FieldOfViewMascote fov;
    private bool estaAtacando = false;

    private RaycastHit hit;
    private Door portas;
    public float velocidade = 6f;
    void Start()
    {
        agente = GetComponent<NavMeshAgent>();
        heroi = GameObject.FindWithTag("Player");
        anim = GetComponent<Animator>();
        audioSrc = GetComponent<AudioSource>();
        fov = GetComponent<FieldOfViewMascote>();
        agente.enabled = false;
        sliderVida.gameObject.SetActive(false);
        agente.speed = velocidade;
    }

    void Update()
    {
        PortaMascoteIsLocked();

        if (agente.enabled){
            sliderVida.gameObject.SetActive(true);
            sliderVida.value = vida;
            if (vida <= 0)
            {
                Morrer();
                return;
            }

            // Se ver inimigos, vai até eles e ataca
            if (fov.podeVerInimigo)
            {
                VaiAtrasInimigo(fov.inimigoVisivel);
            } else if (heroi.GetComponent<MovimentarPersonagem>().getVida() < 100 && fov.podeVerCaixaDeVida) // Se herói estiver ferido, procurar caixas de vida
            {
                VaiAtrasCaixaDeVida(fov.caixaDeVidaVisivel);
            } else // Caso contrário, segue o herói
            {
                SeguirHeroi();
            }
        }
    }

    private void VaiAtrasInimigo(GameObject inimigo)
    {
        float distanciaDoInimigo = Vector3.Distance(transform.position, inimigo.transform.position);
        inimigoAtual = inimigo;

        if (distanciaDoInimigo < distanciaAtaque)
        {
            agente.isStopped = true;
            if (!estaAtacando)
            {
                anim.SetTrigger("ataque");
                estaAtacando = true;
            }
        }
        else
        {
            anim.SetBool("podeAndar", true);
            agente.SetDestination(inimigo.transform.position);
            agente.isStopped = false;
            estaAtacando = false;
        }
    }

    private void SeguirHeroi()
    {
        float distanciaDoHeroi = Vector3.Distance(transform.position, heroi.transform.position);
        if (distanciaDoHeroi > 2.0f)
        {
            anim.SetBool("podeAndar", true);
            anim.SetBool("pararAtaque",true);
            agente.SetDestination(heroi.transform.position);
            agente.isStopped = false;
        }
        else
        {
            anim.SetBool("podeAndar", false);
        }
    }

    private void VaiAtrasCaixaDeVida(GameObject caixaDeVida)
    {
        float distanciaCaixa = Vector3.Distance(transform.position, caixaDeVida.transform.position);

        if (distanciaCaixa < 2.0f)
        {
            agente.isStopped = true;
            // Reabastecer vida do herói ao tocar na caixa
            heroi.GetComponent<MovimentarPersonagem>().AtualizarVida(100);
            Destroy(caixaDeVida);  // Destruir caixa de vida após uso
        }
        else
        {
            agente.SetDestination(caixaDeVida.transform.position);
            agente.isStopped = false;
        }
    }


    private void Morrer()
    {
        audioSrc.PlayOneShot(somMorte);
        anim.SetBool("morreu", true);
        agente.isStopped = true;
        this.enabled = false;   
        sliderVida.gameObject.SetActive(false);
    }


    // Eventos de animação
    public void Passo()
    {
        // Som de passos do mascote
    }

    public void Ataque()
    {
        if(hit.transform.tag == "LevarDano" )
            {
                ILevarDano levarDano = hit.transform.GetComponent<ILevarDano>();
                
                levarDano.LevarDano(5);
                audioSrc.PlayOneShot(somAtaque);
            }
        
    }

    public void LevarDano(int dano){
        vida -= dano;
        agente.isStopped = true;
        anim.SetTrigger("levouTiro");
        anim.SetBool("podeAndar",false);
        audioSrc.clip = somDano;
        audioSrc.Play();
    }    

    private void PortaMascoteIsLocked(){
       
       //vamos verificar se a porta do boss está destrancada
        Door[] portas = FindObjectsOfType<Door>();
        foreach (Door porta in portas){
            if (porta.CompareTag("Door2"))
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

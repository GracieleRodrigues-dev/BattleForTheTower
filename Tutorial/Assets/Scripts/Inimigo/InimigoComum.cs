using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class InimigoComum : MonoBehaviour, ILevarDano
{
    private NavMeshAgent agente;
    private GameObject player;
    private GameObject mascote;
    private Animator anim;
    private AudioSource audioSrc;
    public AudioClip somMorte, somDano, somPasso;
    public float distanciaDoAtaque = 2.0f;
    public int vida = 50;
    private PatrulharAleatorio pal;
    public int pointValue = 10;
    private FieldOfView fov;
    private ScoreManager scoreManager;

    void Start()
    {
        agente = GetComponent<NavMeshAgent>();
        player = GameObject.FindWithTag("Player");
        mascote = GameObject.FindWithTag("mascote"); 
        anim = GetComponent<Animator>();
        audioSrc = GetComponent<AudioSource>();
        fov = GetComponent<FieldOfView>();
        pal = GetComponent<PatrulharAleatorio>();
        scoreManager = GameObject.FindObjectOfType<ScoreManager>();
    }

    void Update()
    {
        if (vida <= 0)
        {
            Morrer();
            return;
        }

        if (fov.podeVerPlayer || fov.podeVerMascote)  
        {
            VaiAtrasAlvo();  
        }
        else
        {
            anim.SetBool("pararAtaque", true);
            CorrigirRigidSair();
            agente.isStopped = false;
            pal.Andar();
        }
    }

    private void VaiAtrasAlvo()
    {
        GameObject alvo = fov.podeVerPlayer ? player : mascote;  
        float distanciaDoAlvo = Vector3.Distance(transform.position, alvo.transform.position);

        if (distanciaDoAlvo < distanciaDoAtaque)
        {
            agente.isStopped = true;
            anim.SetTrigger("ataque");
            anim.SetBool("podeAndar", false);
            anim.SetBool("pararAtaque", false);
            CorrigirRigidEntrar();
        }

        if (distanciaDoAlvo >= distanciaDoAtaque + 1)
        {
            anim.SetBool("pararAtaque", true);
            CorrigirRigidSair();
        }

        if (anim.GetBool("podeAndar"))
        {
            agente.isStopped = false;
            agente.SetDestination(alvo.transform.position);
            anim.ResetTrigger("ataque");
        }
    }

    public void DarDano()
    {
        if (fov.podeVerPlayer)
        {
            player.GetComponent<MovimentarPersonagem>().AtualizarVida(-10);
        }
        else if (fov.podeVerMascote)
        {
            mascote.GetComponent<Mascote>().LevarDano(-10); 
        }
    }

    public void LevarDano(int dano)
    {
        vida -= dano;
        agente.isStopped = true;
        anim.SetTrigger("levouTiro");
        anim.SetBool("podeAndar", false);
        audioSrc.clip = somDano;
        audioSrc.Play();
    }

    private void Morrer()
    {
        audioSrc.clip = somMorte;
        audioSrc.Play();
        agente.isStopped = true;
        anim.SetBool("podeAndar", false);
        anim.SetBool("pararAtaque", true);
        anim.SetBool("morreu", true);
        scoreManager.AdicionarPontos(pointValue, 1);
        this.enabled = false;
        Destroy(gameObject, 5f);
    }

    public void Passo()
    {
        audioSrc.PlayOneShot(somPasso, 0.5f);
    }
    

    private void CorrigirRigidEntrar()
    {
        GetComponent<Rigidbody>().isKinematic = true;
    }

    private void CorrigirRigidSair()
    {
        GetComponent<Rigidbody>().isKinematic = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class InimigoComum : MonoBehaviour, ILevarDano
{
    private NavMeshAgent agente;
    private GameObject player;
    private Animator anim;
    private AudioSource audioSrc;
    public AudioClip somMorte,somDano,somPasso;
    public float distanciaDoAtaque = 2.0f;
    public int vida = 50;
    private PatrulharAleatorio pal;
    public int pointValue = 10;
    private FieldOfView fov;
    private ScoreManager scoreManager;

    
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
    }

    // Update is called once per frame
    void Update()
    {
        //VaiAtrasJogador();
        //OlharParaJogador();

        if(vida <=0){
            Morrer();
            return;
        }

        if (fov.podeVerPlayer)
        {
            VaiAtrasJogador();
        }else 
        {
            anim.SetBool("pararAtaque",true);
            CorrigirRigidSair();
            agente.isStopped= false;
            pal.Andar();
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
        scoreManager.AdicionarPontos(pointValue,1);
        this.enabled = false;
        Destroy(gameObject, 5f);
    }

    public void DarDano(){
        player.GetComponent<MovimentarPersonagem>().AtualizarVida(-10);
    }

    public void Passo()
    {
        audioSrc.PlayOneShot(somPasso,0.5f);
    }
}

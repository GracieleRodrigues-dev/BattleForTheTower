using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class FieldOfViewMascote : MonoBehaviour
{
    public float distanciaVisao;
    [Range(0, 360)]
    public float anguloVisao;
    public bool podeVerInimigo;
    public bool podeVerCaixaDeVida;
    public GameObject inimigoVisivel;
    public GameObject caixaDeVidaVisivel;
    
    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        // O campo de visão é processado no FixedUpdate para garantir precisão física
    }

    // Procura por inimigos e caixas de vida dentro do campo de visão
    private void ProcurarAlvosVisiveis()
    {
        podeVerInimigo = false;
        podeVerCaixaDeVida = false;
        inimigoVisivel = null;
        caixaDeVidaVisivel = null;

        // Verifica por inimigos dentro do campo de visão
        Collider[] alvosDentroRaio = Physics.OverlapSphere(transform.position, distanciaVisao);
        foreach (Collider alvo in alvosDentroRaio)
        {
            Vector3 dirToAlvo = (alvo.transform.position - transform.position).normalized;
            dirToAlvo.y = 0;  // Ignora a diferença na altura
            float distanciaAlvo = Vector3.Distance(transform.position, alvo.transform.position);

            // Verifica se é um inimigo e se está no campo de visão
            if (alvo.CompareTag("LevarDano") && Vector3.Angle(transform.forward, dirToAlvo) < anguloVisao / 2)
            {
                if (!Physics.Raycast(transform.position, dirToAlvo, distanciaAlvo))
                {
                    podeVerInimigo = true;
                    inimigoVisivel = alvo.gameObject;
                    return;  // Prioriza ataque a inimigos
                }
            }

            // Verifica se é uma caixa de vida e se está no campo de visão
            if (alvo.CompareTag("CaixaDeVida") && Vector3.Angle(transform.forward, dirToAlvo) < anguloVisao / 2)
            {
                if (!Physics.Raycast(transform.position, dirToAlvo, distanciaAlvo))
                {
                    podeVerCaixaDeVida = true;
                    caixaDeVidaVisivel = alvo.gameObject;
                }
            }
        }
    }

    void FixedUpdate()
    {
        ProcurarAlvosVisiveis();
    }


}

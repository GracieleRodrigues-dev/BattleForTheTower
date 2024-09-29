using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    public float distanciaVisao;
    [Range(0,360)]
    public float anguloVisao;
    public bool podeVerPlayer;
    private GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OlharParaJogador(){
        Vector3 direcaoOlhar = player.transform.position-transform.position;
        Quaternion rotacao = Quaternion.LookRotation(direcaoOlhar);
        transform.rotation = Quaternion.RotateTowards(transform.rotation,rotacao, Time.deltaTime*300);
    }

    private void ProcurarPlayerVisivel()
    {
        Collider[] alvosDentroRaio = Physics.OverlapSphere(transform.position,distanciaVisao);
        
        foreach(Collider alvo in alvosDentroRaio)
        {
            if(alvo.gameObject == player)
            {
                Vector3 dirToAlvo = (alvo.transform.position - transform.position).normalized;
                dirToAlvo.y = 0;
                if(Vector3.Angle(transform.forward,dirToAlvo) < anguloVisao/2)
                {
                    float disToAlvo = Vector3.Distance(transform.position, alvo.transform.position);
                    if(!Physics.Raycast(transform.position, dirToAlvo, disToAlvo))
                    {
                        podeVerPlayer = true;
                        OlharParaJogador();
                        return;
                    }
                }
            }
        }

        podeVerPlayer = false;
    }


    void FixedUpdate()
    {
        ProcurarPlayerVisivel();
    }

}

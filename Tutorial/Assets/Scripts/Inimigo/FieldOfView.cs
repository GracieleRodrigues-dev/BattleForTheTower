using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    public float distanciaVisao;
    [Range(0, 360)]
    public float anguloVisao;
    public bool podeVerPlayer;
    public bool podeVerMascote;  
    private GameObject player;
    private GameObject mascote;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        mascote = GameObject.FindWithTag("mascote"); 
    }

    void FixedUpdate()
    {
        ProcurarAlvoVisivel();
    }

    private void ProcurarAlvoVisivel()
    {
        podeVerPlayer = ProcurarVisivel(player);
        podeVerMascote = ProcurarVisivel(mascote);  
    }

    private bool ProcurarVisivel(GameObject alvo)
    {
        Collider[] alvosDentroRaio = Physics.OverlapSphere(transform.position, distanciaVisao);

        foreach (Collider col in alvosDentroRaio)
        {
            if (col.gameObject == alvo)
            {
                Vector3 dirToAlvo = (alvo.transform.position - transform.position).normalized;
                dirToAlvo.y = 0;
                if (Vector3.Angle(transform.forward, dirToAlvo) < anguloVisao / 2)
                {
                    float disToAlvo = Vector3.Distance(transform.position, alvo.transform.position);
                    if (!Physics.Raycast(transform.position, dirToAlvo, disToAlvo))
                    {
                        OlharParaAlvo(alvo);
                        return true;
                    }
                }
            }
        }
        return false;
    }

    private void OlharParaAlvo(GameObject alvo)
    {
        Vector3 direcaoOlhar = alvo.transform.position - transform.position;
        Quaternion rotacao = Quaternion.LookRotation(direcaoOlhar);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotacao, Time.deltaTime * 300);
    }
}

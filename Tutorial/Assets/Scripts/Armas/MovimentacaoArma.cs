using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public float valor = 0.1f;
    public float valorMaximo =0.6f;
    public float suavizaValor = 6;
    private Vector3 posicaoInicial;
    // Start is called before the first frame update
    void Start()
    {
       posicaoInicial = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        float movimentoX = Input.GetAxis("Mouse X") * valor;
        float movimentoY = Input.GetAxis("Mouse Y") * valor;   
        movimentoX = Mathf.Clamp(movimentoX,-valorMaximo,valorMaximo);
        movimentoY = Mathf.Clamp(movimentoY,-valorMaximo,valorMaximo);

        Vector3 finalPosition = new Vector3(movimentoX, movimentoY,0);
        transform.localPosition = Vector3.Lerp(transform.localPosition, finalPosition+posicaoInicial,Time.deltaTime * suavizaValor);
    }
}

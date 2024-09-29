using UnityEngine;

public class AcoesObjeto : MonoBehaviour
{
    private IdentificarObjeto idObjetos;
    private bool pegou = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        idObjetos = GetComponent<IdentificarObjeto>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F) && idObjetos.GetObjPegar()!=null){
            Pegar();
        }

        if (Input.GetKeyDown(KeyCode.F) && idObjetos.GetObjArrastar() != null){
            if(!pegou){
                Arrastar();
            } else {
                Soltar();
            }
            pegou = !pegou;
        }
    }

    private void Pegar(){
        IPegavel obj = idObjetos.GetObjPegar().GetComponent<IPegavel>();
        obj.Pegar();
        
        Destroy(idObjetos.GetObjPegar());
        idObjetos.EsconderTexto();
    }

    private void Arrastar(){
        GameObject obj = idObjetos.GetObjArrastar();
        obj.AddComponent<DragDrop>();
        obj.GetComponent<DragDrop>().Ativar();
        idObjetos.enabled = false;
    }


    private void Soltar(){
        GameObject obj = idObjetos.GetObjArrastar();
        Destroy(obj.GetComponent<DragDrop>());
        idObjetos.enabled = true;
    }
}

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MovimentarPersonagem : MonoBehaviour
{

    public CharacterController controle; 
    public float velocidade = 7f; 
    public float alturaPulo = 6f; 
    public float gravidade = -20f; 
    public AudioClip somPulo;
    public AudioClip somPassos;
    private AudioSource audioSrc;
    public Transform checaChao;
    public float raioEsfera = .4f; 
    public LayerMask chaoMask;
    public bool estaNoChao;
    Vector3 velocidadeCai; 
    private Transform cameraTransform;
    private bool estahAbaixado = false;
    private bool levantarBloqueado;
    public float alturaLevantado,alturaAbaixado,posicaoCameraEmPe,posicaoCameraAbaixado;
    private int vida = 100;
    public Slider sliderVida;

    public bool estahVivo = true;

    public void AtualizarVida(int novaVida){
        vida = Mathf.CeilToInt(Mathf.Clamp(vida + novaVida,0,100));
        sliderVida.value = vida;
    }

    private void AgacharLevantar(){
        estahAbaixado = !estahAbaixado;
        if(estahAbaixado){
            controle.height = alturaAbaixado;
            cameraTransform.localPosition = new Vector3(0,posicaoCameraAbaixado,0);
        } else {
            controle.height = alturaLevantado;
            cameraTransform.localPosition = new Vector3(0,posicaoCameraEmPe,0);
        }
    }      

    void Start(){
        controle = GetComponent<CharacterController>();
        cameraTransform = Camera.main.transform;
        audioSrc = GetComponent<AudioSource>();
    }

    void Update(){

        if(vida<=0){
            FimDeJogo();
            return;
        }    

        estaNoChao = Physics.CheckSphere(checaChao.position,raioEsfera,chaoMask);
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 mover = transform.right * x + transform.forward * z;
        controle.Move(mover * velocidade * Time.deltaTime);

        if (estaNoChao && mover.magnitude > 0f) {
            if (!audioSrc.isPlaying && audioSrc.clip != somPulo) { // Não tocar se som de pulo está tocando
                audioSrc.clip = somPassos;
                audioSrc.loop = true; // Som de passos deve ser contínuo
                audioSrc.Play();
            }
        } else {
            if (audioSrc.clip == somPassos) {
                audioSrc.Stop(); // Parar o som de passos se o personagem parar ou não estiver no chão
            }
        }

        ChecarBloqueioAbaixado();

        if(!levantarBloqueado && estaNoChao && Input.GetButton("Jump")){
            velocidadeCai.y = Mathf.Sqrt(alturaPulo *-2f*gravidade);
            audioSrc.clip = somPulo;
            audioSrc.loop = false; 
            audioSrc.Play();
        }

        if(!estaNoChao){
            velocidadeCai.y += gravidade *Time.deltaTime;
        }

        controle.Move(velocidadeCai * Time.deltaTime);

        if(Input.GetKeyDown(KeyCode.LeftControl)){
            AgacharLevantar();
        }
    }

    void OnDrawGizmosSelected(){
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(checaChao.position,raioEsfera);
    }

    private void ChecarBloqueioAbaixado(){
        Debug.DrawRay(cameraTransform.position,Vector3.up *1.1f,Color.red);
    }

    private void FimDeJogo(){
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        ScoreManager.Instance.setStatus("VOCÊ MORREU!");
        SceneManager.LoadScene(2);
    }


    public int getVida(){
        return vida;
    }

    public void ReabastecerVida()
    {
        vida = 100; 
    }
}

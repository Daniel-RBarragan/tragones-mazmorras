using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private static readonly int HorizontalMovement = Animator.StringToHash("horizontalMovement");
    private static readonly int VerticalMovement = Animator.StringToHash("verticalMovement");

    // Velocidad de movimiento
    [SerializeField] private float speed;
    
    // Referencia al Animator
    [SerializeField] private Animator animator;
    
    // RigidBody para aplicar movimiento
    private Rigidbody2D _rb;
    
    // Direccion a la que mira el personaje
    private int _facingDirection = 1;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Obtener valores de entrada de teclas WASD
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        
        // Checar si se esta presionando la tecla de una direccion y si el Sprite esta orientado en la direccion contraria
        if (horizontal > 0 && transform.localScale.x < 0 || horizontal < 0 && transform.localScale.x > 0)
        {
            // Imagen espejo del Sprite
            FlipSprite();
        }
        
        // Actualizar valores de los parametros del Animator
        animator.SetFloat(HorizontalMovement, Mathf.Abs(horizontal));
        animator.SetFloat(VerticalMovement, Mathf.Abs(vertical));
        
        // Aplicar velocidad para mover al personaje
        _rb.linearVelocity = new Vector2(horizontal, vertical) * speed;
    }

    private void FlipSprite()
    {
        _facingDirection *= -1;
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Classe pra logica do jogador
public class Player : MonoBehaviour
{
    private Rigidbody2D rb2d;
    private Animator anim;

    [SerializeField] private float moveSpeed; //Velocidade de movimento 

    private Vector2 movement; 

    //Variaveis utilizadas para alterar a animação 
    private bool move;             
    private bool facingRight;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        LookAtMouse();
        ProcessInput();
        UpdateAnimator();
    }

    //Atulizando a posição utilizando o vetor movemento
    private void FixedUpdate()
    {
        rb2d.MovePosition((Vector2)transform.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
    
    //Função chamada todo frame para atualizar o vetor movimento com o input do jogador
    private void ProcessInput()
    {
        move = false;
        
        movement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        movement.Normalize();

        if(movement.magnitude != 0.0f)
        {
            move = true;
        }

        //Logica para rotacionar o sprite
        if(movement.x < 0.0f && !facingRight || movement.x > 0.0f && facingRight)
        {
            Flip();
        }

    }
    
    //Função para rotacionar o sprite 
    private void Flip()
    {
        facingRight = !facingRight;

        transform.Rotate(0, 180, 0);
    }

    //Função chamada todo frame para forçar que o sprite "olhe" para o mouse
    private void LookAtMouse()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 pointDirection = mousePosition - (Vector2)transform.position;
        
        if(pointDirection.x < 0.0f && !facingRight || pointDirection.x > 0.0f && facingRight)
        {
            Flip();
        }
    }

    //Atualiza as variavéis do componente Animator
    private void UpdateAnimator()
    {
        anim.SetBool("move", move);
    }
}


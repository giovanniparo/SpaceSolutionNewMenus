using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Logica dos obstáculos
public class Barrel : MonoBehaviour
{
    private string moveDirection;

    public void setMoveDirection(string moveDirection)
    {
        this.moveDirection = moveDirection;
    }
    
    public string getMoveDirection()
    {
        return this.moveDirection;
    }

    //Checa se a bala colidiu com o obstáculo e danifica o jogador caso necessário
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Bullet"))
        {
            GameManager.instance.DamagePlayer();
            Destroy(this.gameObject);
        }
    }
}

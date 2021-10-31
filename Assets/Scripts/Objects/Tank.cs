using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Lógica dos objetivos (alvos = tanques)
public class Tank : MonoBehaviour
{
    private bool isHit;  //Necessário para checar se o tanque foi atingido no frame anterior

    private void Awake()
    {
        isHit = false;
    }

    public bool getIsHit()
    {
        return this.isHit;
    }

    public void setHit(bool isHit)
    {
        this.isHit = isHit;
    }

    //Atualiza o bollean isHit caso o tanque colida com um projétil bala
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Bullet"))
        {
            setHit(true);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Logica para instanciar os projeteis 
public class Gun : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject muzzlePrefab;
    [SerializeField] private Transform firePoint;

    [SerializeField] private float bulletSpeed;
    [SerializeField] private float fireRate;      //Posição da qual a bala será instanciada

    private float nextShot;                       //Variável interna para cooldown do tiro

    private Quaternion rotZ;
    private Quaternion rotX;

    private void Start()
    {
        nextShot = 0.0f;
    }

    void LateUpdate()
    {
        PointAtMouse();
        ProcessInput();
    }

    //Checa se o jogador apertou o botão esquerdo do mouse para atirar
    private void ProcessInput()
    {
        if(Input.GetMouseButton(0))
        {
            Fire();
        }
    }

    //Instancia a bala e a da velocidade 
    private void Fire()
    {
        if (Time.time >= nextShot)
        {
            nextShot = Time.time + (1 / fireRate);

            GameManager.instance.playSound("fire");

            GameObject bullet = Instantiate(bulletPrefab, firePoint.transform.position, rotZ);
            bullet.GetComponent<Rigidbody2D>().velocity = transform.right * bulletSpeed;

            GameObject muzzle = Instantiate(muzzlePrefab, firePoint.transform.position, Quaternion.identity);
        }
    }

    //Função para rotacionar o sprite da arma na direção da posição do cursor do mouse
    private void PointAtMouse()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector2 pointDirection = new Vector2(mousePosition.x - transform.position.x, mousePosition.y - transform.position.y);
        pointDirection.Normalize();

        float angle = Mathf.Atan2(pointDirection.y, pointDirection.x) * Mathf.Rad2Deg;

        if (angle > 90.0f || angle < -90.0f)
        {
            rotX = Quaternion.AngleAxis(180.0f, Vector3.right);
        }
        else
        {
            rotX = Quaternion.identity;
        }

        rotZ = Quaternion.AngleAxis(angle, Vector3.forward);

        transform.rotation = rotZ * rotX; //Logica de rotação
    }
}

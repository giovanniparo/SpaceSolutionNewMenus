using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Lógica do Projeil
public class Bullet : MonoBehaviour
{
    [SerializeField] private GameObject hitFXPrefab;
    
    private void Start()
    {
        Destroy(this.gameObject, 3.0f);
    }

    //Na colisão instanciar o FX de hit
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Instantiate(hitFXPrefab, transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }
}

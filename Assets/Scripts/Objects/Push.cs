using UnityEngine;

//Logica dos elementos da esteira para empurrar os obstáculos
public class Push : MonoBehaviour
{
    [SerializeField] private string direction;      //Direção do elemento
    [SerializeField] private float speed;           //Velocidade da esteira

    private Vector2 pushDirection;

    private void Start()
    {
        switch (direction)
        {
            case "up":
                pushDirection = transform.up;
                break;
            case "down":
                pushDirection = (-1) * transform.up;
                break;
            case "right":
                pushDirection = transform.right;
                break;
            case "left":
                pushDirection = (-1) * transform.right;
                break;
            default:
                Debug.LogError("Push direction is not defined!");
                break;
        }
    }

    //Empura o barril caso o centro da base do barril esteja colidindo com o elemento da esteira
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("BaseCenter"))
        {
            collision.gameObject.GetComponentInParent<Rigidbody2D>().velocity = pushDirection * speed * Time.deltaTime;
        }
    }
}

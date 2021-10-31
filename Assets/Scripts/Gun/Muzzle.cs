using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Brilho do tiro
public class Muzzle : MonoBehaviour
{
    private void Awake()
    {
        Destroy(this.gameObject, 0.03f);
    }
}

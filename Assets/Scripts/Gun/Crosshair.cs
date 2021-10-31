using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Classe para alterar o cursor para o sprite de "mira"
public class Crosshair : MonoBehaviour
{
    private void OnEnable()
    {
        Cursor.visible = false;
    }

    private void OnDisable()
    {
        Cursor.visible = true;
    }


    private void Update()
    {

        float zDistance = transform.position.z - Camera.main.transform.position.z;

        transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, zDistance));

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//Logica para atualizar o texto das "bolhas de fala"
public class SpeechBubble : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textMesh;

    public void UpdateText(string text)
    {
        textMesh.text = text;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Classe que contem as funções chamadas pelos butões da cena de tutorial
public class TutorialManager : MonoBehaviour
{
    public GameObject tutorial1;
    public GameObject tutorial2;

    public void NextButton()
    {
        tutorial1.SetActive(false);
        tutorial2.SetActive(true);
    }

    public void StartButton()
    {
        SceneManager.LoadScene("level1");
    }
}

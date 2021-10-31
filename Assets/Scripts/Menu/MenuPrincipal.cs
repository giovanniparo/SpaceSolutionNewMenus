using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Classe que contem as funções chamadas pelos butões do menu principal
public class MenuPrincipal : MonoBehaviour
{
    //Tornando o cursor do mouse visível
    private void Start()
    {
        Cursor.visible = true;
    }

    public void loadTutorial()
    {
        SceneManager.LoadScene("tutorial");
    }
    
    public void fase1(int fase)
    {
        SceneManager.LoadScene(fase);
    }

    public void introducao(int fase)
    {
        SceneManager.LoadScene(fase);
    }


    public void creditos(int fase)
    {
        SceneManager.LoadScene(fase);
    }

    public void voltar_menu(int fase)
    {
        SceneManager.LoadScene(fase);
    }

    public void QuitApp()
    {
        Application.Quit();
    }

} 

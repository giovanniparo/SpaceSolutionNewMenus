using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//Classe singleton responsável por controlar boa parte dos aspectos do jogo
//Contido nessa classe estão controladores para a logica de derrota e vitória assim como
//controladores de audio e UI 
public class GameManager : MonoBehaviour
{
    public static GameManager instance; //Instância do singleton
    
    //Elementos do jogo
    public GameObject player;
    public GameObject PC;
    public GameObject tank1;
    public GameObject tank2;
    public GameObject tank3;
    public GameObject exitDoor;
    public GameObject barrelPrefab;
    public GameObject crossHair;

    //Elementos de menu da UI
    public GameObject pauseMenu;
    public GameObject gameoverScreen;
    public GameObject winScreen;

    //Elmentos primários da UI
    public Slider healthBar;
    public Image healthBarFill;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI passwordsText;

    //Vetor com as posições para instanciar os obstáculos
    public Transform[] barrelSpawns;

    //Audio
    public AudioSource sfxAudioSource;
    public AudioClip fireClip;
    public AudioClip correctClip;
    public AudioClip incorrectClip;
    public AudioClip hurtClip;
    public AudioClip gameoverClip;
    public AudioClip winClip;

    //Componentes de "Bolha de Fala" no PC e nos tanques
    private SpeechBubble PCtext;
    private SpeechBubble tank1Text;
    private SpeechBubble tank2Text;
    private SpeechBubble tank3Text;

    //Parâmetros do jogo
    [SerializeField] private int maxHealth;   //Vida máxima
    [SerializeField] private float time;      //Tempo máximo
    [SerializeField] private int passwords;   //# de senhas necessárias

    //Cores da barra de vida da UI
    private Color maxHealthBarColor = Color.green;
    private Color minHealthBarColor = Color.red;

    //Variáveis internas
    private int health;

    private int num1;
    private int num2;
    private int operation;
    private int result;

    private int randomTank;

    private int difficulty;
    private int score;
    private int currentTimeMin;
    private int currentTimeSeg;

    private bool gamePaused;     //Jogo pausado
    private bool genOp;          //Necessário gerar outra operação

    //Criando instância de gameManager
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogError("More than one instance of GameManager present");
            Destroy(this);
        }
    }

    private void Start()
    {   
        PCtext = PC.GetComponentInChildren<SpeechBubble>();
        tank1Text = tank1.GetComponentInChildren<SpeechBubble>();
        tank2Text = tank2.GetComponentInChildren<SpeechBubble>();
        tank3Text = tank3.GetComponentInChildren<SpeechBubble>();

        health = maxHealth;
        difficulty = 1;
        score = 0;

        randomTank = 0;

        healthBar.wholeNumbers = true;
        healthBar.minValue = 0f;
        healthBar.maxValue = maxHealth;
        healthBar.value = maxHealth;

        gamePaused = false;
        Time.timeScale = 1.0f;
        genOp = true;
    }

    private void Update()
    {
        ProcessInput();
        CheckIfTanksHit();

        if(genOp) //Caso um tanque seja atingido é necessário gerar outra operação
        {
            GenerateRandomOperation();
            UpdatePCandTanks();
            ResetTanks();
            genOp = false;
        }

        UpdateClock();
        UpdateUI();
    }

    //Checa se os tanques foram atingidos
    //Atualiza os parametros e chama as funções para tocar o clip de audio correspondente
    private void CheckIfTanksHit()
    {
        if(tank1.GetComponent<Tank>().getIsHit() ||
            tank2.GetComponent<Tank>().getIsHit() ||
            tank3.GetComponent<Tank>().getIsHit())
        {
            genOp = true;
        }

        if ((tank1.GetComponent<Tank>().getIsHit() && randomTank == 1) ||
            (tank2.GetComponent<Tank>().getIsHit() && randomTank == 2) ||
            (tank3.GetComponent<Tank>().getIsHit() && randomTank == 3))
        {
            playSound("correct");
            score += Mathf.Abs(result);
            difficulty++;
            passwords--;
            if(passwords <= 0)
            {
                GameWin();
            }
            else
            {
                SpawnBarrel();
            }
        }
        
        if ((tank1.GetComponent<Tank>().getIsHit() && (randomTank == 2 || randomTank == 3)) ||
            (tank2.GetComponent<Tank>().getIsHit() && (randomTank == 1 || randomTank == 3)) ||
            (tank3.GetComponent<Tank>().getIsHit() && (randomTank == 1 || randomTank == 2)))
        {
            playSound("incorrect");
        }
    }

    //Logica para instanciar obstáculos em posições aleatórias no vetor barrelSpawns
    private void SpawnBarrel()
    {
        int randomPos = Random.Range(0, 10);
        Instantiate(barrelPrefab, barrelSpawns[randomPos].transform.position, Quaternion.identity);
    }

    //Reinicia os parametros nos scripts dos tanques
    private void ResetTanks()
    {
        tank1.GetComponent<Tank>().setHit(false);
        tank2.GetComponent<Tank>().setHit(false);
        tank3.GetComponent<Tank>().setHit(false);
    }

    //Checa se o jogador apertou o botão de pause (TODO FRAME)
    private void ProcessInput()
    {
        if(Input.GetKeyDown(KeyCode.Escape) && !gamePaused)
        {
            PauseGame();
        }
    }
    
    //Logica para geração de uma operação matemática baseada na dificuldade atual
    //Chamada caso um tanque seja atingido no frame anterior
    private void GenerateRandomOperation()
    {
        if(difficulty <= 4)
        {
            num1 = Random.Range(1, 10);
            num2 = Random.Range(1, 10);
            operation = Random.Range(1, 3);
        }
        else if(difficulty <= 6)
        {
            num1 = Random.Range(10, 50);
            num2 = Random.Range(10, 50);
            operation = Random.Range(1, 3);
        }
        else if(difficulty < 10)
        {
            num1 = Random.Range(1, 10);
            num2 = Random.Range(10, 15);
            operation = 3;
        }
        else
        {
            num1 = Random.Range(10, 15);
            num2 = Random.Range(10, 15);
            operation = 3;
        }

        switch(operation)
        {
            case 1:
                result = num1 + num2;
                break;
            case 2:
                result = num1 - num2;
                break;
            case 3:
                result = num1 * num2;
                break;
            default:
                Debug.LogError("The operation is not defined");
                break;
        }
    }

    //Função para atualizar os textos das bolhas de fala dos tanques 
    //Também gera dois resultados "falsos"
    private void UpdatePCandTanks()
    {
        int fakeResult1;
        int fakeResult2;

        randomTank = Random.Range(1, 4);

        do
        {
            fakeResult1 = Random.Range(result - 10, result + 10);
            fakeResult2 = Random.Range(result - 10, result + 10);
        } while (fakeResult1 == result || fakeResult2 == result || fakeResult1 == fakeResult2);

        switch (operation)
        {
            case 1:
                PCtext.UpdateText(num1 + "+" + num2);
                break;
            case 2:
                PCtext.UpdateText(num1 + "-" + num2);
                break;
            case 3:
                PCtext.UpdateText(num1 + "*" + num2);
                break;
            default:
                Debug.LogError("The operation is not defined");
                break;
        }

        switch (randomTank)
        {
            case 1:
                tank1Text.UpdateText(result.ToString());
                tank2Text.UpdateText(fakeResult1.ToString());
                tank3Text.UpdateText(fakeResult2.ToString());
                break;
            case 2:
                tank2Text.UpdateText(result.ToString());
                tank1Text.UpdateText(fakeResult1.ToString());
                tank3Text.UpdateText(fakeResult2.ToString());
                break;
            case 3:
                tank3Text.UpdateText(result.ToString());
                tank1Text.UpdateText(fakeResult1.ToString());
                tank2Text.UpdateText(fakeResult2.ToString());
                break;
            default:
                Debug.LogError("Tank not defined");
                break;
        }
    }

    //Função para tocar o clipe de audio necessário
    public void playSound(string action)
    {
        switch(action)
        {
            case "fire":
                sfxAudioSource.clip = fireClip;
                sfxAudioSource.Play();
                break;
            case "correct":
                sfxAudioSource.clip = correctClip;
                sfxAudioSource.Play();
                break;
            case "incorrect":
                sfxAudioSource.clip = incorrectClip;
                sfxAudioSource.Play();
                break;
            case "damage":
                sfxAudioSource.clip = hurtClip;
                sfxAudioSource.Play();
                break;
            case "gameover":
                sfxAudioSource.clip = gameoverClip;
                sfxAudioSource.Play();
                break;
            case "win":
                sfxAudioSource.clip = winClip;
                sfxAudioSource.Play();
                break;
            default:
                Debug.LogError("Audio Clip not defined!");
                break;
        }
    }

    //Função para pausar o jogo
    private void PauseGame()
    {
        //Variavel interna que controla se o jogo está pausado ou não
        gamePaused = true;

        //Pausa o tempo
        Time.timeScale = 0;

        //Desabilita os gameObjects necessários
        crossHair.SetActive(false);
        player.SetActive(false);
        PCtext.gameObject.SetActive(false);
        tank1Text.gameObject.SetActive(false);
        tank2Text.gameObject.SetActive(false);
        tank3Text.gameObject.SetActive(false);

        //Torna o elemento da UI visível
        pauseMenu.SetActive(true);
    }

    //Despausa o jogo
    public void ResumeGame()
    {
        gamePaused = false;
        
        Time.timeScale = 1;

        crossHair.SetActive(true);
        player.SetActive(true);
        PCtext.gameObject.SetActive(true);
        tank1Text.gameObject.SetActive(true);
        tank2Text.gameObject.SetActive(true);
        tank3Text.gameObject.SetActive(true);

        pauseMenu.SetActive(false);
    }

    //Logica do botão para retornar ao menu principal
    public void ReturnMenu()
    {
        SceneManager.LoadScene("MenuPrincipal");
    }

    //Fecha o jogo
    public void QuitGame()
    {
        Application.Quit();
    }

    //Função para atulizar o relógio interno da cena
    //Transforma o tempo em segundos em (00:00)
    private void UpdateClock()
    {
        float currentTime = time - Time.timeSinceLevelLoad;
        currentTimeMin = (int)(currentTime / 60);
        currentTimeSeg = (int)(currentTime - currentTimeMin * 60);

        //Chama GameOver caso o tempo se esgote
        if(currentTime <= 0.0f)
        {
            GameOver();
        }
    }

    //Função para atualizar os elmentos da interface de usuário 
    //Vida, Senhas, Relogio e Pontos
    public void UpdateUI()
    {
        healthBar.value = health;
        healthBarFill.color = Color.Lerp(minHealthBarColor, maxHealthBarColor, (float)health / maxHealth);

        scoreText.text = "Pontos: " + score.ToString();

        passwordsText.text = "Senhas: " + passwords.ToString();

        timeText.text = currentTimeMin.ToString() + ":" + currentTimeSeg.ToString();
    }

    //Função para diminuir o numero de vidas do jogador e checar se o mesmo morreu
    //Chama GameOver caso necessário
    public void DamagePlayer()
    {
        health -= 1;

        Debug.Log("Player took 1 damage");
        playSound("damage");

        if(health <= 0)
        {
            GameOver();
        }
    }

    //Chamado caso o jogador vença
    private void GameWin()
    {
        if(!gamePaused) //Necessário para que a lógica só seja executada uma única vez (corrige alguns bugs)
        {
            gamePaused = true;

            crossHair.SetActive(false);
            player.SetActive(false);
            PCtext.gameObject.SetActive(false);
            tank1Text.gameObject.SetActive(false);
            tank2Text.gameObject.SetActive(false);
            tank3Text.gameObject.SetActive(false);

            playSound("win");

            StartCoroutine(DoorOpeningCoroutine());
        }
    }

    //Corotina para esperar a animação de abrir a porta terminar antes de encerrar o jogo
    //Chamado em caso de vitória
    IEnumerator DoorOpeningCoroutine()
    {
        exitDoor.GetComponent<Animator>().SetTrigger("open");
        yield return new WaitForSeconds(2.0f);
        Time.timeScale = 0;
        winScreen.SetActive(true);
    }

    //Chamado caso o tempo ou o numero de vidas seja nulo
    private void GameOver()
    {
        if(!gamePaused) //Necessário para que a lógica só seja executada uma única vez (corrige alguns bugs)
        {
            gamePaused = true;

            Time.timeScale = 0;

            crossHair.SetActive(false);
            player.SetActive(false);
            PCtext.gameObject.SetActive(false);
            tank1Text.gameObject.SetActive(false);
            tank2Text.gameObject.SetActive(false);
            tank3Text.gameObject.SetActive(false);

            playSound("gameover");

            gameoverScreen.SetActive(true);
        }
    }
}

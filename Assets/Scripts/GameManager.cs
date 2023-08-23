using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

//목적1: 게임의 상태(Ready, Start, GameOver)를 구별하고, 게임의 시작과 끝을 TextUI로 표현한다.
//필요속성1: 게임상태 열거형 변수, TextUI


//목적2: 2초 후 게임이 Ready상태에서 Start상태로 변경되며 시작된다.

//목적3: Ready상태일 때는 플레이어,적이 움직일 수 없도록 한다.

//목적4: 플레이어의 Hp가 0보다 작으면 상태 text를 게임 오버로 바꾸어 주고 상태텍스트와 상태도 또한 GameOver로 바꾸어 준다.
//필요속성4: hp가 들어있는 플레이어 무브
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    //필요속성1: 게임상태 열거형 변수, TextUI
    public enum GameState
    {
        Ready,
        Start,
        GameOver
    }
    public GameState state = GameState.Ready;
    public TMP_Text stateText;


    //필요속성4: hp가 들어있는 플레이어 무브
    PlayerMove player;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        stateText.text = "Ready";
        stateText.color = new Color32(255, 185, 0, 255);

        StartCoroutine(GameStart());

        player = GameObject.Find("Player").GetComponent<PlayerMove>();
    }


    //목적2: 2초 후 게임이 Ready상태에서 Start상태로 변경되며 시작된다.
    IEnumerator GameStart()
    {
        yield return new WaitForSeconds(2);

        stateText.text = "Game Start";
        stateText.color = new Color32(0, 255, 0, 255);

        yield return new WaitForSeconds(0.5f);

        stateText.gameObject.SetActive(false);

        state = GameState.Start;
    }


    //목적4: 플레이어의 Hp가 0보다 작으면 상태 text를 게임 오버로 바꾸어 주고 상태텍스트와 상태도 또한 GameOver로 바꾸어 준다.
    void CheckGameOver()
    {
        if (player.hp < 0)
        {
            stateText.gameObject.SetActive(true);

            stateText.text = "Game Over";
            stateText.color = new Color32(255, 0, 0, 255);

            state = GameState.GameOver;
        }
    }

    // Update is called once per frame
    void Update()
    {
        CheckGameOver();
    }
}

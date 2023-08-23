using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

//����1: ������ ����(Ready, Start, GameOver)�� �����ϰ�, ������ ���۰� ���� TextUI�� ǥ���Ѵ�.
//�ʿ�Ӽ�1: ���ӻ��� ������ ����, TextUI


//����2: 2�� �� ������ Ready���¿��� Start���·� ����Ǹ� ���۵ȴ�.

//����3: Ready������ ���� �÷��̾�,���� ������ �� ������ �Ѵ�.

//����4: �÷��̾��� Hp�� 0���� ������ ���� text�� ���� ������ �ٲپ� �ְ� �����ؽ�Ʈ�� ���µ� ���� GameOver�� �ٲپ� �ش�.
//�ʿ�Ӽ�4: hp�� ����ִ� �÷��̾� ����
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    //�ʿ�Ӽ�1: ���ӻ��� ������ ����, TextUI
    public enum GameState
    {
        Ready,
        Start,
        GameOver
    }
    public GameState state = GameState.Ready;
    public TMP_Text stateText;


    //�ʿ�Ӽ�4: hp�� ����ִ� �÷��̾� ����
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


    //����2: 2�� �� ������ Ready���¿��� Start���·� ����Ǹ� ���۵ȴ�.
    IEnumerator GameStart()
    {
        yield return new WaitForSeconds(2);

        stateText.text = "Game Start";
        stateText.color = new Color32(0, 255, 0, 255);

        yield return new WaitForSeconds(0.5f);

        stateText.gameObject.SetActive(false);

        state = GameState.Start;
    }


    //����4: �÷��̾��� Hp�� 0���� ������ ���� text�� ���� ������ �ٲپ� �ְ� �����ؽ�Ʈ�� ���µ� ���� GameOver�� �ٲپ� �ش�.
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

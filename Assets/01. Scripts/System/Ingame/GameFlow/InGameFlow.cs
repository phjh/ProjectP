using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 이건 접속할때 만들어주고 서버랑 InGame상태 연동해줄예정 + 코드 옮길 예정
public enum GameState
{
    None = 0,
    Login = 1, //로그인 및 플레이어 정보 받아오는 상태
    MainMenu = 2, //메인메뉴화면
    InGame = 3,   //게임 안 상태
    //딱히 생각나는게 없다~...
}

//이거는 인게임에서의 현재 라운드 상태 
public enum RoundState
{
    None = 0,
    Ready = 1, //이떄 증강 고르기
    InRound = 2, //투닥투닥 싸울떄
    EndRound = 3, //싸우는게 끝났을때
}

public class InGameFlow : MonoBehaviour
{

	#region Card variables

	[SerializeField]
    private RectTransform CardSelectUI;

    [SerializeField]
    [Tooltip("이건 CardInfo SO")]
    private List<CardsInfo> cardInfos = new List<CardsInfo>();
    [SerializeField]
    [Tooltip("이건 UI쪽 카드")]
    private List<Cards> cards = new List<Cards>();
	private CardsInfo GetRandomCard => cardInfos[Random.Range(0, cardInfos.Count)];

	#endregion

	private void Start()
	{
        GameManager.Instance.game = this;

        InitCards();
	}

	private void OnDestroy()
	{
		GameManager.Instance.game = null;
	}

	private void Update()
	{
        if (Input.GetKeyDown(KeyCode.T))
        {
            CardSelect();
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            EndCardSelect();
        }
	}

	#region Card Methods

	public void CardSelect()
    {
        ResetCards();
        CardSelectUI.gameObject.SetActive(true);
        GameManager.Instance.player._input.Disable();
    }

    public void EndCardSelect()
    {
        CardSelectUI.gameObject.SetActive(false);
        GameManager.Instance.player._input.Enable();
    }

	private void InitCards()
    {
        Player player = GameManager.Instance.player;
        UserStatus stat = player.status;
        IAttackable attack = player.attackComponent;
        IShieldable shield = player.shieldComponent;
        foreach(var card in cards)
        {
            card.Init(stat, attack, shield);
        }
    }

    public void ResetCards()
    {
        foreach(var card in cards)
        {
            card.gameObject.SetActive(true);
            card.SetCard(GetRandomCard);
        }
    }

    public void Countdown(bool value)
    {

    }

    private IEnumerator SetSelectCountDown()
    {
        yield return new WaitForSeconds(1f);
    }

	#endregion

}

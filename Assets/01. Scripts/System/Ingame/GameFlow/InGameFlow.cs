using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

    #region Loading variables

    [SerializeField]
    private Slider _loadingBar;
    [SerializeField]
    private TextMeshProUGUI _loadingText;
    [SerializeField]
    private RectTransform _loadingUI;
    [SerializeField]
    private TextMeshProUGUI _loadingUserText;
    [SerializeField]
    private TextMeshProUGUI _loadingInfoText;

	#endregion

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

	public void InitCards()
    {
        Debug.Log("init card");

        Player player = GameManager.Instance.player;
        UserStatus stat = new UserStatus();
        stat = player.status;
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

	#region Loading Methods

    public void SetLoadingProcess(int num, int limitnum)
    {
        _loadingBar.value = Mathf.Clamp((float)num/limitnum, 0.2f, 0.9f);
        _loadingUserText.text = $"{num} / {limitnum} 로딩완료\n다른 유저를 기다리는중...";
    }

    public void GameStarting() => StartCoroutine(GameStartingCoroutine());

	IEnumerator GameStartingCoroutine()
    {
        yield return new WaitForSeconds(0.5f);
        _loadingBar.value = 1;
        InitCards();
        yield return new WaitForSeconds(0.5f);
        _loadingUI.gameObject.SetActive(false);
    }

	#endregion

}

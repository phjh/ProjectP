using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// �̰� �����Ҷ� ������ְ� ������ InGame���� �������ٿ��� + �ڵ� �ű� ����
public enum GameState
{
    None = 0,
    Login = 1, //�α��� �� �÷��̾� ���� �޾ƿ��� ����
    MainMenu = 2, //���θ޴�ȭ��
    InGame = 3,   //���� �� ����
    //���� �������°� ����~...
}

//�̰Ŵ� �ΰ��ӿ����� ���� ���� ���� 
public enum RoundState
{
    None = 0,
    Ready = 1, //�̋� ���� ����
    InRound = 2, //�������� �ο
    EndRound = 3, //�ο�°� ��������
}

public class InGameFlow : MonoBehaviour
{

	#region Card variables

	[SerializeField]
    private RectTransform CardSelectUI;

    [SerializeField]
    [Tooltip("�̰� CardInfo SO")]
    private List<CardsInfo> cardInfos = new List<CardsInfo>();
    [SerializeField]
    [Tooltip("�̰� UI�� ī��")]
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
        _loadingUserText.text = $"{num} / {limitnum} �ε��Ϸ�\n�ٸ� ������ ��ٸ�����...";
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

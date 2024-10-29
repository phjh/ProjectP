using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

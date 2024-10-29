using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New ChangeStatus", menuName = "SO/Cards")]
public class CardsInfo : ScriptableObject
{
	[SerializeField]
	private int CardID = -1;

	public Sprite cardImage;
	public string cardName;
	public string cardDescription;
	public string cardStats;

	[SerializeField]
	private AbilityCards _ability;
	[SerializeField]
	private EditStatusCard _editStat;

	public void Init(UserStatus stat, IAttackable attackAble, IShieldable shieldable)
	{
		if(CardID == -1)
		{
			Debug.LogWarning("CardID is not defined");
			return;
		}
		if(CardID == 0)
		{
			Debug.LogWarning("test card is in pools");
		}

		_ability?.Init(attackAble, shieldable);
		_editStat?.Init(stat);
	}


}

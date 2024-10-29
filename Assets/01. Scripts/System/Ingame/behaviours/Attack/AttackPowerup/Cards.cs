using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Cards : MonoBehaviour
{
	private CardsInfo card;

	private UserStatus _stat;
	private IAttackable _attack;
	private IShieldable _shield;

	[SerializeField]
	private Image cardImage;
	[SerializeField]
	private TextMeshProUGUI cardName;
	[SerializeField]
	private TextMeshProUGUI cardDescription;
	[SerializeField]
	private TextMeshProUGUI cardStats;

	public void Init(UserStatus stat, IAttackable attackAble, IShieldable shieldable)
	{
		_stat = stat;
		_attack = attackAble;
		_shield = shieldable;
	}

	public void SetCard(CardsInfo info)
	{
		card = info;
		cardImage.sprite = info.cardImage;
		cardName.text = SetCardTexts(info.cardName);
		cardDescription.text = SetCardTexts(info.cardDescription);
		cardStats.text = SetCardTexts(info.cardStats);
	}

	private string SetCardTexts(string str)
	{
		StringBuilder sb = new();
		for (int i = 0; i < str.Length; i++)
		{
			if (str[i] == '/')
			{
				sb.Append("\n");
			}
			else
			{
				sb.Append(str[i]);
			}
		}
		return sb.ToString();
	}

	public void OnSelected()
	{
		card.Init(_stat, _attack, _shield);
		this.gameObject.SetActive(false);
	}
}
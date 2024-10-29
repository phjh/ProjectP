using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerUIs : MonoBehaviour
{
	[SerializeField]
	private TextMeshProUGUI userName;
	[SerializeField]
	private TextMeshProUGUI userScore;

	public void SetUI(string name, int score)
	{
		userName.text = name;
		userScore.text = score.ToString();
	}

}

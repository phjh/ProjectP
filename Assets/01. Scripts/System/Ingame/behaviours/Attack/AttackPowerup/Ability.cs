using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability
{
	public Action abilityAction;
	public int selectedCounts { get; private set; } = 0;

	public void Selected() => selectedCounts++;

	public void Selected(Action act)
	{
		abilityAction += act;
		Selected();
	}

	public void Clear()
	{
		abilityAction = null;
		selectedCounts = 0;
	}

	public void Invoke()
	{
		for (int i = 0; i < selectedCounts; i++)
		{
			abilityAction.Invoke();
		}
	}

}

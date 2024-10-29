using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChattingManager : MonoSingleton<ChattingManager>
{
	[SerializeField]
	private RectTransform ChatScroll;

	[SerializeField]
	private List<GameObject> chatList;

	private void Start()
	{
		DontDestroyOnLoad(this.gameObject);
	}

	public void SetChatting(string s)
	{
		PoolableMono mono = PoolManager.Instance.Pop(PoolUIListEnum.ChatText);
		mono.transform.parent = ChatScroll.transform;
		mono.GetComponent<TextMeshProUGUI>().text = s;
		chatList.Add(mono.gameObject);
	}


}

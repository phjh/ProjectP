using UnityEngine;
using TMPro;
using Google.Protobuf.Protocol;
using System.Data.SqlClient;
using System;
using Unity.Burst.Intrinsics;

namespace ServerCore
{
	public class DBManager : MonoSingleton<DBManager>
	{
		private string userId;
		private string userPassword;
		public string userName;
		public int userScore;

		public RectTransform errorUI;
		[SerializeField]
		private TextMeshProUGUI errorText;
		[SerializeField]
		private TextMeshProUGUI errorDescription;

		public Action Success;

		void Start()
		{
			DontDestroyOnLoad(this.gameObject);
		}

		public void Login(string id, string password)
		{
			C_Login login = new C_Login();
			login.Id = id;
			login.Pw = password;
			userId = id;
			userPassword = password;
			NetworkManager.Instance.Send(login);
		}

		public void CheckLogin()
		{
			if (userId == null)
			{
				Debug.Log("no info");
				return;
			}

			C_Login login = new C_Login();
			login.Id = userId;
			login.Pw = userPassword;
			NetworkManager.Instance.Send(login);
		}

		public void Logout()
		{
			userId = null;
			userPassword = null;
			userName = null;
			userScore = 0;
		}

		public void Register(string id, string password, string name)
		{
			C_Register register = new C_Register();
			register.Id = id;
			register.Pw = password;
			register.Name = name;
			userId = id;
			userPassword = password;
			NetworkManager.Instance.Send(register);
		}

		public void DBError(string type, int code)
		{
			Logout();
			errorUI.gameObject.SetActive(true);
			Console.WriteLine($"error - type :{type}, code: {code}, description: {ErrorCodeToDescriptor(code)}");
			errorText.text = type;
			errorDescription.text = ErrorCodeToDescriptor(code);
		}

		public void DBContinue()
		{
			errorUI.gameObject.SetActive(false);
		}

		private string ErrorCodeToDescriptor(int code)
		{
			switch(code)
			{
				case -100001:
					return "There are not existing ID";
				case -100002:
					return "Wrong password";
				case -200001:
					return "Same ID exist";
				case -200002:
					return "Same name exist";
			}
			return code.ToString() + "     bug¿ŒµÌ?";
		}

	}
}
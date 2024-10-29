using TMPro;
using ServerCore;
using UnityEngine;

public class LoginScene : MonoBehaviour
{
	[SerializeField]
	private RectTransform mainUI;
	[SerializeField]
	private TextMeshProUGUI username;

	[SerializeField]
	private RectTransform loginUI;
	[SerializeField]
	private TextMeshProUGUI loginID;
	[SerializeField]
	private TextMeshProUGUI loginPassword;


	[SerializeField]
	private RectTransform registerUI;
	[SerializeField]
	private TextMeshProUGUI registerID;
	[SerializeField]
	private TextMeshProUGUI registerPassword;
	[SerializeField]
	private TextMeshProUGUI registerName;

	private void Start()
	{
		DBManager.Instance.Success += SetUI;
	}

	private void OnDestroy()
	{
		if(DBManager.Instance != null) 
			DBManager.Instance.Success -= SetUI;
	}

	public void SetUI()
	{
		mainUI.gameObject.SetActive(true);
		loginUI.gameObject.SetActive(false);
		registerUI.gameObject.SetActive(false);
		username.text = DBManager.Instance.userName;
	}

	public void Login()
	{
		DBManager.Instance.Login(loginID.text, loginPassword.text);
		
	}

	public void SetLoginUI(bool value)
	{
		loginUI.gameObject.SetActive(value);
		mainUI.gameObject.SetActive(!value);
	}

	public void Register()
	{
		DBManager.Instance.Register(registerID.text, registerPassword.text, registerName.text);
	}

	public void SetRegisterUI(bool value)
	{
		registerUI.gameObject.SetActive(value);
		mainUI.gameObject.SetActive(!value);
	}

	public void LogOut()
	{
		DBManager.Instance.Logout();
	}

	public void LobbyScene()
	{
		SceneManager.Instance.LoadNextScene();
	}

}

public class SceneManager : MonoSingleton<SceneManager>
{
	private void Awake()
	{
		DontDestroyOnLoad(this.gameObject);
	}

	public enum SceneList
	{
		None = 0,
		MainScreen = 1,
		Login = 2,
		StandRoom = 3,
		InGame = 4,
	}

	public SceneList nowScene = SceneList.None;

	public void LoadNextScene()
	{
		nowScene = (SceneList)(((int)nowScene + 1) % (int)SceneList.InGame);
		UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(nowScene.ToString() + "Scene");
	}


}

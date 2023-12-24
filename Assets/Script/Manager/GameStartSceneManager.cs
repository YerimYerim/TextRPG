using Script.Manager;
using UnityEditor.Compilation;

public class GameStartSceneManager : Singleton<GameStartSceneManager>
{
    enum LoadSceneState
    {
        
    }
    private void Start()
    {
        GameDataManager.Instance.LoadData();

        if (GameUIManager.Instance.TryGetOrCreate<UIGameStart>(false, UILayer.LEVEL_1, out var ui))
        {
            ui.Show();
        }
    }
}

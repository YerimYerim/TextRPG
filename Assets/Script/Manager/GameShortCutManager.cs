using Script.Manager;

public class GameShortCutManager : Singleton<GameShortCutManager>
{

    public bool GoToContent(CONTENT_TYPE contentType, params object[] param)
    {
        switch (contentType)
        {
            case CONTENT_TYPE.CONTENT_TYPE_ITEM:
            {
                if (GameUIManager.Instance.TryGetOrCreate<UIPopUpInventory>(false, UILayer.LEVEL_3, out var ui))
                {
                    ui.Show();
                }
                return true;
            } break;
            case CONTENT_TYPE.CONTENT_TYPE_STATUS:
            {
                if (GameUIManager.Instance.TryGetOrCreate<UIPopUpStatus>(false, UILayer.LEVEL_4, out var ui))
                {
                    ui.Show();
                }
                return true;
            } break;
            case CONTENT_TYPE.CONTENT_TYPE_ACHIEVEMENT:
            {
                if (GameUIManager.Instance.TryGetOrCreate<UIPopUpAchievement>(false, UILayer.LEVEL_3, out var ui))
                {
                    ui.Show();
                }
                return true;
            } break;
            case CONTENT_TYPE.CONTENT_TYPE_COLLECTION:
            {
                if (GameUIManager.Instance.TryGetOrCreate<UIPopUpCollection>(false, UILayer.LEVEL_3, out var ui))
                {
                    ui.Show();
                }
                return true;
            } break;
            default:
            {
                return false;
            }
        }
    }
}

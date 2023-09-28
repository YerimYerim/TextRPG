using System.IO;
using Script.DataClass;
using UnityEngine;

namespace Script.Manager
{
    public class GameResourceManager : Singleton<GameResourceManager>
    {
        public Sprite GetImage(string imageName)
        {
            var image = Resources.Load<Sprite>($"Sprites/{imageName}");
            return image;
        }
    }
}

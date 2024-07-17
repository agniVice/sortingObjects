using System;
using LeTai.Asset.TranslucentImage;
using UnityEngine;

public class TranslucentSourcesManager : MonoBehaviour
{
        public static TranslucentSourcesManager instance;

        [SerializeField] private TranslucentImageSource _bg;
        [SerializeField] private TranslucentImageSource _game;

        private void Awake()
        {
                if (instance == null)
                {
                        instance = this;
                }
                // else
                // {
                //         Destroy(gameObject);
                //         return;
                // }
                //
                // DontDestroyOnLoad(this);
        }

        public TranslucentImageSource GetSource(TranslucentSourceType type)
        {
                switch (type)
                {
                        case TranslucentSourceType.BG:
                                return _bg;
                                break;
                        case TranslucentSourceType.Game:
                                return _game;
                                break;
                        default:
                                break;
                }

                return null;
        }
}

public enum TranslucentSourceType
{
        BG,
        Game
}

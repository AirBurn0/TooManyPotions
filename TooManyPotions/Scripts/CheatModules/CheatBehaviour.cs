using UnityEngine;

namespace TooManyPotions.Scripts.CheatModules
{
    public class CheatBehaviour<T>: MonoBehaviour where T : MonoBehaviour
    {
        public static MonoBehaviour? Instance
        {
            get
            {
                if (_instance == null && ModInfo.CheatHolder != null)
                    _instance = ModInfo.CheatHolder.GetComponent<T>() ?? ModInfo.CheatHolder.AddComponent<T>();
                return _instance;
            }
        }
        private static MonoBehaviour? _instance;
        
	}

}

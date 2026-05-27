using UnityEngine;

namespace MaiNull.Singleton
{
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;

        public static T Instance
        {
            get 
            {
                if (_instance != null) return _instance;
                _instance = FindFirstObjectByType<T>();
                
                if (_instance != null) return _instance;
                GameObject singletonObj = new GameObject
                {
                    name = typeof(T).ToString()
                };
                _instance = singletonObj.AddComponent<T>();
                return _instance;
            } 
        }

        public virtual void Awake()
        {
            if (_instance != null)
            {
                Destroy(gameObject);
                return;
            }

            _instance = GetComponent<T>();

            DontDestroyOnLoad(gameObject);

            if (_instance != null)
                return;
        }
    }
}

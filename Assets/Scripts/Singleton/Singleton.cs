using UnityEngine;

namespace MaiNull.Singleton
{
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T instance;

        public static T Instance
        {
            get 
            { 
                if (instance == null) 
                {
                    instance = FindFirstObjectByType<T>();
                    if (instance == null)
                    {
                        var singletonObj = new GameObject();
                        singletonObj.name = typeof(T).ToString();
                        instance = singletonObj.AddComponent<T>();
                    }
                }
                return instance;
            } 
        }


        public virtual void Awake()
        {
            if (instance != null)
            {
                Destroy(gameObject);
                return;
            }

            instance = GetComponent<T>();

            DontDestroyOnLoad(gameObject);

            if (instance != null)
                return;
        }
    }
}

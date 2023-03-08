using UnityEngine;

namespace _Game.Code.Utils
{
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static readonly bool m_ShuttingDown = false;
        private static object m_Lock = new object();
        private static T m_Instance;

        public static T Instance
        {
            get
            {
                if (m_ShuttingDown)
                {
                    Debug.LogWarning("[Singleton] Instance '" + typeof(T) +
                                     "' already destroyed. Returning null.");
                    return null;
                }

                if (m_Instance == null)
                {
                    m_Instance = (T) FindObjectOfType(typeof(T));
                    if (m_Instance == null)
                    {
                        Debug.Log("[Singleton] Instance Created '" + typeof(T) +
                                         "' because returning null.");
                        var go = new GameObject(typeof(T).Name);
                        m_Instance = go.AddComponent<T>();
                    }
                }

                return m_Instance;
            }
        }
    }

  
}
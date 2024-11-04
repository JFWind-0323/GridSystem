using UnityEngine;

public class Agent<T> : MonoBehaviour
    where T : Agent<T>
{
    private static T instance;
    private static readonly object lockObject = new object();

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                lock (lockObject)
                {
                    if (instance == null)
                    {
                        GameObject obj = GameObject.Find("MonoSingle");
                        if (obj == null)
                        {
                            obj = new GameObject("MonoSingle");
                        }
                        instance = obj.GetComponent<T>();
                        if (instance == null)
                        {
                            instance = obj.AddComponent<T>();
                        }
                    }
                }
            }
            return instance;
        }
    }

    protected virtual void Awake()
    {
        if (instance == null)
        {
            instance = this as T;
            DontDestroyOnLoad(gameObject); // 保持单例在切换场景时不被销毁
        }
        else if (instance != this)
        {
            Destroy(gameObject); // 确保只存在一个实例
        }
    }

    protected virtual void OnDestroy()
    {
        if (instance == this)
        {
            instance = null; // 清理引用
        }
    }
}

    using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Component
{

    private static T instance;

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<T>();
                if (instance == null)
                {
                    GameObject gO = new GameObject();
                    instance = gO.AddComponent<T>();
                }
            }

            return instance;

        }
    }
    
    protected virtual void Awake()
    {
        instance = this as T;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

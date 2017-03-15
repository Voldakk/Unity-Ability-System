using UnityEngine;

public class MenuManager : MonoBehaviour
{
    static MenuManager instance;

    public string startMenu;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        Change(startMenu);
    }

    public void Change(string menuName)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(transform.GetChild(i).name == menuName);
        }
    }

    public static void ChangeMenu(string menuName)
    {
        instance.Change(menuName);
    }
}
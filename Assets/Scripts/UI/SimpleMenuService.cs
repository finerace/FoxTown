using UnityEngine;

public class SimpleMenuService : MonoBehaviour
{
    [SerializeField] private GameObject[] menus;

    private void Start()
    {
        OpenMenu(0);
    }

    public void OpenMenu(int id)
    {
        for (int i = 0; i < menus.Length; i++)
        {
            var item = menus[i];
            
            if(id == i)
                item.SetActive(true);
            else
                item.SetActive(false);
        }   
    }

}

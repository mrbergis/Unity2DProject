using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance = null;

    [SerializeField]
    List<GameObject> hearts = new List<GameObject>();

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        SetHealth(3);
    }

    public void SetHealth(int health)
    {
        for(int index = 0; index < hearts.Count; ++index)
        {
            if(index < health)
            {
                hearts[index].SetActive(true);
            }
            else
            {
                hearts[index].SetActive(false);
            }
        }
    }
}

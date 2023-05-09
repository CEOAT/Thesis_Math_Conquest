using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPageButton : MonoBehaviour
{
    [SerializeField] public GameObject openObject;
    [SerializeField] public GameObject openObjectAnother;
    [SerializeField] public GameObject closeObject;
    [SerializeField] public GameObject closeObjectAnother;


    public void OpenWindow()
    {
        if(openObject != null)
        {
            openObject.SetActive(true);
        }

        if(openObjectAnother != null)
        {
            openObjectAnother.SetActive(true);
        }

        if(closeObjectAnother != null)
        {
            closeObjectAnother.SetActive(false);
        }

        if(closeObject != null)
        {
            closeObject.SetActive(false);
        }
    }
}

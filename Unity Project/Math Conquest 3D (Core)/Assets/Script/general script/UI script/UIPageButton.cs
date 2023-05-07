using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPageButton : MonoBehaviour
{
    public GameObject openObject;
    public GameObject closeObject;

    public void OpenWindow()
    {
        openObject.SetActive(true);
        closeObject.SetActive(false);
    }
}

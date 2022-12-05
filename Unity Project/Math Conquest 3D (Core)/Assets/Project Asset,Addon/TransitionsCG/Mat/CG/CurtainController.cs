using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CurtainController : MonoBehaviour
{
    [SerializeField] private Image color_01;
    [SerializeField] private string curtainSceneName;
    [SerializeField] private float curtainSpeed;
    [SerializeField] private bool hasEnter;
    private float radiusColor_01;
    private bool enter;    
    
    // Start is called before the first frame update
    void Start()
    {        
        Initialize();        
    }

    private void Initialize()
    {
        if (!hasEnter)
        {
            if (color_01 != null)
            {      
                radiusColor_01 = 0; 
                color_01.material.SetFloat("_Cutoff", radiusColor_01); // UP TO 1     
            }
        }
        else
        {
             if (color_01 != null)
            {       
                radiusColor_01 = 1;
                color_01.material.SetFloat("_Cutoff", radiusColor_01); 
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {            
            StartCoroutine(ActiveCanvas());
        }

        if (enter &&  color_01.material.GetFloat("_Cutoff") <= 1)
        {
            radiusColor_01 += Time.deltaTime * curtainSpeed;
            color_01.material.SetFloat("_Cutoff", radiusColor_01); // UP TO 1     
        }

        if (hasEnter && color_01.material.GetFloat("_Cutoff") >= 0)
        {
            radiusColor_01 -= Time.deltaTime * curtainSpeed;
            color_01.material.SetFloat("_Cutoff", radiusColor_01); // UP TO 1    

            if(color_01.material.GetFloat("_Cutoff") <= 0) 
            {
                color_01.material.SetFloat("_Cutoff", 0);   
                hasEnter = false;    
            }
        }       
    }

    IEnumerator ActiveCanvas()
    {        
        enter = true;
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(curtainSceneName);
    }
}

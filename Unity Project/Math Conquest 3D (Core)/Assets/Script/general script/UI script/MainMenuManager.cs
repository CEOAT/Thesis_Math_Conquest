using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MainMenuManager : MonoBehaviour
{
    public Image transistion;
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void playtransistion()
    {
        transistion.raycastTarget = false;
        StartCoroutine(ChangeTransistionMaterial());
    }

    IEnumerator ChangeTransistionMaterial()
    {
        transistion.raycastTarget = true;
        yield return new WaitForSeconds(1f);
        Material tempmat = transistion.material;
        float t = 0;
        Sequence s = DOTween.Sequence();
        s.Append(DOVirtual.Float(0, 1f, 1f, v => tempmat.SetFloat("_Cutoff",v )));
        yield return new WaitForSeconds(1.25f);
        s.Append(DOVirtual.Float(1F, 0f, 1f, v => tempmat.SetFloat("_Cutoff",v )));
        transistion.raycastTarget = false;
       
        yield break;
    }
}

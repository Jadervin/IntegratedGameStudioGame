using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class HoverTips : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    public string tipToShow;
    private float timeToWait = 0.2f;


    public void OnPointerEnter(PointerEventData eventData)
    {
        StopAllCoroutines();
        StartCoroutine(StartTimer());
    }


    public void OnPointerExit(PointerEventData eventData)
    {
        StopAllCoroutines();
        
        HoverTipManager.OnMouseLoseFocus();


    }


    public void OnButtonPresssToDisable()
    {
        StopAllCoroutines();
        
        HoverTipManager.OnMouseLoseFocus();
    }

    private void ShowMessage()
    {
        StopAllCoroutines();
        HoverTipManager.OnMouseHover(tipToShow, Input.mousePosition);

    }

    private IEnumerator StartTimer()
    {
        yield return new WaitForSeconds(timeToWait);


        ShowMessage();

    }

}

using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class Draggable : MonoBehaviour, 
    IDragHandler, 
    IEndDragHandler, 
    IPointerEnterHandler, 
    IPointerExitHandler, 
    IPointerDownHandler
{
    public Transform Target;
    private bool dragging;
    
    public void OnDrag(PointerEventData eventData)
    {
        this.transform.position = eventData.position;
        dragging = true;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        dragging = false;
    }
    

    private void Update()
    {
        if (!dragging)
        {
            transform.position = Vector3.Lerp(transform.position, Target.position, Time.deltaTime * 10);    
        }

        var xRelPos = (Screen.width - transform.position.x) / Screen.width;

        var rot = transform.localRotation;
        rot.z = Mathf.Lerp(- Mathf.PI / 8, Mathf.PI / 8, xRelPos);

        transform.localRotation = rot;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.DOScale(1.3f, 0.2f).Play();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.DOScale(1, 0.2f).Play();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        transform.DOScale(1, 0.2f).Play();
    }
}

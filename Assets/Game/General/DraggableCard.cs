using System.Collections;
using DG.Tweening;
using Game.Event;
using Game.General;
using UnityEngine;
using UnityEngine.EventSystems;
using EventSystem = Game.General.EventSystem;

public class DraggableCard : MonoBehaviour, 
    IBeginDragHandler,
    IDragHandler, 
    IEndDragHandler, 
    IPointerEnterHandler, 
    IPointerExitHandler, 
    IPointerDownHandler
{
    public CardView CardView;
    public Transform Target;
    private bool dragging;
    private Vector3 startingPosition;
    
    public void OnDrag(PointerEventData eventData)
    {
        this.transform.position = eventData.position;
        dragging = true;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        dragging = false;

        if (Mathf.Abs(startingPosition.y - transform.position.y) > 500)
        {
            var xDeltaPos = transform.position.x - (Screen.width / 2f);
            var pos = Position.LEFT;
            if (xDeltaPos > - Screen.width / 6f && xDeltaPos < Screen.width / 6f)
            {
                pos = Position.MIDDLE;
            } 
            else if (xDeltaPos > Screen.width / 6f)
            {
                pos = Position.RIGHT;
            }
            
            EventSystem.Instance.Raise(new TryUseCardEvent()
            {
                CardView = CardView,
                Position = pos
            });
        }
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

    public void OnBeginDrag(PointerEventData eventData)
    {
        startingPosition = transform.position;
    }
}

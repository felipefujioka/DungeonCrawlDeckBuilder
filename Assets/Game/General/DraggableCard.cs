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

    private Camera camera;
    
    private void Start()
    {
        camera = Camera.main;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(transform.parent as RectTransform, eventData.position,
            camera, out pos);
        this.transform.position = transform.parent.TransformPoint(pos);
        dragging = true;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        dragging = false;

        var screenHeight = camera.orthographicSize * 2;
        var screenWidth = camera.orthographicSize * 2 * camera.aspect;

        if (Mathf.Abs(startingPosition.y - transform.position.y) > screenHeight * 0.15f)
        {
            var xDeltaPos = transform.position.x + (screenWidth / 2f);
            var pos = Position.LEFT;
            if (xDeltaPos > screenWidth / 3f && xDeltaPos < 2 * screenWidth / 3f)
            {
                pos = Position.MIDDLE;
            } 
            else if (xDeltaPos > 2 * screenWidth / 3f)
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

        var screenWidth = camera.orthographicSize * 2 * camera.aspect;
        
        var xRelPos = (screenWidth / 2f + transform.position.x) / screenWidth;

        var rot = transform.localRotation;
        rot.z = - Mathf.Lerp(- Mathf.PI / 8, Mathf.PI / 8, xRelPos);

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

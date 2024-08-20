using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum Direction
{
    Up,
    Down,
    Left,
    Right
}

namespace Prefabs.UpcomingAppointments
{
    public class AppointmentList : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
    {
        public GameObject appointmentTemplate;

        private float _width, _height;
        private const float AspectRatio = 889f/202f, Scale = 800f/889f;
        private Vector2 _touchStart;
        private bool _isAnimating;

        private void Start()
        {
            _width = GetComponent<RectTransform>().rect.width;
            _height = _width/AspectRatio;
      
            //Set Images in MakeAnAppointment button
            foreach (Transform transform in transform.parent.Find("MakeAnAppointment").transform)
            {
                transform.GetComponent<RectTransform>().sizeDelta = new Vector2(_width, _height);
            
                if (transform.name == "Image1")
                {
                    transform.position = Move(transform.position, 1, Direction.Down);
                    transform.localScale = SetScale(transform.localScale, Scale);
                }
                else if (transform.name == "Image2")
                {
                    transform.position = Move(transform.position, 2, Direction.Down);
                    transform.localScale = SetScale(transform.localScale, Scale*Scale);
                }
                else if (transform.name == "Image3")
                {
                    transform.position = Move(transform.position, 3, Direction.Down);
                    transform.localScale = SetScale(transform.localScale, Scale*Scale*Scale);
                }
            }

            float height = transform.parent.Find("MakeAnAppointment").Find("Image0").position.y -
                transform.parent.Find("MakeAnAppointment").Find("Image3").position.y + transform.parent
                    .Find("MakeAnAppointment").Find("Image3").GetComponent<RectTransform>().rect.height;
        
            transform.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
            transform.parent.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);//Bottom
            transform.parent.Find("MakeAnAppointment").GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);

            float height2 = height + transform.parent.parent.Find("Top").GetComponent<RectTransform>().rect.height;
            transform.parent.parent.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height2);//UpcomingAppointments
        }

        public void InitializeList(IEnumerable<Appointment> appointments)
        {
            for (int i = 0; i < transform.childCount; i++) Destroy(transform.GetChild(i).gameObject);
        
            appointments = appointments.OrderByDescending(x => x.DateAppointmentStart);
        
            for (var i = 0; i < appointments.Count(); i++)
            {
                GameObject gameObject = Instantiate(appointmentTemplate, transform);
                gameObject.GetComponent<AppointmentTemplate>().Appointment = appointments.ElementAt(i);

                Transform gameObjectTransform = gameObject.transform;
                gameObjectTransform.GetComponent<RectTransform>().sizeDelta = new Vector2(_width, _height);
                gameObjectTransform.position = Move(gameObjectTransform.position, appointments.Count() -1 - i, Direction.Down);
                Image backgroundImage = gameObjectTransform.Find("Background").GetComponent<Image>();
            
                if (i == appointments.Count()-1)
                {
                    gameObjectTransform.localScale = SetScale(gameObjectTransform.localScale, 1f);
                    backgroundImage.sprite = Resources.Load<Sprite>("Patient/BackgroundTemplateAppointment0");
                }
                else if (i == appointments.Count()-2)
                {
                    gameObjectTransform.localScale = SetScale(gameObjectTransform.localScale, Scale);
                    backgroundImage.sprite = Resources.Load<Sprite>("Patient/BackgroundTemplateAppointment1");
                }
                else if (i == appointments.Count()-3)
                {
                    gameObjectTransform.localScale = SetScale(gameObjectTransform.localScale, Scale*Scale);
                    backgroundImage.sprite = Resources.Load<Sprite>("Patient/BackgroundTemplateAppointment2");
                }
                else if (i == appointments.Count()-4)
                {
                    gameObjectTransform.localScale = SetScale(gameObjectTransform.localScale, Scale*Scale*Scale);
                    backgroundImage.sprite = Resources.Load<Sprite>("Patient/BackgroundTemplateAppointment3");
                }
                else
                {
                    gameObjectTransform.localScale = SetScale(gameObjectTransform.localScale, 0f);
                }
            }
        }
    
        private void UpdateList(Direction direction)
        {
            if (direction == Direction.Down && CurrentChild() != transform.GetChild(transform.childCount - 1))
            {
                _isAnimating = true;

                for (int i = 0; i < transform.childCount; i++)
                {
                    transform.GetChild(i).transform.DOMove(Move(transform.GetChild(i).transform.position, 1, Direction.Down), 1f).OnComplete(() =>
                    {
                        _isAnimating = false;
                    });
                    Image backgroundImage = transform.GetChild(i).Find("Background").GetComponent<Image>();

                    if (i == CurrentChild().GetSiblingIndex() +1)
                    {
                        transform.GetChild(i).transform.DOScale(SetScale(transform.GetChild(i).transform.localScale, 1f), 1f);
                        backgroundImage.sprite = Resources.Load<Sprite>("Patient/BackgroundTemplateAppointment0");
                    }
                    else if (i == CurrentChild().GetSiblingIndex())
                    {
                        transform.GetChild(i).transform.DOScale(SetScale(transform.GetChild(i).transform.localScale, Scale), 1f);
                        backgroundImage.sprite = Resources.Load<Sprite>("Patient/BackgroundTemplateAppointment1");
                    }
                    else if (i == CurrentChild().GetSiblingIndex() -1)
                    {
                        transform.GetChild(i).transform.DOScale(SetScale(transform.GetChild(i).transform.localScale, Scale*Scale), 1f);
                        backgroundImage.sprite = Resources.Load<Sprite>("Patient/BackgroundTemplateAppointment2");
                    }
                    else if (i == CurrentChild().GetSiblingIndex() -2)
                    {
                        transform.GetChild(i).transform.DOScale(SetScale(transform.GetChild(i).transform.localScale, Scale*Scale*Scale), 1f);
                        backgroundImage.sprite = Resources.Load<Sprite>("Patient/BackgroundTemplateAppointment3");
                    }
                    else
                    {
                        transform.GetChild(i).transform.DOScale(SetScale(transform.GetChild(i).transform.localScale, 0f), 1f);
                    }
                }
            }
            else if (direction == Direction.Up && CurrentChild() != transform.GetChild(0))
            {
                _isAnimating = true;

                for (int i = 0; i < transform.childCount; i++)
                {
                    transform.GetChild(i).transform.DOMove(Move(transform.GetChild(i).transform.position, 1, Direction.Up), 1f).OnComplete(() =>
                    {
                        _isAnimating = false;
                    });
                    Image backgroundImage = transform.GetChild(i).Find("Background").GetComponent<Image>();

                    if (i == CurrentChild().GetSiblingIndex() -1)
                    {
                        transform.GetChild(i).transform.DOScale(SetScale(transform.GetChild(i).transform.localScale, 1f), 1f);
                        backgroundImage.sprite = Resources.Load<Sprite>("Patient/BackgroundTemplateAppointment0");
                    }
                    else if (i == CurrentChild().GetSiblingIndex() -2)
                    {
                        transform.GetChild(i).transform.DOScale(SetScale(transform.GetChild(i).transform.localScale, Scale), 1f);
                        backgroundImage.sprite = Resources.Load<Sprite>("Patient/BackgroundTemplateAppointment1");
                    }
                    else if (i == CurrentChild().GetSiblingIndex() -3)
                    {
                        transform.GetChild(i).transform.DOScale(SetScale(transform.GetChild(i).transform.localScale, Scale*Scale), 1f);
                        backgroundImage.sprite = Resources.Load<Sprite>("Patient/BackgroundTemplateAppointment2");
                    }
                    else if (i == CurrentChild().GetSiblingIndex() -4)
                    {
                        transform.GetChild(i).transform.DOScale(SetScale(transform.GetChild(i).transform.localScale, Scale*Scale*Scale), 1f);
                        backgroundImage.sprite = Resources.Load<Sprite>("Patient/BackgroundTemplateAppointment3");
                    }
                    else
                    {
                        transform.GetChild(i).transform.DOScale(SetScale(transform.GetChild(i).transform.localScale, 0f), 1f);
                    }
                }
            }
        }

        private static Vector3 SetScale(Vector3 vector, float scale)
        {
            vector.x = scale;
            vector.y = scale;
            return vector;
        }

        private Vector3 Move(Vector3 vector, int howManyTimes = 0, Direction direction = Direction.Up)
        {
            if (direction == Direction.Up)
            {
                for (int i = 0; i < howManyTimes; i++)
                {
                    vector.y += _height/3;
                }
            }
            else
            {
                for (int i = 0; i < howManyTimes; i++)
                {
                    vector.y -= _height/3;
                }
            }
        
            return vector;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (_isAnimating) return;
        
            _touchStart = eventData.position;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (_isAnimating) return;

            Vector2 touchEnd = eventData.position;
            float swipeDistance = touchEnd.y - _touchStart.y;
        
            if (Mathf.Abs(swipeDistance) < 30)
            {
                Globals.appointment = CurrentChild().GetComponent<AppointmentTemplate>().Appointment;
                MenuManager.Instance.OpenMenu(MenuScreenEnum.APPOINTMENTSTATUS);
            }
            else
            {
                Direction direction = swipeDistance > 0 ? Direction.Up : Direction.Down;
                UpdateList(direction);
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            // Debug.Log("Dragging " + gameObject.name);
        }

        private Transform CurrentChild()
        {
            return (from Transform t in transform where Mathf.Approximately(t.localScale.x, 1f) select t).FirstOrDefault();
        }
    }
}

using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Prefabs.UpcomingAppointments
{
    public class AppointmentTemplate : MonoBehaviour
    {
        public Appointment Appointment;

        private void Start()
        {
            transform.Find("Background").GetComponent<Image>().sprite = Resources.Load<Sprite>("BackgroundTemplateAppointment0");
            
            GameObject icon = transform.Find("Status").Find("Icon").gameObject;

            string iconImage = null;
        
            if (Appointment.Status == AppointmentStatus.Pending.ToString())
            {
                iconImage = "IconAppointmentStatusPending";
                icon.transform.DORotate(Vector3.forward * -180, 1f).SetLoops(-1);
            }
            else if (Appointment.Status == AppointmentStatus.Approved.ToString())
            {
                iconImage = "IconAppointmentStatusApproved";
            }
        
            if (!string.IsNullOrEmpty(iconImage)) icon.GetComponent<Image>().sprite = Resources.Load<Sprite>(iconImage);
            // if (!string.IsNullOrEmpty(iconImage)) icon.GetComponent<Image>().sprite = Resources.Load<Sprite>("Resources/"+iconImage);


            transform.Find("Status").Find("Text").GetComponent<TextMeshProUGUI>().text = Appointment.Status;
            if (Appointment.Consultant.ProfileImage != null) transform.Find("Mask").Find("ProfileImage").GetComponent<Image>().sprite = Helpers.ConvertToSprite(Appointment.Consultant.ProfileImage);
            transform.Find("FullName").GetComponent<TextMeshProUGUI>().text = Appointment.Consultant.FullName;
            transform.Find("Hospital").GetComponent<TextMeshProUGUI>().text = Appointment.Consultant.Hospital.Name;
            transform.Find("Right").Find("TextCalendar").GetComponent<TextMeshProUGUI>().text = Appointment.DateAppointmentStart.ToLocalTime().ToString("dddd, MMMM d");
            transform.Find("Right").Find("TextClock").GetComponent<TextMeshProUGUI>().text = Appointment.DateAppointmentStart.ToLocalTime().ToString("hh:mm tt");
        }
    }
}

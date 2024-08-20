using System;
using System.Collections.Generic;
using System.Linq;
using Realms;
using UnityEngine;
using UnityEngine.UI;

namespace Prefabs.UpcomingAppointments
{
    public class UpcomingAppointments : MonoBehaviour
    {
        public Toggle toggleApproved, togglePending;
        public Button makeAnAppointment;
        public AppointmentList appointmentList;
    
        private IDisposable _subscription;
        private readonly List<Appointment> _appointments = new();
        private bool _showApproved = true, _showPending = true; 
    
        private void Start()
        {
            IQueryable<Appointment> appointments = Globals.realm.All<Appointment>().Where(item => item.PatientID == Globals._signedinPatient.Id).OrderBy(item => item.DateAppointmentStart);
            _subscription = appointments.SubscribeForNotifications((sender, changes) =>
            {
                _appointments.Clear();
            
                foreach (Appointment item in appointments)
                {
                    if (item.DateAppointmentStart.UtcDateTime > DateTime.UtcNow) _appointments.Add(item);
                }
            
                UpdateAppointmentTable();
            });

            toggleApproved.onValueChanged.AddListener(isOn => {
                _showApproved = isOn;
                UpdateAppointmentTable();
            });
            togglePending.onValueChanged.AddListener(isOn => {
                _showPending = isOn;
                UpdateAppointmentTable();
            });
        
            makeAnAppointment.onClick.AddListener(() =>
            {
                MenuManager.Instance.OpenMenu(MenuScreenEnum.SURVEY);
            });
        }

        private void UpdateAppointmentTable()
        {
            IEnumerable<Appointment> appointmentsFiltered;

            if (_showApproved && _showPending)
            {
                appointmentsFiltered = _appointments.Where(x => x.Status == AppointmentStatus.Approved.ToString() || x.Status == AppointmentStatus.Pending.ToString());
            }
            else if (_showApproved && !_showPending)
            {
                appointmentsFiltered = _appointments.Where(x => x.Status == AppointmentStatus.Approved.ToString());
            }
            else if (!_showApproved && _showPending)
            {
                appointmentsFiltered = _appointments.Where(x => x.Status == AppointmentStatus.Pending.ToString());
            }
            else
            {
                // appointmentsFiltered = appointments.Where(x => x.Status != AppointmentStatus.Approved.ToString() && x.Status != AppointmentStatus.Pending.ToString());
                appointmentsFiltered = null;
            }

            if (appointmentsFiltered != null && appointmentsFiltered.Any())
            {
                makeAnAppointment.gameObject.SetActive(false);
                appointmentList.gameObject.SetActive(true);
                appointmentList.InitializeList(appointmentsFiltered);
            }
            else
            {
                makeAnAppointment.gameObject.SetActive(true);
                appointmentList.gameObject.SetActive(false);
            }
        }
    
        private void OnDestroy()
        {
            _subscription.Dispose();
        }
    }
}

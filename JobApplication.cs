using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobApplicationTracker
{
    public class JobApplication
    {
        // Integer for keeping track of applications 
        public int ID { get; set; }

        // string name of the company applied to
        public string? CompanyName { get; set; }

        // srting name of the position applied to
        public string? PositionName { get; set; }

        // date of application
        public DateOnly DateApplied { get; set; }
        public string? Notes { get; set; }

        //constructor
        public JobApplication()
        {

        }
        public JobApplication(string _companyName, string _positionName, DateOnly _dateTime, string _notes, ApplicationStatus _status, int _ID)
        {
            CompanyName = _companyName;
            PositionName = _positionName;
            DateApplied = _dateTime;
            Status = _status;
            Notes = _notes;
            ID = _ID;
        }

        public enum ApplicationStatus
        { 
            Applied,
            Interview,
            Rejected,
            Offer

        }

        public ApplicationStatus Status { get; set; }
    }
}

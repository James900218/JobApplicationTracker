using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Security.Permissions;

namespace JobApplicationTracker
{
    public class ApplicationsTracker
    {
        // list holding all the objects of type "JobApplication"
        private List<JobApplication> Jobs = new();

        // Initializing an ID so that jobs can be tracked easily
        public int applicationID = 1;

        public List<JobApplication> GetApplications()
        {
            return Jobs;
        }

        // Function to add new applications into the list
        public void AddApplication(string _companyName, string _positionName, DateOnly _dateTime, string _notes)
        {
            Jobs.Add(new JobApplication(_companyName, _positionName, _dateTime, _notes, JobApplication.ApplicationStatus.Applied, applicationID));

            // increment ID everytime a new job is added
            applicationID++;
        }

        // function that shows all applications
        public void ViewApplications()
        {
            if (Jobs.Count == 0)
            {
                Console.WriteLine("\nJobs list empty\n");
                return;
            }

            foreach (var job in Jobs)
            {
                Console.WriteLine($"\n{job.ID} | " +
                  $"{job.CompanyName} | " +
                  $"{job.PositionName} | " +
                  $"{job.Status} | " +
                  $"{job.Notes} | " +
                  $"{job.DateApplied} | \n");
            }
        }

        // function to remove an application from the list
        public void DeleteApplication(int _applicationID)
        {
            if (Jobs != null)
            {
                Jobs.RemoveAll(Jobs => Jobs.ID == _applicationID);
            }

        }

        // function that edits a field in a jobApplication 
        public void EditApplication(int _applicationID, int _editChoice, string _input)
        {

            // query to search lists for the correct job ID
            var job = Jobs.FirstOrDefault(j => j.ID == _applicationID);

            if (job == null)
            {
                Console.WriteLine("\nJob not found\n");
                return;
            }

            switch (_editChoice)
            {
                default:
                    break;
                case 1:
                    job.CompanyName = _input;

                    break;
                case 2:
                    job.PositionName = _input;

                    break;
                case 3:
                    if (int.TryParse(_input, out int choice))
                    {
                        job.Status = (JobApplication.ApplicationStatus)choice;
                    }
                    else
                    {
                        Console.WriteLine("\nFailed to read choice number\n");
                        Console.WriteLine(); // for empty line in console, formatting
                    }

                    break;
                case 4:
                    job.Notes = _input;

                    break;
                case 5:
                    DateOnly.TryParse(_input, out var dateOnly);
                    job.DateApplied = dateOnly;
                    break;
            }
        }

        // saves job list to a json file
        public void SaveToFile(string _filePath)
        {
            string jsonData = JsonSerializer.Serialize(Jobs);
            File.WriteAllText(_filePath, jsonData);
        }

        // loads and deserialises a json file into a jobs list object
        public void LoadFromFile(string _filePath)
        {
            if (!File.Exists(_filePath))
            {
                Console.WriteLine("\nno save file found.\n");
                return;
            }
            else
            {
                string jsonFile = File.ReadAllText(_filePath);

                // set job list to the deserialised file, if null add a new list
                Jobs =
                    JsonSerializer.Deserialize<List<JobApplication>>(jsonFile)
                    ?? new List<JobApplication>();

                // check applications contains something, if true get the highest ID, increment the new ID from this
                applicationID = Jobs.Any() ? Jobs.Max(j => j.ID) + 1 : 1;
            }
        }

        public JobApplication? SearchList(string _choice)
        {
            // Empty JobApplication
            JobApplication result = new();

            // Check list conatins any elements
            if (Jobs.Count > 0)
            {
                // does the input match any company names in the list, if not check position names
                if (Jobs.Any(j => j.CompanyName == _choice))
                {
                    result = Jobs.FirstOrDefault(j => j.CompanyName == _choice);
                    return result;
                }
                else if (Jobs.Any(j => j.PositionName == _choice))
                {
                    result = Jobs.FirstOrDefault(j => j.PositionName == _choice);
                    return result;
                }


            }
            

            return null;
        }

        public void FilterList(JobApplication.ApplicationStatus _status)
        {
            // Check the list has any elements in it
            if (Jobs.Count > 0)
            {
                List<JobApplication> filteredJobs = new();

                // Store filtered results
                IEnumerable<JobApplication> query = Jobs.Where(j => j.Status == _status);

                // unless empty
                if (query.Any())
                {
                    // add them to a new list
                    foreach (JobApplication jobApplication in query)
                    {
                        filteredJobs.Add(jobApplication);
                    }

                    // Print to console
                    foreach (JobApplication job in filteredJobs)
                    {
                        Console.WriteLine($"\n{job.ID} | " +
                        $"{job.CompanyName} | " +
                        $"{job.PositionName} | " +
                        $"{job.Status} | " +
                        $"{job.Notes} | " +
                        $"{job.DateApplied} | \n");
                    }
                }
                else Console.WriteLine("\nNo Applications Found");


                    return;
            }

            Console.WriteLine("Filter Result Failed");
            return;
        }

        public void SortList(string _choice)
        {
            IEnumerable<JobApplication> sortedJobs;

            if (!string.IsNullOrEmpty(_choice))
            {
                switch (_choice)
                {
                    default:
                        break;
                    case "1" or "Date":
                        sortedJobs = Jobs.OrderBy(j => j.DateApplied);

                        foreach (JobApplication job in sortedJobs)
                        {
                            Console.WriteLine($"\n{job.ID} | " +
                            $"{job.CompanyName} | " +
                            $"{job.PositionName} | " +
                            $"{job.Status} | " +
                            $"{job.Notes} | " +
                            $"{job.DateApplied} | \n");
                        }

                        break;

                    case "2" or "Company":
                        sortedJobs = Jobs.OrderBy(j => j.CompanyName);

                        foreach (JobApplication job in sortedJobs)
                        {
                            Console.WriteLine($"\n{job.ID} | " +
                            $"{job.CompanyName} | " +
                            $"{job.PositionName} | " +
                            $"{job.Status} | " +
                            $"{job.Notes} | " +
                            $"{job.DateApplied} | \n");
                        }
                        break;

                    case "3" or "Status":
                        sortedJobs = Jobs.OrderBy(j => j.Status);

                        foreach (JobApplication job in sortedJobs)
                        {
                            Console.WriteLine($"\n{job.ID} | " +
                            $"{job.CompanyName} | " +
                            $"{job.PositionName} | " +
                            $"{job.Status} | " +
                            $"{job.Notes} | " +
                            $"{job.DateApplied} | \n");
                        }
                        break;


                }
            }
        }

        public string ApplicationStatistics()
        {
            string result = "";

            result += "\nTotal Applications | " + Jobs.Count.ToString();
            result += "\nInterviews | " + Jobs.Count(j => j.Status == JobApplication.ApplicationStatus.Interview).ToString();
            result += "\nOffers | " + Jobs.Count(j => j.Status == JobApplication.ApplicationStatus.Offer).ToString();
            result += "\nRejected | " + Jobs.Count(j => j.Status == JobApplication.ApplicationStatus.Rejected).ToString();

            // num of responses
            // divide by total
            // multiply by 100

            int responses = Jobs.Count(j =>
                j.Status == JobApplication.ApplicationStatus.Offer ||
                j.Status == JobApplication.ApplicationStatus.Interview);

            int percentage = (responses * 100) / Jobs.Count;
            result += "\nResponse Rate | " + percentage.ToString();

            return result;
        }
    }
}

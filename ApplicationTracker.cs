using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace JobApplicationTracker
{
    public class ApplicationsTracker
    {
        // list holding all the objects of type "JobApplication"
        public List<JobApplication> Jobs = new();

        // Initializing an ID so that jobs can be tracked easily
        public int applicationID = 1;

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
            if (!Jobs.Any())
            {
                Console.WriteLine("Jobs list empty");
                return;
            }

            foreach (var job in Jobs)
            {
                Console.WriteLine($"{job.ID} | " +
                  $"{job.CompanyName} | " +
                  $"{job.PositionName} | " +
                  $"{job.Status} | " +
                  $"{job.Notes} | " +
                  $"{job.DateApplied} | ");
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
                Console.WriteLine("Job not found");
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
                        Console.WriteLine("Failed to read choice number");
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
                Console.WriteLine("no save file found.");
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
    }
}

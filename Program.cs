using JobApplicationTracker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.IO;


// This class handles the UI and Console interaction with the classes that hold the app functionality
class Program
{

    private static ApplicationsTracker applicationsTracker = new();

    private static string filePath = "C:\\Projects\\JobApplicationTracker\\Saved Lists\\";

    static void Main()
    {

        Console.WriteLine("Hello World");

        bool running = true;

        while (running)
        {
            using var db = new ApplicationDbContext();

            Console.WriteLine(db.Applications.Count());
            Console.WriteLine("\nWelcome! Please select an option from the list below:");

            Console.WriteLine("1: Add Application");
            Console.WriteLine("2: View Applications");
            Console.WriteLine("3: Delete Application");
            Console.WriteLine("4: Edit Application");
            Console.WriteLine("5: Save");
            Console.WriteLine("6: Load");
            Console.WriteLine("7: More Options");
            Console.WriteLine("8: Exit");


            string? choiceInput = Console.ReadLine();

            switch (choiceInput)
            {
                case "1":
                    ConsoleAddApplication();
                    break;

                case "2":
                    applicationsTracker.ViewApplications();
                    break;

                case "3":
                    ConsoleDeleteApplication();
                    break;

                case "4":
                    ConsoleEditApplication();             
                    break;
                case "5":
                    ConsoleSaveToFile();
                    break;

                case "6":
                    ConsoleLoadFromFile();                 
                    break;

                case "7":
                    Console.WriteLine("\n1: Search");
                    Console.WriteLine("2: Filter");
                    Console.WriteLine("3: Sort");
                    Console.WriteLine("4: Statistics");
                    Console.WriteLine("5: Back");

                    choiceInput = Console.ReadLine();

                    switch (choiceInput)
                    {
                        case "1":
                            ConsoleSearch();
                            break;
                        case "2":
                            ConsoleFilter();
                            break;
                        case "3":
                            ConsoleSort();
                            break;
                        case "4":
                            ConsoleStatistics();
                            break;
                        case "5":
                            break;
                    }


                    break;

                case "8":
                    running = false;
                    break;

            }


        }

    }

    static void ConsoleAddApplication()
    {
        Console.WriteLine("\nEnter the Name of the Company");
        string? companyName = Console.ReadLine();

        Console.WriteLine("\nEnter the title of the position applied for");
        string? roleName = Console.ReadLine();

        Console.WriteLine("\nEnter the date that you applied to the role (Format dd -space- mm -space- yyyy)");
        string? date = Console.ReadLine();
        DateOnly.TryParse(date, out var dateOnly);

        Console.WriteLine("\nEnter any notes about the application");
        string? notes = Console.ReadLine();

        if (companyName != null && roleName != null && date != null)
        {
            applicationsTracker.AddApplication(companyName, roleName, dateOnly, notes);
        }
        else Console.WriteLine("\nAdd Application Failed, Invalid Field\n"); return;

    }

    static void ConsoleDeleteApplication()
    {
        Console.WriteLine("\nEnter the ID of the application you wish to delete");
        string? choiceID = Console.ReadLine();

        if (int.TryParse(choiceID, out int id))
        {
            applicationsTracker.DeleteApplication(id);
        }
        else
        {
            Console.WriteLine("\nFailed to find job application from the given ID\n");
        }

    }

    static void ConsoleEditApplication()
    {
        Console.WriteLine("\nenter the ID number of the application you wish to edit");
        string? choice = Console.ReadLine();

        if (int.TryParse(choice, out int id))
        {
            if (applicationsTracker.GetApplications().Count >= id)
            {
                Console.WriteLine($"{applicationsTracker.GetApplications()[id - 1].CompanyName} |" +
                        $" {applicationsTracker.GetApplications()[id - 1].PositionName} | {applicationsTracker.GetApplications()[id - 1].Status} |" +
                        $" {applicationsTracker.GetApplications()[id - 1].Notes} | {applicationsTracker.GetApplications()[id - 1].DateApplied.ToString()}");

                Console.WriteLine("\nChoose a field to edit: ");
                Console.WriteLine("1: Company Name");
                Console.WriteLine("2: Position Name");
                Console.WriteLine("3: Status");
                Console.WriteLine("4: Notes");
                Console.WriteLine("5: Date");
                Console.WriteLine("6: Back");

                choice = Console.ReadLine();

                if (int.TryParse(choice, out int choiceID))
                {
                    // in the job that matches the ID
                    switch (choiceID)
                    {
                        default:
                            Console.WriteLine("\nInvalid Choice\n");
                            break;
                        case 1: // 1: change Company Name
                            Console.WriteLine("\nEnter new Company Name:");
                            choice = Console.ReadLine();
                            if (choice != null) applicationsTracker.EditApplication(id, choiceID, choice);
                            break;

                        case 2: // 2: change Position Name
                            Console.WriteLine("\nEnter new Position Name:");
                            choice = Console.ReadLine();
                            if (choice != null) applicationsTracker.EditApplication(id, choiceID, choice);
                            break;

                        case 3: // 3: Change Application Status
                            Console.WriteLine("\nEnter the number from the options below:");
                            Console.WriteLine("0: Applied");
                            Console.WriteLine("1: Interview");
                            Console.WriteLine("2: Rejected");
                            Console.WriteLine("3: Offer");

                            choice = Console.ReadLine();
                            if (choice != null) applicationsTracker.EditApplication(id, choiceID, choice); 
                            break;

                        case 4: // 4: Change Application Notes
                            Console.WriteLine("\nEnter new Notes:");
                            choice = Console.ReadLine();
                            if (choice != null) applicationsTracker.EditApplication(id, choiceID, choice);
                            break;
                        case 5: // 5: Change application date
                            Console.WriteLine("\nEnter new date, format (dd -space- mm -space- yyyy):");
                            choice = Console.ReadLine();
                            if (choice != null) applicationsTracker.EditApplication(id, choiceID, choice);
                            break;


                    }
                }
                else
                {
                    Console.WriteLine("Failed to read choice number");
                    Console.WriteLine(); // for empty line in console, formatting
                }




            }
            else return;

        }
        else 
        {
            Console.WriteLine("Failed to read application ID");
            Console.WriteLine(); // for empty line in console, formatting
            return;       
        }
    }

    static public void ConsoleSaveToFile()
    {
        // Save a new file in the current file path
        Console.WriteLine("\nSave as: ");
        string? choice = Console.ReadLine();
        choice = filePath + choice + ".json";

        applicationsTracker.SaveToFile(choice);
    }

    static public void ConsoleLoadFromFile()
    {
        Console.WriteLine("\nEnter File name: ");
        string? choice = Console.ReadLine();
        choice = filePath + choice + ".json";

        applicationsTracker.LoadFromFile(choice);
    }

    static public void ConsoleSearch()
    {
        Console.WriteLine("\nEnter your search: ");
        string? choice = Console.ReadLine();

        if (string.IsNullOrEmpty(choice))
        {
            Console.WriteLine("\nInvalid Search\n");
            return;
        }

        JobApplication? searchResult = applicationsTracker.SearchList(choice);

        if (searchResult != null)
        {
            Console.WriteLine($"{searchResult.ID} | " +
            $"{searchResult.CompanyName} | " +
            $"{searchResult.PositionName} | " +
            $"{searchResult.Status} | " +
            $"{searchResult.Notes} | " +
            $"{searchResult.DateApplied} | ");
        }
        else Console.WriteLine("\nSearch result not found\n");

            return;
    }

    static public void ConsoleFilter()
    {
        Console.WriteLine("\nEnter Filter Option:");
        Console.WriteLine("1: Applied");
        Console.WriteLine("2: Interview");
        Console.WriteLine("3: Rejected");
        Console.WriteLine("4: Offer");

        string? choice = Console.ReadLine();

        switch(choice)
        {
            default:
                Console.WriteLine("\nInvalid Input\n");
                break;

            case "1" or "Applied":
                applicationsTracker.FilterList(JobApplication.ApplicationStatus.Applied);
                break;

            case "2" or "Interview":
                applicationsTracker.FilterList(JobApplication.ApplicationStatus.Interview);
                break;

            case "3" or "Rejected":
                applicationsTracker.FilterList(JobApplication.ApplicationStatus.Rejected);
                break;

            case "4" or "Offer":
                applicationsTracker.FilterList(JobApplication.ApplicationStatus.Offer);
                break;


        }

        return;
    }

    static public void ConsoleSort()
    {
        Console.WriteLine("\nSort by: ");
        Console.WriteLine("1: Date Applied ");
        Console.WriteLine("2: Company ");
        Console.WriteLine("3: Status ");

        string? choice = Console.ReadLine();

        applicationsTracker.SortList(choice);
    }

    static public void ConsoleStatistics()
    {
        Console.WriteLine(applicationsTracker.ApplicationStatistics());
    }

}







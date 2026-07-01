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
            Console.WriteLine("Welcome! Please select an option from the list below:");

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
                    Console.WriteLine("1: Search");
                    Console.WriteLine("2: Filter");
                    Console.WriteLine("3: Sort");
                    Console.WriteLine("4: Statistics");
                    Console.WriteLine("5: Back");

                    break;

                case "8":
                    running = false;
                    break;

            }


        }

    }

    static void ConsoleAddApplication()
    {
        Console.WriteLine("Enter the Name of the Company");
        string? companyName = Console.ReadLine();

        Console.WriteLine("Enter the title of the position applied for");
        string? roleName = Console.ReadLine();

        Console.WriteLine("Enter the date that you applied to the role (Format dd -space- mm -space- yyyy)");
        string? date = Console.ReadLine();
        DateOnly.TryParse(date, out var dateOnly);

        Console.WriteLine("Enter any notes about the application");
        string? notes = Console.ReadLine();

        if (companyName != null && roleName != null && date != null)
        {
            applicationsTracker.AddApplication(companyName, roleName, dateOnly, notes);
        }
        else Console.WriteLine("Add Application Failed, Invalid Field"); return;

    }

    static void ConsoleDeleteApplication()
    {
        Console.WriteLine("Enter the ID of the application you wish to delete");
        string? choiceID = Console.ReadLine();

        if (int.TryParse(choiceID, out int id))
        {
            applicationsTracker.DeleteApplication(id);
        }
        else
        {
            Console.WriteLine("Failed to find job application from the given ID");
        }

    }

    static void ConsoleEditApplication()
    {
        Console.WriteLine("enter the ID number of the application you wish to edit");
        string? choice = Console.ReadLine();

        if (int.TryParse(choice, out int id))
        {
            if (applicationsTracker.GetApplications().Count >= id)
            {
                Console.WriteLine($"{applicationsTracker.GetApplications()[id - 1].CompanyName} |" +
                        $" {applicationsTracker.GetApplications()[id - 1].PositionName} | {applicationsTracker.GetApplications()[id - 1].Status} |" +
                        $" {applicationsTracker.GetApplications()[id - 1].Notes} | {applicationsTracker.GetApplications()[id - 1].DateApplied.ToString()}");

                Console.WriteLine("Choose a field to edit: ");
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
                            Console.WriteLine("Invalid Choice");
                            break;
                        case 1: // 1: change Company Name
                            Console.WriteLine("Enter new Company Name:");
                            choice = Console.ReadLine();
                            if (choice != null) applicationsTracker.EditApplication(id, choiceID, choice);
                            break;

                        case 2: // 2: change Position Name
                            Console.WriteLine("Enter new Position Name:");
                            choice = Console.ReadLine();
                            if (choice != null) applicationsTracker.EditApplication(id, choiceID, choice);
                            break;

                        case 3: // 3: Change Application Status
                            Console.WriteLine("Enter the number from the options below:");
                            Console.WriteLine("0: Applied");
                            Console.WriteLine("1: Interview");
                            Console.WriteLine("2: Rejected");
                            Console.WriteLine("3: Offer");

                            choice = Console.ReadLine();
                            if (choice != null) applicationsTracker.EditApplication(id, choiceID, choice); 
                            break;

                        case 4: // 4: Change Application Notes
                            Console.WriteLine("Enter new Notes:");
                            choice = Console.ReadLine();
                            if (choice != null) applicationsTracker.EditApplication(id, choiceID, choice);
                            break;
                        case 5: // 5: Change application date
                            Console.WriteLine("Enter new date, format (dd -space- mm -space- yyyy):");
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
        Console.WriteLine("Save as: ");
        string? choice = Console.ReadLine();
        choice = filePath + choice + ".json";

        applicationsTracker.SaveToFile(choice);
    }

    static public void ConsoleLoadFromFile()
    {
        Console.WriteLine("Enter File name: ");
        string? choice = Console.ReadLine();
        choice = filePath + choice + ".json";

        applicationsTracker.LoadFromFile(choice);
    }

}







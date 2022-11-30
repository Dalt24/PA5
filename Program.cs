using System;
using System.IO;
using System.Linq;
using PA5Test4;
// Manager password is ‘password’
// Used List of Objects
// Used LINQ to directly query data & update data
// User Can report movie rating when returning movie
// User can Rent / Return multiple movies
// Manager can delete 1 or All copies of same movie title
// Movie can be Any genre; manager chooses genre when adding movie
// Movie Count by Genre
// Ratings of All Movies
// Average Rating of All Movies
// Allowed User to type number OR the Movie title to rent a movie without prompting them to choose 1
// GUID MovieID and GUID TransactionID to make each movie Unique
// Proper tracking of GUID so data isn’t lost between the two .txt files
// Data on Whether transaction is outstanding or already returned
// Used TryParse to allow user to type a number (ex. 1) or type a string (ex. Christmas Vacation)
// User can At Most Rent 3 Movies at Once unless they return one

namespace PA5Test4
{
    class Program
    {
        static void Main()
        {
            string userChoice = "";
            string managerChoice = "";
            string customerChoice = "";

            List<Movie> listMov = new List<Movie>(); // creating list of Movie Objects
            List<Transaction> listTrans = new List<Transaction>(); // creating list of Transaction Objects

            listMov = MovieUtility.LoadListMov(listMov); // filling movie List with data from .txt
            listTrans = TransactionUtility.LoadListTrans(listTrans); // filling Transaction list with data from .txt
            

            managerEnd:
            WriteLine("Welcome to VideoMart!\n",ConsoleColor.Green);
            while(userChoice != "9" && userChoice != "9." && userChoice != "EXIT") // while user hasn't selected 'exit'
            {
                userChoice = userWelcome().ToUpper();   
                customerChoice = "0";    
                if(userChoice == "1" || userChoice == "1." || userChoice == "CUSTOMER") // user menu start if()
                {
                    while(customerChoice != "9" && customerChoice != "9." && customerChoice != "EXIT")// while user hasn't selected 'exit'
                    {
                        customerChoice = customerWelcome().ToUpper();
                        Console.Clear();
                        if(customerChoice == "1" || customerChoice == "1." || customerChoice == "VIEW")
                        {
                            WriteLine("The Movies Currently in Stock Are:\n",ConsoleColor.White);
                            MovieReport.GetAllMoviesInStock(listMov); // shows all movies IN Stock
                        }
                        else if(customerChoice == "2" || customerChoice == "2." || customerChoice == "RENT")TransactionUtility.RentAMovie(listMov, listTrans); // calls rent method
                        else if(customerChoice == "3" || customerChoice == "3." || customerChoice == "RENTED") // calls method to show rented movies by email
                        {
                            System.Console.WriteLine("What is the Email Associated with Your Account?");
                            string email = ReadLine(); // gets email to find movies rented
                            TransactionReport.GetRentedMoviesByEmail(listTrans,email);
                        }
                        else if(customerChoice == "4" || customerChoice == "4." || customerChoice == "RETURN")TransactionUtility.ReturnAMovie(listMov,listTrans); // return method
                        else if(customerChoice == "9" || customerChoice == "9." || customerChoice == "EXIT")goto managerEnd;// selected 'exit'
                        else if(customerChoice != "9")WriteLine("Invalid Choice, Try Again!",ConsoleColor.Green); // invalid choices 
                        PressToContinue();
            
                    }
                } // user menu end if()
                else if(userChoice == "2" || userChoice == "2." || userChoice == "MANAGER")
                { // manager menu start if()
                    System.Console.WriteLine("Hello Manager! Type in the Manager password to Login!");
                    string mngPass = ReadLine(); 
                    if(mngPass != "password"){ // checking manager password, if incorrect sends out of menu 
                        System.Console.WriteLine("Incorrect Password\nGo Back to the Menu and Try Again\nHint:the Password is 'password");
                        PressToContinue();
                        goto managerEnd;
                    }
                    while(managerChoice != "5" && managerChoice != "5." && managerChoice != "EXIT")// while user not selected 'exit'
                    {
                        //manager menu for Add, Remove, Edit, Report, Exit
                        managerChoice = managerWelcome().ToUpper();
                        if(managerChoice == "1" || managerChoice == "1." || managerChoice == "ADD")MovieUtility.AddMovieToStock(listMov); // call add method
                        else if(managerChoice == "2" || managerChoice == "2." || managerChoice == "REMOVE")MovieUtility.RemoveMovieFromStock(listMov); // call remove method
                        else if(managerChoice == "3" || managerChoice == "3." || managerChoice == "EDIT")MovieUtility.EditMovieFromStock(listMov); // call edit method
                        else if(managerChoice == "4" || managerChoice == "4." || managerChoice == "REPORT")ReportMenu(listMov, listTrans); // call report menu
                        else if(managerChoice == "9" || managerChoice == "9." || managerChoice == "EXIT")// selected 'exit'
                        {
                            Console.Clear();
                            managerChoice = "9";
                            customerChoice = "0";
                            userChoice = "0";
                            goto managerEnd; // when manager exits menu it resets admin choices so they have to login again.
                        }
                        else
                        {
                            System.Console.WriteLine(""); // catch for invalid choice
                            WriteLine("Invalid Choice, Try Again!",ConsoleColor.Green);
                        }
                    }
                } // manager menu end if()
                else if(userChoice=="3" || userChoice == "3." || userChoice == "EXIT")WriteLine("GoodBye! Thanks for Visiting VideoMart!", ConsoleColor.Green); // exit msg
                else if(userChoice != "3")WriteLine("Invalid Choice, Try Again!",ConsoleColor.Green); // failed user choice catch
            }



        static void ReportMenu(List<Movie> listMov, List<Transaction> listTrans)
        {
            System.Console.WriteLine("************************************************");
            start:
            string reportChoice = reportWelcome(); // call text welcome
                bool tryp;
            tryp = int.TryParse(reportChoice, out int k); // try to parse users choice
            Console.Clear();

            if(tryp == false)WriteLine("Invalid Choice, Try Again With an Integer",ConsoleColor.White);
            if(k == 1)MovieReport.GetAllMoviesInStock(listMov); // in stock
            else if(k == 2)TransactionReport.GetCurrentlyRentedMovies(listTrans); // currently rented
            else if(k == 3)TransactionReport.GetRentalTotalsByGenre(listTrans); // rentalbygenre
            else if(k == 4)TransactionReport.GetTop5InTermOfRental(listTrans); // top5rental
            else if(k == 5)MovieReport.GetAllGenres(listMov); // all genres
            else if(k == 6)TransactionReport.GetAllRating(listTrans); // all ratings
            else if(k == 7)TransactionReport.OverallMovieRatingAverage(listTrans); // overall movie rating avg
            else if(k == 9) // exit choice
            if(k != 9) // if user doesn't choose to exit but made selection not in menu, loops to startMenu prompt
            {
                PressToContinue();
                goto start;
            }
            System.Console.WriteLine("************************************************");
        }

        static string reportWelcome()
        { // txt msg manager -> report
            Console.Clear();
            System.Console.WriteLine("Choose What Report You Would Like to Access");
            WriteLine($"\n1. In Stock Movies\n2. Movies Currently Rented\n3. Rentals Total Per Genre\n4. Top 5 Movies in Terms of Rentals\n5. Movie Count by Genre",ConsoleColor.Green);
            WriteLine($"6. List Ratings of All Movies\n7. Average Rating of All Movies\n9. Exit",ConsoleColor.Green);
            return ReadLine();
        }



            static string customerWelcome()
            { // txt msg customer
                Console.Clear();
                WriteLine("\nHello Customer!\nChoose an Option from the Menu Below!\n",ConsoleColor.White);
                WriteLine("1. View Movies Available to Rent",ConsoleColor.Green);
                WriteLine("2. Rent a Movie",ConsoleColor.Green);
                WriteLine("3. View Currently Rented Movies",ConsoleColor.Green);
                WriteLine("4. Return a Rented Movie",ConsoleColor.Green);
                WriteLine("9. Exit to Menu",ConsoleColor.Green);
                return ReadLine();   
            }
            
            static string managerWelcome()
            { // txt msg manager
                WriteLine("Hello Manager!\nChoose an Option from the Menu Below!\n",ConsoleColor.White);
                WriteLine("1. Add a New Movie to the Inventory",ConsoleColor.Green);
                WriteLine("2. Remove a Movie from the Inventory",ConsoleColor.Green);
                WriteLine("3. Edit Information about a Movie",ConsoleColor.Green);
                WriteLine("4. Access the Report Menu",ConsoleColor.Green);
                WriteLine("9. Exit to Menu",ConsoleColor.Green);
                return ReadLine();
            }

            static string userWelcome()
            { // txt msg main menu 
                WriteLine("Hello User!\nChoose an Option from the Menu Below! ex:(1)\n",ConsoleColor.White);
                WriteLine("1. You are a VideoMart Customer ", ConsoleColor.Green);
                WriteLine("2. You are a VideoMart Manager",ConsoleColor.Green);
                WriteLine("9. You would like to Exit the Program",ConsoleColor.Green);
                return ReadLine();
            }



            static string ReadLine() //method to use instead of console.ReadLine(); (mostly to avoid null warning which is annoying)
            {
                var line = Console.ReadLine();
                return line ?? string.Empty;
            } 

            static void WriteLine(string msg, ConsoleColor storeColor) //method to use instead of console.WriteLine(); to Choose colors 
            {
                Console.ForegroundColor = storeColor; 
                Console.WriteLine(msg);
                Console.ResetColor();
            }

            static void PressToContinue() // method to keep console from clearing so user can read until they press button
            {
                WriteLine("\nPress Any Key to Continue",ConsoleColor.White);
                Console.ReadKey();
            }
        }
        
    }
 
}
using System;
using System.IO;
using System.Linq;
using PA5Test4;

namespace PA5Test4
{
    class Program
    {
        static void Main()
        {
            string userChoice = "";
            string managerChoice = "";
            string customerChoice = "";

            List<Movie> listMov = new List<Movie>();//
            List<Transaction> listTrans = new List<Transaction>();

            listMov = MovieUtility.LoadListMov(listMov);
            listTrans = TransactionUtility.LoadListTrans(listTrans);
            

            managerEnd:
            WriteLine("Welcome to VideoMart!\n",ConsoleColor.Green);
            while(userChoice != "9" && userChoice != "9." && userChoice != "EXIT")
            {
                userChoice = userWelcome().ToUpper();   
                customerChoice = "0";    
                if(userChoice == "1" || userChoice == "1." || userChoice == "CUSTOMER")
                {
                    while(customerChoice != "9" && customerChoice != "9." && customerChoice != "EXIT")
                    {
                        customerChoice = customerWelcome().ToUpper();
                        if(customerChoice == "1" || customerChoice == "1." || customerChoice == "VIEW")
                        {
                            Console.Clear();
                            WriteLine("The Movies Currently in Stock Are:\n",ConsoleColor.White);
                            MovieReport.GetAllMoviesInStock(listMov);
                            PressToContinue();
                        }
                        else if(customerChoice == "2" || customerChoice == "2." || customerChoice == "RENT")
                        {
                            Console.Clear();
                            TransactionUtility.RentAMovie(listMov, listTrans);
                            PressToContinue();
                        }
                        else if(customerChoice == "3" || customerChoice == "3." || customerChoice == "RENTED")
                        {
                            Console.Clear();
                            System.Console.WriteLine("What is the Email Associated with Your Account?");
                            string email = ReadLine();
                            TransactionReport.GetRentedMoviesByEmail(ref listTrans, ref email);
                            PressToContinue();
                        }
                        else if(customerChoice == "4" || customerChoice == "4." || customerChoice == "RETURN")
                        {
                            Console.Clear();
                            TransactionUtility.ReturnAMovie(ref listMov, ref listTrans);
                            PressToContinue();
                        }
                        else if(customerChoice == "9" || customerChoice == "9." || customerChoice == "EXIT")
                        {
                            Console.Clear();
                            goto managerEnd;
                        }
                        else if(customerChoice != "9")
                        {
                            System.Console.WriteLine("");
                            WriteLine("Invalid Choice, Try Again!",ConsoleColor.Green);
                        }
                    }
                }
                else if(userChoice == "2" || userChoice == "2." || userChoice == "MANAGER")
                {
                    System.Console.WriteLine("Hello Manager! Type in the Manager password to Login!");
                    string mngPass = ReadLine();
                    if(mngPass != "password"){
                        System.Console.WriteLine("Incorrect Password\nGo Back to the Menu and Try Again\nHint:the Password is 'password");
                        PressToContinue();
                        goto managerEnd;
                    }
                    while(managerChoice != "5" && managerChoice != "5." && managerChoice != "EXIT")
                    {
                        
                        managerChoice = managerWelcome().ToUpper();
                        if(managerChoice == "1" || managerChoice == "1." || managerChoice == "ADD")
                        {
                            MovieUtility.AddMovieToStock(ref listMov);
                        }
                        else if(managerChoice == "2" || managerChoice == "2." || managerChoice == "REMOVE")
                        {
                            MovieUtility.RemoveMovieFromStock(listMov);
                        }
                        else if(managerChoice == "3" || managerChoice == "3." || managerChoice == "EDIT")
                        {
                            MovieUtility.EditMovieFromStock(listMov);
                        }
                        else if(managerChoice == "4" || managerChoice == "4." || managerChoice == "REPORT")
                        {
                            ReportMenu(ref listMov, ref listTrans);
                        }
                        else if(managerChoice == "9" || managerChoice == "9." || managerChoice == "EXIT")
                        {
                            Console.Clear();
                            managerChoice = "9";
                            customerChoice = "0";
                            userChoice = "0";
                            goto managerEnd;
                        }
                        else
                        {
                            System.Console.WriteLine("");
                            WriteLine("Invalid Choice, Try Again!",ConsoleColor.Green);
                        }
                    }
                }
                else if(userChoice=="3" || userChoice == "3." || userChoice == "EXIT")
                {
                    WriteLine("GoodBye! Thanks for Visiting VideoMart!", ConsoleColor.Green);
                }
                else WriteLine("Invalid Choice, Try Again!",ConsoleColor.Green);
            }



        static void ReportMenu(ref List<Movie> listMov, ref List<Transaction> listTrans)
        {
            System.Console.WriteLine("************************************************");
            start:
            string reportChoice = reportWelcome();
                bool tryp;
            tryp = int.TryParse(reportChoice, out int k);
            Console.Clear();
            if(tryp == false)WriteLine("Invalid Choice, Try Again With an Integer",ConsoleColor.White);
            if(k == 1)MovieReport.GetAllMoviesInStock(listMov);
            else if(k == 2)TransactionReport.GetCurrentlyRentedMovies(listTrans);
            else if(k == 3)TransactionReport.GetRentalTotalsByGenre(listTrans);
            else if(k == 4)TransactionReport.GetTop5InTermOfRental(listTrans);
            else if(k == 5)MovieReport.GetAllGenres(listMov);
            else if(k == 6)TransactionReport.GetAllRating(listTrans);
            else if(k == 7)TransactionReport.OverallMovieRatingAverage(listTrans);
            else if(k == 9)
            if(k != 9)
            {
                PressToContinue();
                goto start;
            }
            System.Console.WriteLine("************************************************");
        }

        static string reportWelcome()
        {
            Console.Clear();
            System.Console.WriteLine("Choose What Report You Would Like to Access");
            WriteLine($"\n1. In Stock Movies\n2. Movies Currently Rented\n3. Rentals Total Per Genre\n4. Top 5 Movies in Terms of Rentals\n5. Movie Count by Genre",ConsoleColor.Green);
            WriteLine($"6. List Ratings of All Movies\n7. Average Rating of All Movies\n9. Exit",ConsoleColor.Green);
            return ReadLine();
        }



            static string customerWelcome()
            {
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
            {
                WriteLine("Hello Manager!\nChoose an Option from the Menu Below!\n",ConsoleColor.White);
                WriteLine("1. Add a New Movie to the Inventory",ConsoleColor.Green);
                WriteLine("2. Remove a Movie from the Inventory",ConsoleColor.Green);
                WriteLine("3. Edit Information about a Movie",ConsoleColor.Green);
                WriteLine("4. Access the Report Menu",ConsoleColor.Green);
                WriteLine("9. Exit to Menu",ConsoleColor.Green);
                return ReadLine();
            }

            static string userWelcome()
            {
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

            static void PressToContinue()
            {
                WriteLine("\nPress Any Key to Continue",ConsoleColor.White);
                Console.ReadKey();
            }
        }
        
    }
 
}
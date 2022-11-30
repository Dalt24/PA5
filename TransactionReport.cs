using System.Linq;
using System.IO.Compression;
using System.Transactions;
using System.Xml.Linq;
using System.Security.Cryptography.X509Certificates;
using System;

namespace PA5Test4
{
    public class TransactionReport
    {
        public static void GetRentedMoviesByEmail(List<Transaction> listTrans,string email)
        {
            string newEmail = email; // gets email from ref
            int i = 1;// start at one to print int to choose from
            var test = listTrans
                .Where(x=>x.customerEmail == newEmail)
                .Where(x=>x.transactionStanding == "OutStanding")
                .Select(x=> new Transaction {movieTitle = x.movieTitle, rentalDate = x.rentalDate, returnDate = x.returnDate}).ToList();
            // selecting all transaction that are currently in outstanding and creating temp list to query

            if(!test.Any())
            { // if no transaction
                System.Console.WriteLine("************************************************");
                System.Console.WriteLine("You Currently Have No Movies Rented!");
                System.Console.WriteLine("************************************************");
                return;
            }
            System.Console.WriteLine("\nYou Currently Have the Following Movies Rented");
            System.Console.WriteLine("************************************************");
            foreach(var Transaction in test)
            { // list every movie associated with email provided
                WriteLine($"{i}. Movie: {Transaction.movieTitle} - Rented On: {Transaction.rentalDate}",ConsoleColor.Green);
                i++;
            }
            System.Console.WriteLine("************************************************");
        }

        public static void GetRentalTotalsByGenre(List<Transaction> listTrans)
        {
            Console.Clear();
            System.Console.WriteLine("************************************************");
            var test = listTrans
                .OrderBy(x=>x.movieGenre)
                .Select(x=> new Transaction {movieGenre = x.movieGenre}).DistinctBy(x=>x.movieGenre).ToList();
                //selecting all movies and sorting by genre distinctly
            int i =0;
            foreach(var Transaction in test)
            { // printing genre and the count of the genre
                WriteLine($"Genre: {Transaction.movieGenre} - Times Rented: {listTrans.Where(x=>x.movieGenre == test[i].movieGenre).Count()}",ConsoleColor.Green);
                i++;
            }
            System.Console.WriteLine("************************************************");

        }
        public static void GetTop5InTermOfRental(List<Transaction> listTrans)
        {
            System.Console.WriteLine("************************************************");
            var test = listTrans.GroupBy(x=>x.movieTitle)
                .OrderByDescending(g=>g.Count()) // sort by count lrg -> small
                .SelectMany(g=>g).DistinctBy(g=>g.movieTitle).ToList();
            //selecting movies and copying all info into temp list
            int i = 1;
            foreach(var Transaction in test)
            {//finding count of movies
                int count = listTrans.Where(x=>x.movieTitle == test[i-1].movieTitle).Count();
                WriteLine($"{i}. {Transaction.movieTitle} - {count}",ConsoleColor.Green); // printing movie and how many rentals 
                i++;//inc
                if(i>5)// when the 5th movie is printed, end program
                {
                    System.Console.WriteLine("************************************************");
                    return;
                }
            }
            System.Console.WriteLine("************************************************");
        }

        public static void GetAllRating(List<Transaction> listTrans)
        {
            Console.Clear();
            System.Console.WriteLine("************************************************");
            var test = listTrans.OrderBy(x=>x.movieTitle) 
                .Where(x=>x.movieRating != 0 && x.movieRating <=5)
                .Select(x=> new Transaction {movieRating = x.movieRating, movieTitle = x.movieTitle}).ToList();
            //selecting transactions and storing data in a temp list
            foreach(var Transaction in test)
            {//printing movie and the rating
                WriteLine($"Movie: {Transaction.movieTitle} - Rating: {Transaction.movieRating}",ConsoleColor.Green);
            }
        }


        public static void OverallMovieRatingAverage(List<Transaction> listTrans)
        {
            Console.Clear();
            System.Console.WriteLine("************************************************");
            var test = listTrans.OrderBy(x=>x.movieTitle)
                .Where(x=>x.movieRating != 0 && x.movieRating <=5)
                .Select(x=> new Transaction {movieRating = x.movieRating, movieTitle = x.movieTitle}).ToList();
            //selecting transactions where movierating is a acceptable score
            var query = test.Average(x=>x.movieRating); // finding average of rating
            System.Console.WriteLine($"The Average Rating of All Movies is: {query.ToString("0.00")}");  // printing average
        }
        public static void GetCurrentlyRentedMovies(List<Transaction> listTrans)
        {
            System.Console.WriteLine("************************************************");
            int i = 1; //start at 1 so first print is 1
            var test = listTrans
                .Where(x=>x.transactionStanding == "OutStanding")
                .Select(x=> new Transaction {movieTitle = x.movieTitle, movieGenre = x.movieGenre, customerEmail = x.customerEmail, rentalDate = x.rentalDate, returnDate = x.returnDate}).ToList();
                //selecting transactions that are related to movies that haven't been returned yet
                foreach(var Transaction in test)
                {//printing movies and when it was rented and who it was rented by
                    WriteLine($"{i}. Movie: {Transaction.movieTitle} - Rented On: {Transaction.rentalDate} - Rented By: {Transaction.customerEmail}",ConsoleColor.Green);
                    i++;//inc
                }
            System.Console.WriteLine("************************************************");

        }

        static void WriteLine(string msg, ConsoleColor storeColor) //method to use instead of console.WriteLine(); to Choose colors 
        {
            Console.ForegroundColor = storeColor; 
            Console.WriteLine(msg);
            Console.ResetColor();
        }
    }
}

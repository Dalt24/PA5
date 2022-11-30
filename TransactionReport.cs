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
        public static void GetRentedMoviesByEmail(ref List<Transaction> listTrans,ref string email)
        {
            string newEmail = email;
            int i = 1;
            var test = listTrans
                .Where(x=>x.customerEmail == newEmail)
                .Where(x=>x.transactionStanding == "OutStanding")
                .Select(x=> new Transaction {movieTitle = x.movieTitle, rentalDate = x.rentalDate, returnDate = x.returnDate}).ToList();
            if(!test.Any())
            {
                System.Console.WriteLine("************************************************");
                System.Console.WriteLine("You Currently Have No Movies Rented!");
                System.Console.WriteLine("************************************************");
                return;
            }
            System.Console.WriteLine("\nYou Currently Have the Following Movies Rented");
            System.Console.WriteLine("************************************************");
            foreach(var Transaction in test)
            {
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
            int i =0;
            foreach(var Transaction in test)
            {
                WriteLine($"Genre: {Transaction.movieGenre} - Times Rented: {listTrans.Where(x=>x.movieGenre == test[i].movieGenre).Count()}",ConsoleColor.Green);
                i++;
            }
            System.Console.WriteLine("************************************************");

        }
        public static void GetTop5InTermOfRental(List<Transaction> listTrans)
        {
            System.Console.WriteLine("************************************************");
            var test = listTrans.GroupBy(x=>x.movieTitle)
                .OrderByDescending(g=>g.Count())
                .SelectMany(g=>g).DistinctBy(g=>g.movieTitle).ToList();

            int i = 1;
            foreach(var Transaction in test)
            {
                int count = listTrans.Where(x=>x.movieTitle == test[i-1].movieTitle).Count();
                WriteLine($"{i}. {Transaction.movieTitle} - {count}",ConsoleColor.Green);
                i++;
                if(i>5)
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
            foreach(var Transaction in test)
            {
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
            var query = test.Average(x=>x.movieRating);
            System.Console.WriteLine($"The Average Rating of All Movies is: {query.ToString("0.00")}"); 
        }
        public static void GetCurrentlyRentedMovies(List<Transaction> listTrans)
        {
            System.Console.WriteLine("************************************************");
            int i = 1;
            var test = listTrans
                .Where(x=>x.transactionStanding == "OutStanding")
                .Select(x=> new Transaction {movieTitle = x.movieTitle, movieGenre = x.movieGenre, customerEmail = x.customerEmail, rentalDate = x.rentalDate, returnDate = x.returnDate}).ToList();
                foreach(var Transaction in test)
                {
                    WriteLine($"{i}. Movie: {Transaction.movieTitle} - Rented On: {Transaction.rentalDate} - Rented By: {Transaction.customerEmail}",ConsoleColor.Green);
                    i++;
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

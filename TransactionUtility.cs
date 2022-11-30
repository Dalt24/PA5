using System.Transactions;
using System;

namespace PA5Test4
{
    public class TransactionUtility
    {
        public static (List<Movie>, List<Transaction>) ReturnAMovie(List<Movie> listMov, List<Transaction> listTrans)
        {
            System.Console.WriteLine("What is the Email Associated with Your Account?"); // getting email from user
            string returnUserEmail = ReadLine();
            start:
            var c = new List<Transaction>();//temp list
            var d = new List<Movie>();//temp list
            var test = listTrans
                .Where(x=>x.customerEmail == returnUserEmail)
                .Where(x=>x.transactionStanding == "OutStanding")
                .Select(x=> new Transaction {transactionID = x.transactionID, movieID = x.movieID, movieTitle = x.movieTitle,movieGenre = x.movieGenre ,rentalDate = x.rentalDate, returnDate = x.returnDate, transactionStanding = x.transactionStanding, movieRating = x.movieRating}).ToList();
            //selecting transaction data that is related to movies not returned yet and related to user email and storing in temp list
            if(test.Count == 0)
            {//no movies to return
                System.Console.WriteLine("\nThis Email Doesn't Have any Oustanding Movies Right Now!");
                return(listMov, listTrans);
            }

            TransactionReport.GetRentedMoviesByEmail(listTrans, returnUserEmail);//print movies associated with email
            c = test;// copying into another temp lsit

            System.Console.WriteLine("\nWhich Number Movie Would you Like to Return?");
                bool tryp; // asking which movie to return
            tryp = int.TryParse(ReadLine(), out int k);
                System.Console.WriteLine("How Would you Rate this Movie on a Scale of 1 to 5");
            WriteLine("1.\n2.\n3.\n4.\n5.",ConsoleColor.Green); // getting user rating
            System.Console.WriteLine("Type an Integer or Your Ratings may not be Considered for Future Listings!");
                bool tryR; // if not 1-5 it'll just ignore data as it isn't that important
            tryR = int.TryParse(ReadLine(), out int a);
            // dcwright4@crimson.ua.edu
                try{
                    var dateNow = DateOnly.FromDateTime(DateTime.Now);
                    c[k-1].transactionStanding = "Returned";c[k-1].returnDate = dateNow;c[k-1].customerEmail = returnUserEmail;c[k-1].movieRating = a;
                    listTrans.RemoveAt(listTrans.FindIndex(a => a.transactionID == c[k-1].transactionID));
                    listTrans.Add(c[k-1]);
                }catch{
                    System.Console.WriteLine("Your Selection was Invalid, Sorry!");
                    return(listMov,listTrans);
                }// adding new information to a list


            var test2 = listMov
                .Where(x=>x.movieID == c[k-1].movieID)
                .Distinct().Select(x=> new Movie {movieID = x.movieID,movieTitle = x.movieTitle, movieGenre = x.movieGenre, movieInStock = x.movieInStock}).ToList();
            //selected movie list to u pdate now that transaction is done
                d = test2;
                try{
                    d[0].movieInStock = "trueInStock";

                    listMov.RemoveAt(listMov.FindIndex(a => a.movieID == c[k-1].movieID));
                    listMov.Add(d[0]);
                }catch{
                    System.Console.WriteLine("The Movie Failed to Return, Try Again!");
                }//updating movie date

                UpdateTransactionList(ref listTrans); // updating file data
                UpdateMovieList(ref listMov); //updating file data

                if(ContinueRunning("Return") == "YES")goto start;
            return(listMov, listTrans);
        }

        public static void RentAMovie(List<Movie> listMov, List<Transaction> listTrans)
        {
            System.Console.WriteLine("************************************************");
            System.Console.WriteLine("What is the Email Associated with Your Account?"); //email to rent to
            string renterEmail = ReadLine(); // getting user email
            start:
            //getting # of outstanding movies user has
            int count = listTrans.Where(x=>x.transactionStanding=="OutStanding").Where(x=>x.customerEmail == renterEmail).Count(); // getting count of specific movie
            if(count>=3)
            {//if more than 3 outstanding movies, they can't rent more
                System.Console.WriteLine("You have 3 OutStanding Movies, Return One Before Renting Another!");
                goto end;
            }
            MovieReport.GetAllMoviesInStock(listMov); // printing in stock
            System.Console.WriteLine("\nType in the Number Or the Title of the Movie you Would Like to Rent");
            string movieRentChoice = ReadLine(); // user choice

            string[] arr = new string[10000];//temp arr
            int[] arrInt = new int[10000];//temp arr

            bool tryp;
            tryp = int.TryParse(movieRentChoice, out int key);
            if(key<=0 && tryp == true) //if key is invalid, movie isn't available
            {
                Console.Clear();
                System.Console.WriteLine("\nThat Movie Isn't Available!\n");
                if(ContinueRunning("Rent") == "YES") goto start;
                else goto end;
            }
            if(tryp == true) // if key is a valid int
            {
                RentMovieByInt(arrInt, arr, listMov); // get int + Rent
                int akula = BinarySearch(arrInt,key); //find position of int
                movieRentChoice = arr[key]; // use int to find associated title
            }
            if(listMov.Any(x=>x.movieTitle == movieRentChoice)==true || listMov.Any(x=>x.movieTitle == movieRentChoice)==false)
            {
                if(listMov.Where(x=>x.movieTitle== movieRentChoice).All(x=>x.movieInStock == "falseOutStock")==true || listMov.Any(x=>x.movieTitle == movieRentChoice)==false)
                { // if movie doesn't exist
                    System.Console.WriteLine("\nThat Movie Isn't Available!\n");
                    if(ContinueRunning("Rent") == "YES") goto start;
                    else goto end;
                }
            }

            AddRentedMovieToList(listMov, listTrans, movieRentChoice, renterEmail);
            //Add Movie to List
            if(ContinueRunning("Rent") == "YES")
            {//if continue, call again and update list info
                UpdateMovieList(ref listMov);
                UpdateTransactionList(ref listTrans);
                goto start;
            }
            end:
            System.Console.WriteLine("************************************************");
        }

        private static void AddRentedMovieToList(List<Movie> listMov, List<Transaction> listTrans, string movieRentChoice, string renterEmail)
        {
            var test = listMov 
                .Where(x=>x.movieTitle == movieRentChoice)
                .Where(x=>x.movieInStock == "trueInStock")
                .Distinct().Select(x=> new Movie {movieID = x.movieID, movieTitle = x.movieTitle, movieGenre = x.movieGenre, movieInStock = x.movieInStock}).ToList();
            //select all data where movie is in stock and it's the users choice
            Transaction c = new Transaction {transactionID = Guid.NewGuid(), customerEmail = renterEmail, movieID = test[0].movieID, movieTitle = test[0].movieTitle, movieGenre = test[0].movieGenre, rentalDate = DateOnly.FromDateTime(DateTime.Today), returnDate = DateOnly.FromDateTime(DateTime.MaxValue),transactionStanding = "OutStanding"};
            // ^^ making new Transaction to store the renters information / data // 
            test[0].movieInStock = "falseOutStock"; // changing rented movie to out of stock in file

            listMov.RemoveAt(listMov.FindIndex(a => a.movieID == c.movieID)); // removing old movie information
            listMov.Add(test[0]); // adding new movie information
            UpdateMovieList(ref listMov); // updating list
            listTrans.Add(c); // adding temp data to main list
            UpdateTransactionList(ref listTrans);//updating data
            System.Console.WriteLine($"\nCongrats! You've rented {test[0].movieTitle}, You Have 7 Days to Return it.....Or Else ;)");
        }
        private static (int[], string[])RentMovieByInt(int[] arrInt, string[] arr, List<Movie> listMov)
        {//if user inputs integer
            int i = 1;
            var test2 = listMov.Select(t => new {movieTitle = t.movieTitle, movieGenre = t.movieGenre, movieInStock = t.movieInStock})
                .Where(x=>x.movieInStock == "trueInStock")
                .Distinct().Select(x=> new Movie {movieTitle = x.movieTitle, movieGenre = x.movieGenre}).ToList();
                //selecting instock movies 
            foreach(var Movie in test2)
            {//adding movies to array to get index positions to serach
                arr[i] = Movie.movieTitle;
                arrInt[i] = i; 
                i++;
            }
            return(arrInt,arr);
        }

        private static int BinarySearch(int[] arr, int item) // binary search implementation for ints to find position in an array
        {
            int min = 0;
            int max = arr.Length - 1;
            while(min <= max) // while not found
            {
                var mid = (min+max)/2; // start in center
                if(arr[mid] == item)return mid; // if correct, return
                if (item < arr[mid])max = mid - 1; // if lower, decrement
                else min = mid + 1; // if higher, increment position
            }
            return -1; // fail 
        }
        public static List<Transaction> LoadListTrans(List<Transaction> listTrans)
        {
            var fileLines = File.ReadAllLines("transactions.txt").ToList();
            List<Transaction> lst = new List<Transaction>();// making temp list to return (which will be added to main list)
            foreach (string line in fileLines)
            {// for every line Delimit it with # and add it to a temp Object which will be stored in a List
                string[] temp = line.Split('#');
                Transaction c = new Transaction{transactionID = Guid.Parse(temp[0]), customerEmail = temp[1], movieID = Guid.Parse(temp[2]),movieTitle = temp[3], 
                movieGenre = temp[4], rentalDate = DateOnly.Parse(temp[5]), returnDate = DateOnly.Parse(temp[6]),transactionStanding = temp[7],movieRating = decimal.Parse(temp[8])};
                lst.Add(c); // adding object to list
            }
            return lst;// returning list 
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
        public static List<Transaction> UpdateTransactionList(ref List<Transaction> listTrans)
        {
            using(TextWriter sw = new StreamWriter("transactions.txt"))
            {
                foreach(Transaction c in listTrans)
                sw.WriteLine($"{c.transactionID}#{c.customerEmail}#{c.movieID}#{c.movieTitle}#{c.movieGenre}#{c.rentalDate}#{c.returnDate}#{c.transactionStanding}#{c.movieRating}");}
            return listTrans;
        }
        public static List<Movie> UpdateMovieList(ref List<Movie> listMov)// writing list to text file for storage
        {
            using(TextWriter tw = new StreamWriter("movieinventory.txt"))
            {
                foreach(Movie s in listMov)
                tw.WriteLine($"{s.movieID}#{s.movieTitle}#{s.movieGenre}#{s.movieInStock}");
            }
            return listMov;
        }

        static string ContinueRunning(string choice) // used to ask user if they want to run "rent" "edit" etc again
        {
            System.Console.WriteLine($"Would you Like to {choice} another option?");
            WriteLine("1. Yes\n2. No",ConsoleColor.Green);
            string userContinue = ReadLine().ToUpper();
            if(userContinue == "1" || userContinue == "1." || userContinue == "YES")
            {
                return "YES";
            }
            else return "NO";
        }

    }
}

using System.Linq;
using System.Globalization;
using System.Buffers;
using System;

namespace PA5Test4
{
    public class MovieUtility
    {
        public static List<Movie> LoadListMov(List<Movie> listMov)
        {
            var fileLines = File.ReadAllLines("movieinventory.txt").ToList();
            List<Movie> lst = new List<Movie>(); // making temp list to return (which will be added to main list)
            foreach (string line in fileLines)
            { // for every line Delimit it with # and add it to a temp Object which will be stored in a List
                string[] temp = line.Split('#');
                Movie c = new Movie{movieID = Guid.Parse(temp[0]),movieTitle = temp[1], movieGenre = temp[2],movieInStock = temp[3]};
                lst.Add(c); // adding object to list
            }
            return lst; // returning list 
        }

        public static List<Movie> AddMovieToStock(List<Movie> listMov)
        {
            System.Console.WriteLine("************************************************");
            System.Console.WriteLine("Type in the Movie Title you Would Like to Add");
            string newMovieTitle = ReadLine(); // getting title
            System.Console.WriteLine($"Type in the Genre of {newMovieTitle}");
            string newMovieGenre = ReadLine(); // getting genre 
            // making temp list to return (which will be added to main list)
            Movie c = new Movie{movieID = Guid.NewGuid(), movieTitle = newMovieTitle, movieGenre = newMovieGenre, movieInStock = "trueInStock"};
            using (StreamWriter sw = File.AppendText("movieinventory.txt"))
            {//Appending new Movie to text file
                sw.WriteLine($"{c.movieID}#{c.movieTitle}#{c.movieGenre}#{c.movieInStock}");
            }	
            listMov.Add(c); // adding new Movie to main List
            System.Console.WriteLine($"You've Successfully added one copy of {newMovieTitle}!");
            System.Console.WriteLine("************************************************");
            return listMov;
        }

        public static List<Movie> RemoveMovieFromStock(List<Movie> listMov)
        {
            start:
            System.Console.WriteLine("************************************************");
            System.Console.WriteLine("Note! Deleting a Movie Means Users can no Longer Rent/Return that Movie\nAny Previous Transactions can still be viewed however the User can't interact with the Movie at all!");
            MovieReport.GetAllMovies(listMov); // print all movies
            Console.WriteLine("Type How many Copies of the Movie Would you like to Delete?");
            WriteLine("1. One Copy\n2. All Copies",ConsoleColor.Green); // asking manager how many to delete
            string removeMovChoice = ReadLine();
            System.Console.WriteLine("Type in the Movie Number or Movie Title you Would Like to Remove\n");
            string removeMovieTitle = ReadLine(); //choose which movie to delete
            bool tryp; // bool for catching whether it's an int or string
            string[] arr = new string[10000]; // array for binary
            int[] arrInt = new int[10000]; // array for binary
            tryp = int.TryParse(removeMovieTitle, out int key); // trying to parse string into int, if it's an int continue
            if(key<=0 && tryp == true)
            {
                Console.Clear();
                System.Console.WriteLine("\nThat Movie Isn't Available!\n");
                if(ContinueRunning("Rent") == "YES")goto start;
                else goto end; // if the selection is an invalid key it isn't available
            }
            if(tryp == true)
            { // if selection is an int, remove by int method and search array position to delete it
                RemoveMovieByInt(arrInt, arr, listMov);
                int akula = BinarySearch(arrInt,key); 
                removeMovieTitle = arr[key];
            }

            var setToRemove = listMov.Where(t => t.movieTitle == removeMovieTitle).Distinct().ToList();
            if(removeMovChoice == "1")
            {
                try{listMov.RemoveAt(listMov.FindIndex(a => a.movieTitle == removeMovieTitle));} // removing movie in previously determined position
                catch{System.Console.WriteLine("Failed to Remove Movie! Try again");goto start;}; // failsafe
                goto end;
            }
            else foreach(Movie s in setToRemove)listMov.RemoveAt(listMov.FindIndex(a => a.movieTitle == removeMovieTitle)); // remove it with string method
            WriteLine($"\nThe Movie {removeMovieTitle} has been removed\n",ConsoleColor.White);
            UpdateMovieList(listMov);//updating list since a movie is removed
            System.Console.WriteLine("************************************************");
            if(ContinueRunning("Remove") == "YES")goto start; // if user wants to remove another movie, restart method
            end:
            return listMov;
        }
        private static(int[], string[])RemoveMovieByInt(int[] arrInt, string[] arr, List<Movie> listMov)
        { // called when user uses number to delete movie
            int i = 1;
            var test = listMov.GroupBy(x=>x.movieTitle)
                .OrderByDescending(g=>g.Count())
                .SelectMany(g=>g).DistinctBy(g=>g.movieTitle).ToList();
            //selecting all movies and adding them into temp list
            foreach(var Movie in test)
            {
                arr[i] = Movie.movieTitle;
                arrInt[i] = i; 
                i++;
            } // storing movietitle and it's position into arrays, was using dictionary key pair but I needed to meet requirements :(
            return(arrInt,arr);
        }
        public static List<Movie> EditMovieFromStock(List<Movie> listMov)
        {
            System.Console.WriteLine("************************************************");
            MovieReport.GetAllMovies(listMov);//print movies
            System.Console.WriteLine("\nType in the Title of the Movie you Would Like to Edit");


            string editMovTitleChoice = ReadLine();
            
            if(listMov.Any(x=>x.movieTitle == editMovTitleChoice)==false)return listMov;
            
            int i=1;
            var test = listMov
                .Where(x=>x.movieTitle == editMovTitleChoice)
                .Distinct().Select(x=> new Movie {movieID = x.movieID,movieTitle = x.movieTitle, movieGenre = x.movieGenre, movieInStock = x.movieInStock}).ToList();
            //selecting all movies with the users choice and adding them into temp list

            System.Console.WriteLine("\nType Which number you Would Like to Change");
            foreach(var Movie in test)
            { // prints options to edit from
                WriteLine($"{i}. {Movie.movieTitle} - {Movie.movieGenre} - {Movie.movieInStock}",ConsoleColor.Green); 
                i++;
            }
            bool tryp;
            tryp = int.TryParse(ReadLine(), out int j);
            if(tryp == true && j < i)
            { // 
                Guid movIDtoChange = test[j-1].movieID; // storing guid to change info of


                WriteLine("Choose Which You Would Like to Change\n1. Movie Title\n2. Movie Genre\n3. Movie In Stock or Not",ConsoleColor.Green);
                bool trypChange;
                trypChange = int.TryParse(ReadLine(), out int k); // if multiple titles of same movie exist, choose which to change
                if(k == 1)
                {
                    System.Console.WriteLine("What Would you like to Change the Title to?");
                    test[j-1].movieTitle = ReadLine(); // new title
                }
                else if(k == 2)
                {
                    System.Console.WriteLine("What Would you Like to Change the Genre to?");
                    test[j-1].movieGenre = ReadLine(); // new genre
                }
                else if(k == 3)
                {
                    if(test[j-1].movieInStock == "trueInStock") // msg for changing stock indicator
                    {
                        test[j-1].movieInStock = "falseOutStock";
                        System.Console.WriteLine($"The Movie is now Out of Stock!");
                    }
                    else
                    {
                        test[j-1].movieInStock = "trueInStock"; // msg for changing stock indicator
                        System.Console.WriteLine("The Movie is now In Stock!");
                    }
                }
                listMov.RemoveAt(listMov.FindIndex(a => a.movieID == movIDtoChange)); // deleting old info in List
                listMov.Add(test[j-1]); // adding new info to list
                UpdateMovieList(listMov); // updating main list
            }
            else System.Console.WriteLine("Your Choice was Invalid\n"); // invalid choice, repeat menu
            System.Console.WriteLine("************************************************");
            return listMov;
        }

        public static List<Movie> UpdateMovieList(List<Movie> listMov)
        {
            using(TextWriter tw = new StreamWriter("movieinventory.txt")) // writing list to text file for storage
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
                return "YES"; // if yes, return yes to show they want to redo it
            }
            else return "NO";
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
        static void PressToContinue() // method to keep screen from moving on until button pressed
        {
            WriteLine("\nPress Any Key to Continue",ConsoleColor.White);
            Console.ReadKey();
        }
    }
}

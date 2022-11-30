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
            List<Movie> lst = new List<Movie>();
            foreach (string line in fileLines)
            {
                string[] temp = line.Split('#');
                Movie c = new Movie{movieID = Guid.Parse(temp[0]),movieTitle = temp[1], movieGenre = temp[2],movieInStock = temp[3]};
                lst.Add(c);
            }
            return lst;
        }

        public static List<Movie> AddMovieToStock(ref List<Movie> listMov)
        {
            System.Console.WriteLine("************************************************");
            System.Console.WriteLine("Type in the Movie Title you Would Like to Add");
            string newMovieTitle = ReadLine();
            System.Console.WriteLine($"Type in the Genre of {newMovieTitle}");
            string newMovieGenre = ReadLine();
            Movie c = new Movie{movieID = Guid.NewGuid(), movieTitle = newMovieTitle, movieGenre = newMovieGenre, movieInStock = "trueInStock"};
            using (StreamWriter sw = File.AppendText("movieinventory.txt"))
            {
                sw.WriteLine($"{c.movieID}#{c.movieTitle}#{c.movieGenre}#{c.movieInStock}");
            }	
            listMov.Add(c);
            System.Console.WriteLine($"You've Successfully added one copy of {newMovieTitle}!");
            System.Console.WriteLine("************************************************");
            return listMov;
        }

        public static List<Movie> RemoveMovieFromStock(List<Movie> listMov)
        {
            start:
            System.Console.WriteLine("************************************************");
            System.Console.WriteLine("Note! Deleting a Movie Means Users can no Longer Rent/Return that Movie\nAny Previous Transactions can still be viewed however the User can't interact with the Movie at all!");
            MovieReport.GetAllMovies(listMov);
            Console.WriteLine("Type How many Copies of the Movie Would you like to Delete?");
            WriteLine("1. One Copy\n2. All Copies",ConsoleColor.Green);
            string removeMovChoice = ReadLine();
            System.Console.WriteLine("Type in the Movie Number or Movie Title you Would Like to Remove\n");
            string removeMovieTitle = ReadLine();
            bool tryp;
            string[] arr = new string[10000];
            int[] arrInt = new int[10000];
            tryp = int.TryParse(removeMovieTitle, out int key);
            if(key<=0 && tryp == true)
            {
                Console.Clear();
                System.Console.WriteLine("\nThat Movie Isn't Available!\n");
                if(ContinueRunning("Rent") == "YES") goto start;
                else goto end;
            }
            if(tryp == true)
            {
                RemoveMovieByInt(arrInt, arr, listMov);
                int akula = BinarySearch(arrInt,key); 
                removeMovieTitle = arr[key];
            }

            var setToRemove = listMov.Where(t => t.movieTitle == removeMovieTitle).Distinct().ToList();
            if(removeMovChoice == "1")
            {
                try{listMov.RemoveAt(listMov.FindIndex(a => a.movieTitle == removeMovieTitle));}
                catch{System.Console.WriteLine("Failed to Remove Movie! Try again");goto start;};
                goto end;
            }
            else foreach(Movie s in setToRemove)listMov.RemoveAt(listMov.FindIndex(a => a.movieTitle == removeMovieTitle));
            WriteLine($"\nThe Movie {removeMovieTitle} has been removed\n",ConsoleColor.White);
            UpdateMovieList(ref listMov);
            System.Console.WriteLine("************************************************");
            if(ContinueRunning("Remove") == "YES")goto start;
            end:
            return listMov;
        }
        private static(int[], string[])RemoveMovieByInt(int[] arrInt, string[] arr, List<Movie> listMov)
        {
            int i = 1;
            var test = listMov.GroupBy(x=>x.movieTitle)
                .OrderByDescending(g=>g.Count())
                .SelectMany(g=>g).DistinctBy(g=>g.movieTitle).ToList();
            
            foreach(var Movie in test)
            {
                arr[i] = Movie.movieTitle;
                arrInt[i] = i; 
                i++;
            }
            return(arrInt,arr);
        }
        public static List<Movie> EditMovieFromStock(List<Movie> listMov)
        {
            System.Console.WriteLine("************************************************");
            System.Console.WriteLine("Type in Which Movie Would you Like to Edit?");
            string editMovTitleChoice = ReadLine();
            if(listMov.Any(x=>x.movieTitle == editMovTitleChoice)==false)return listMov;
            
            int i=1;
            var test = listMov
                .Where(x=>x.movieTitle == editMovTitleChoice)
                .Distinct().Select(x=> new Movie {movieID = x.movieID,movieTitle = x.movieTitle, movieGenre = x.movieGenre, movieInStock = x.movieInStock}).ToList();

            System.Console.WriteLine("\nType Which number you Would Like to Change");
            foreach(var Movie in test)
            {
                WriteLine($"{i}. {Movie.movieTitle} - {Movie.movieGenre} - {Movie.movieInStock}",ConsoleColor.Green); 
                i++;
            }
            bool tryp;
            tryp = int.TryParse(ReadLine(), out int j);
            if(tryp == true && j < i)
            {
                Guid movIDtoChange = test[j-1].movieID;
                MovieReport.GetAllMovies(listMov);
                System.Console.WriteLine("\nType in the Title of the Movie you Would Like to Rent");

                WriteLine("Choose Which You Would Like to Change\n1. Movie Title\n2. Movie Genre\n3. Movie In Stock or Not",ConsoleColor.Green);
                bool trypChange;
                trypChange = int.TryParse(ReadLine(), out int k);
                if(k == 1)
                {
                    System.Console.WriteLine("What Would you like to Change the Title to?");
                    test[j-1].movieTitle = ReadLine();
                }
                else if(k == 2)
                {
                    System.Console.WriteLine("What Would you Like to Change the Genre to?");
                    test[j-1].movieGenre = ReadLine();
                }
                else if(k == 3)
                {
                    if(test[j-1].movieInStock == "trueInStock")
                    {
                        test[j-1].movieInStock = "falseOutStock";
                        System.Console.WriteLine($"The Movie is now Out of Stock!");
                    }
                    else
                    {
                        test[j-1].movieInStock = "trueInStock";
                        System.Console.WriteLine("The Movie is now In Stock!");
                    }
                }
                listMov.RemoveAt(listMov.FindIndex(a => a.movieID == movIDtoChange));
                listMov.Add(test[j-1]);
                UpdateMovieList(ref listMov);
            }
            else System.Console.WriteLine("Your Choice was Invalid\n");
            System.Console.WriteLine("************************************************");
            return listMov;
        }

        public static List<Movie> UpdateMovieList(ref List<Movie> listMov)
        {
            using(TextWriter tw = new StreamWriter("movieinventory.txt"))
            {
                foreach(Movie s in listMov)
                tw.WriteLine($"{s.movieID}#{s.movieTitle}#{s.movieGenre}#{s.movieInStock}");
            }
            return listMov;
        }

        static string ContinueRunning(string choice)
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
        private static int BinarySearch(int[] arr, int item)
        {
            int min = 0;
            int max = arr.Length - 1;
            while(min <= max)
            {
                var mid = (min+max)/2;
                if(arr[mid] == item)return mid;
                if (item < arr[mid])max = mid - 1;
                else min = mid + 1;
            }
            return -1;
        }
        static void PressToContinue()
        {
            WriteLine("\nPress Any Key to Continue",ConsoleColor.White);
            Console.ReadKey();
        }
    }
}

using System.Security.Cryptography.X509Certificates;
using System;

namespace PA5Test4
{
    public class MovieReport
    {
        public static void GetAllMoviesInStock(List<Movie> listMov) // getMovInStock
        {
            int i = 1; // start at one to print int to choose from
            Console.Clear();
            System.Console.WriteLine("************************************************");
            var test = listMov.Select(t => new {movieTitle = t.movieTitle, movieGenre = t.movieGenre, movieInStock = t.movieInStock})
                .Where(x=>x.movieInStock == "trueInStock")
                .Distinct().Select(x=> new Movie {movieTitle = x.movieTitle, movieGenre = x.movieGenre}).ToList(); 
        // selecting movies that are currently in stock and removing duplicates from view

            foreach(var Movie in test) // for every movie thats in stock print and increment
            {
                WriteLine($"{i}. {Movie.movieTitle} - {Movie.movieGenre}",ConsoleColor.Green); 
                i++;
            }  
            System.Console.WriteLine("************************************************");
        }

        public static void GetAllMovies(List<Movie> listMov)
        { // movies in stock & out of stock
            int i = 1; 
            Console.Clear();
            System.Console.WriteLine("************************************************");
            var test = listMov.GroupBy(x=>x.movieTitle)
                .OrderByDescending(g=>g.Count())
                .SelectMany(g=>g).DistinctBy(g=>g.movieTitle).ToList(); // selecting every movie with a title (all of them) and converting to list 
            
            foreach(var Movie in test)
            { // cycling through every movie that exist and printing # of copies
                int count = listMov.Where(x=>x.movieTitle == test[i-1].movieTitle).Count(); // getting count of specific movie
                if(count == 1)WriteLine($"{i}. {Movie.movieTitle} - {count} Copy",ConsoleColor.Green); // printing movie + count
                else if(count !=1)WriteLine($"{i}. {Movie.movieTitle} - {count} Copies",ConsoleColor.Green); 
                i++;
            }
            System.Console.WriteLine("************************************************");
        }

        public static void GetAllGenres(List<Movie> listMov) //getGenres
        {
            Console.Clear();
            System.Console.WriteLine("************************************************");
            int i = 0;
            var test = listMov
                .OrderBy(x=>x.movieGenre)
                .Select(x=> new Movie {movieGenre = x.movieGenre}).DistinctBy(x=>x.movieGenre).ToList();

                //selecting movies and storing Genre in a List
            foreach(var Movie in test)
            {// inc through list and printing Genre + Count of genre
                WriteLine($"Genre: {Movie.movieGenre} - Movie Count: {listMov.Where(x=>x.movieGenre == test[i].movieGenre).Count()}",ConsoleColor.Green);
                i++;
            }
            System.Console.WriteLine("************************************************");
        }



        public static void GetAllMoviesOutStock(List<Movie> listMov)
        {
            int i = 1;
            System.Console.WriteLine("************************************************");
            var test = listMov.Select(t => new {movieTitle = t.movieTitle, movieGenre = t.movieGenre, movieInStock = t.movieInStock})
                .Where(x=>x.movieInStock == "falseOutStock")
                .Distinct().Select(x=> new Movie {movieTitle = x.movieTitle, movieGenre = x.movieGenre}).ToList();
            //selecting movies currently out of stock and putting them in a List
        
            foreach(var Movie in test)
            {//cycling through movies and printing them with #
                WriteLine($"{i}. {Movie.movieTitle} - {Movie.movieGenre}",ConsoleColor.Green);
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

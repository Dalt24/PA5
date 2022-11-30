using System.Security.Cryptography.X509Certificates;
using System;

namespace PA5Test4
{
    public class MovieReport
    {
        public static void GetAllMoviesInStock(List<Movie> listMov)
        {
            int i = 1;
            Console.Clear();
            System.Console.WriteLine("************************************************");
            var test = listMov.Select(t => new {movieTitle = t.movieTitle, movieGenre = t.movieGenre, movieInStock = t.movieInStock})
                .Where(x=>x.movieInStock == "trueInStock")
                .Distinct().Select(x=> new Movie {movieTitle = x.movieTitle, movieGenre = x.movieGenre}).ToList();
            foreach(var Movie in test)
            {
                WriteLine($"{i}. {Movie.movieTitle} - {Movie.movieGenre}",ConsoleColor.Green); 
                i++;
            }  
            System.Console.WriteLine("************************************************");
        }

        public static void GetAllMovies(List<Movie> listMov)
        {
            int i = 1;
            Console.Clear();
            System.Console.WriteLine("************************************************");
            var test = listMov.GroupBy(x=>x.movieTitle)
                .OrderByDescending(g=>g.Count())
                .SelectMany(g=>g).DistinctBy(g=>g.movieTitle).ToList();
            
            foreach(var Movie in test)
            {
                int count = listMov.Where(x=>x.movieTitle == test[i-1].movieTitle).Count();
                if(count == 1)WriteLine($"{i}. {Movie.movieTitle} - {count} Copy",ConsoleColor.Green);
                else if(count !=1)WriteLine($"{i}. {Movie.movieTitle} - {count} Copies",ConsoleColor.Green);
                i++;
            }
            System.Console.WriteLine("************************************************");
        }

        public static void GetAllGenres(List<Movie> listMov)
        {
            Console.Clear();
            System.Console.WriteLine("************************************************");
            int i = 0;
            var test = listMov
                .OrderBy(x=>x.movieGenre)
                .Select(x=> new Movie {movieGenre = x.movieGenre}).DistinctBy(x=>x.movieGenre).ToList();
            foreach(var Movie in test)
            {
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
            foreach(var Movie in test)
            {
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

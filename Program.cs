using System;
using System.Collections.Generic;


// Delegate
public delegate void MovieAlert(string message);

public class Movie
{
    private static int counter = 1;
    public int Id { get; }
    public string Title { get; set; }
    public int Year { get; set; }
    public string Language { get; set; }
    public Movie(string title, int year, string language)
    {
        Id = counter++;   //Unique Movie ID
        Title = title;
        Year = year;
        Language = language;
    }
}


public class MovieStore : Dictionary<int, Movie>
{
    public string StoreLanguage { get; set; }
    public MovieAlert Notify;
    public void AddMovie(Movie movie)
    {
        Add(movie.Id, movie);   
        Notify?.Invoke($"'{movie.Title}' added to {StoreLanguage} collection.");
    }
    public void RemoveMovie(int movieId)
    {
        if (ContainsKey(movieId))
        {
            string name = this[movieId].Title;
            Remove(movieId);   
            Notify?.Invoke($"'{name}' removed from {StoreLanguage} collection.");
        }
        else
        {
            Notify?.Invoke($"Movie ID {movieId} not found in {StoreLanguage} collection.");
        }
    }
    public void ShowCount()
    {
        Notify?.Invoke($"Total movies in {StoreLanguage}: {Count}");
    }
}
public class Program
{
    static void Main()
    {
        // Language wise movie collections
        MovieStore hindiStore = new MovieStore { StoreLanguage = "Hindi" };
        MovieStore teluguStore = new MovieStore { StoreLanguage = "Telugu" };
        MovieStore englishStore = new MovieStore { StoreLanguage = "English" };
        
        // Lambda for delegate
        MovieAlert alertHandler = msg =>
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(msg);
            Console.ResetColor();
        };
        hindiStore.Notify += alertHandler;
        teluguStore.Notify += alertHandler;
        englishStore.Notify += alertHandler;
        
        
        // Hard coded movies 
        hindiStore.AddMovie(new Movie("Welcome", 2007, "Hindi"));
        teluguStore.AddMovie(new Movie("Magadheera", 2009, "Telugu"));
        englishStore.AddMovie(new Movie("Home Alone", 1990, "English"));
        while (true)
        {
            Console.WriteLine("\n MOVIE MANAGEMENT SYSTEM ");
            Console.WriteLine("1. Add Movie");
            Console.WriteLine("2. Remove Movie");
            Console.WriteLine("3. Get Movie Count");
            Console.WriteLine("4. Exit");
            Console.Write("Choose option: ");
            int choice = int.Parse(Console.ReadLine());
            if (choice == 4)
                break;  
            Console.WriteLine("\nSelect Language:");
            Console.WriteLine("1. Hindi");
            Console.WriteLine("2. Telugu");
            Console.WriteLine("3. English");
            Console.Write("Choose language: ");
            int langChoice = int.Parse(Console.ReadLine());
            MovieStore selectedStore = langChoice switch
            {
                1 => hindiStore,
                2 => teluguStore,
                3 => englishStore,
                _ => null
            };
            if (selectedStore == null)
            {
                Console.WriteLine("Invalid language choice.");
                continue;   
            }
            switch (choice)
            {
                case 1:
                    Console.Write("Enter movie title: ");
                    string title = Console.ReadLine();
                    Console.Write("Enter release year: ");
                    int year = int.Parse(Console.ReadLine());
                    selectedStore.AddMovie(
                        new Movie(title, year, selectedStore.StoreLanguage)
                    );
                    break;
                case 2:
                    Console.Write("Enter Movie ID to remove: ");
                    int id = int.Parse(Console.ReadLine());
                    selectedStore.RemoveMovie(id);
                    break;
                case 3:
                    selectedStore.ShowCount();   
                    break;
                default:
                    Console.WriteLine("Invalid option.");
                    break;
            }
        }
    }
}
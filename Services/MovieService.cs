using MongoDB.Driver;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using MovieCatalog.Models;

public class MovieService
{
    private readonly IMongoCollection<Movie> _movies;

    public MovieService(IOptions<MongoSettings> settings)
    {
        var client = new MongoClient(settings.Value.ConnectionString);
        var database = client.GetDatabase(settings.Value.DatabaseName);
        _movies = database.GetCollection<Movie>(settings.Value.MovieCollectionName);
    }


    public List<Movie> GetByUser(string userId) =>
        _movies.Find(m => m.UserId == userId).ToList();

    //public List<Movie> Get() => _movies.Find(movie => true).ToList();

    public Movie Get(string id) => _movies.Find(m => m.Id == id).FirstOrDefault();

    public Movie Create(Movie movie)
    {
        _movies.InsertOne(movie);
        return movie;
    }

    public void Update(string id, Movie movieIn) =>
        _movies.ReplaceOne(movie => movie.Id == id, movieIn);

    public void Remove(string id) =>
        _movies.DeleteOne(movie => movie.Id == id);
}

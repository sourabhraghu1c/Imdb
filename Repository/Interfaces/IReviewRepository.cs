using IMDBSample.Models.Db;
using System.Collections.Generic;

public interface IReviewRepository
{
    IEnumerable<Review> GetAll(int movieId);

    Review GetById(int movieId, int id);

    int Add(Review review);

    bool Update(int movieId, int id, Review review);

    bool Delete(int movieId, int id);
}
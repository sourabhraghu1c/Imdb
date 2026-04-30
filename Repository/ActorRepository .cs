using IMDBSample;
using IMDBSample.Models.Db;
using IMDBSample.Repository;
using IMDBSample.Repository.Interfaces;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;

public class ActorRepository : BaseRepository, IActorRepository
{
    public ActorRepository(IOptions<ConnectionString> options) : base(options)
    {
    }

    public IEnumerable<Actor> GetAll()
    {
        var sql = @"SELECT Id, Name, Bio, Dob AS DOB, Gender FROM Actors";
        return Query<Actor>(sql);
    }

    public Actor GetById(int id)
    {
        var sql = @"SELECT Id, Name, Bio, Dob AS DOB, Gender 
                    FROM Actors WHERE Id = @Id";

        return QuerySingle<Actor>(sql, new { Id = id });
    }

    public int Add(Actor actor)
    {
        var sql = @"
            INSERT INTO Actors (Name, Bio, Dob, Gender)
            VALUES (@Name, @Bio, @DOB, @Gender);

            SELECT CAST(SCOPE_IDENTITY() AS INT)";

        return ExecuteScalar<int>(sql, actor);
    }

    public bool Update(int id, Actor actor)
    {
        var sql = @"
            UPDATE Actors
            SET Name = @Name,
                Bio = @Bio,
                Dob = @DOB,
                Gender = @Gender
            WHERE Id = @Id";

        var rows = Execute(sql, new
        {
            Id = id,
            actor.Name,
            actor.Bio,
            actor.DOB,
            actor.Gender
        });

        return rows > 0;
    }

    public bool Delete(int id)
    {
        var sql = @"DELETE FROM Actors WHERE Id = @Id";

        var rows = Execute(sql, new { Id = id });

        return rows > 0;
    }

    public IEnumerable<Actor> GetByMovieId(int movieId)
    {
        var sql = @"
        SELECT 
            a.Id,
            a.Name,
            a.Bio,
            a.Dob AS DOB,
            a.Gender
        FROM Actors a
        INNER JOIN MovieActorMapping mam 
            ON a.Id = mam.ActorId
        WHERE mam.MovieId = @MovieId";

        return Query<Actor>(sql, new { MovieId = movieId });
    }

}
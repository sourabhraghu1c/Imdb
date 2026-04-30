using IMDBSample.Models.Request;

namespace IMDBSample.Helpers
{
    public interface IMovieValidator
    {
        void Validate(MovieRequest request);
    }
}

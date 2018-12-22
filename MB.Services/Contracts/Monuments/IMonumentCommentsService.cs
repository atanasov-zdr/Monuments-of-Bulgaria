namespace MB.Services.Contracts.Monuments
{
    using System.Linq;

    using Models;

    public interface IMonumentCommentsService
    {
        IQueryable<MonumentComment> GetAllForMonument(int monumentId);

        void Create(int monumentId, string content, string username);

        void Like(int commentId, string username);
    }
}

namespace MB.Services.Contracts.Monuments
{
    using System.Linq;

    using Models.Monuments;

    public interface IMonumentCommentsService
    {
        MonumentComment GetById(int commentId);

        IQueryable<MonumentComment> GetAllForMonumentOrderedByDateDescending(int monumentId);

        void Create(int monumentId, string content, string username);

        void Like(int commentId, string username);

        bool CheckForExistingLike(int commentId, string username);

        void Delete(int commentId);
    }
}

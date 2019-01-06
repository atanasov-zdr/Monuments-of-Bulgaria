namespace MB.Services.Contracts.Users
{
    using Models;

    public interface IUsersService
    {
        MbUser GetByUsername(string username);
    }
}

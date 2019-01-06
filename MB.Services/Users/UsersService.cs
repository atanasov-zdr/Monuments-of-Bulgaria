namespace MB.Services.Users
{
    using System;
    using System.Linq;

    using Common.Exceptions;
    using Contracts.Users;
    using Data;
    using Models;

    public class UsersService : IUsersService
    {
        private readonly MbDbContext dbContext;

        public UsersService(MbDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public MbUser GetByUsername(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentNullException(nameof(username));

            MbUser user = this.dbContext.Users.FirstOrDefault(x => x.UserName == username);
            if (user == null)
                throw new UserNullException();

            return user;
        }
    }
}

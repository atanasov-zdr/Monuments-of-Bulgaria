namespace MB.ViewModels.MonumentComments
{
    using System;

    using Mapping.Contracts;
    using Models;

    public class MonumentCommentViewModel : IMapFrom<MonumentComment>
    {
        public int Id { get; set; }

        public string Content { get; set; }

        public int Likes { get; set; }

        public int Dislikes { get; set; }

        public string UserUsername { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}

namespace MB.ViewModels.Monuments.MonumentComments
{
    using System;
    
    public class MonumentCommentViewModel
    {
        public int Id { get; set; }

        public string Content { get; set; }

        public int LikesCount { get; set; }
        
        public string UserUsername { get; set; }
        
        public DateTime CreatedOn { get; set; }

        public bool IsLiked { get; set; }
    }
}

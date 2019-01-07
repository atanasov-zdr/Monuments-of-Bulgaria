namespace MB.ViewModels.Hotels.HotelComments
{
    using System;

    public class HotelCommentViewModel
    {
        public int Id { get; set; }

        public string Content { get; set; }

        public int LikesCount { get; set; }

        public string UserUsername { get; set; }
        
        public DateTime CreatedOn { get; set; }

        public bool IsLiked { get; set; }
    }
}

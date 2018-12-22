namespace MB.ViewModels.Hotels.HotelComments
{
    using System;
    using System.Linq;

    using AutoMapper;

    using Mapping.Contracts;
    using Models;

    public class HotelCommentViewModel : IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Content { get; set; }

        public int LikesCount { get; set; }

        public string UserUsername { get; set; }

        public DateTime CreatedOn { get; set; }

        public void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<HotelComment, HotelCommentViewModel>()
                .ForMember(dest => dest.LikesCount, opts => opts.MapFrom(x => x.Likes.Count()));
        }
    }
}

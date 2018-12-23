namespace MB.ViewModels.Monuments.MonumentComments
{
    using System;
    using System.Linq;

    using AutoMapper;

    using Mapping.Contracts;
    using Models.Monuments;

    public class MonumentCommentViewModel : IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Content { get; set; }

        public int LikesCount { get; set; }
        
        public string UserUsername { get; set; }

        public DateTime CreatedOn { get; set; }

        public void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<MonumentComment, MonumentCommentViewModel>()
                .ForMember(dest => dest.LikesCount, opts => opts.MapFrom(x => x.Likes.Count()));
        }
    }
}

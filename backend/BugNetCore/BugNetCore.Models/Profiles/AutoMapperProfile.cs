namespace BugNetCore.Models.Profiles
{

    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {


            // Mapppings between CreateRequestDtos and each entity that has one that has such dto
            CreateMap<CreateBugRequestDto, Bug>();
            CreateMap<CreateProjectRequestDto, Project>();
            CreateMap<CreateUserRequestDto, User>();
            CreateMap<CreateCommentRequestDto, Comment>();
            CreateMap<CreateSupportRequestDto, SupportRequest>();
            


            // Mappings between ReadResponseDtos and each entity that has such dto
            CreateMap<Bug ,ReadBugResponseDto>();
            CreateMap<Project, ReadProjectResponseDto>();
            CreateMap<User, ReadUserResponseDto>();
            CreateMap<Comment, ReadCommentResponseDto>();
            CreateMap<SupportRequest, ReadSupportResponseDto>();
            CreateMap<ChatMessage, ReadMessageResponseDto>();

            CreateMap<SupportRequest, UpdateSupportResponseDto>();

            // Mappings between UpdateRequestDtos and each entity that has such dto
            CreateMap<UpdateBugRequestDto, Bug>();
            CreateMap<UpdateProjectRequestDto, Project>();
            CreateMap<UpdateUserRequestDto, User>();
            CreateMap<UpdateCommentRequestDto, Comment>();
            CreateMap<UpdateSupportRequestDto, SupportRequest>();


            // Mappings between BaseEntity and each entity, as it's the DTO for the delete operation
            CreateMap<BaseDto, Bug>();
            CreateMap<BaseDto, Project>();
            CreateMap<BaseDto, User>();
            CreateMap<BaseDto, Comment>();
            CreateMap<BaseDto, SupportRequest>();




        }



    }
    public static class MapperExtensions
    {
        public static IMappingExpression<TSource, TDestination> IgnoreAllMembers<TSource, TDestination>(this IMappingExpression<TSource, TDestination> expr)
        {
            var destinationType = typeof(TDestination);

            foreach (var property in destinationType.GetProperties())
                expr.ForMember(property.Name, opt => opt.Ignore());

            return expr;
        }
    }
}
namespace BugNetCore.Models.DTOs.Base
{
    public class ReadAllRecordsResponseBaseDto<IReadRecordResponseDto>
    where IReadRecordResponseDto : new()
    {
        public int TotalRecords { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public List<IReadRecordResponseDto> Records { get; set; }

    }
}
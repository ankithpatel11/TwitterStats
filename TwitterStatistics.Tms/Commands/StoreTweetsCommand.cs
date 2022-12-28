using MediatR;

namespace TwitterStatistics.Tms.Commands
{
    public class StoreTweetsCommand : IRequest<bool>
    {
        public string APIKey { get; set; }
        public string APIKeySecret { get; set; }
        public string BearerToken { get; set; }
    }
}

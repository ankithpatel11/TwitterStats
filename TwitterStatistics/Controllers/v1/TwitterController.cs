using MediatR;
using Microsoft.AspNetCore.Mvc;
using TwitterStatistics.Api.Queries;

namespace TwitterStatistics.Api.Site.Controllers.v1
{
    [Route("api/[controller]")]
    [ApiController]
    public class TwitterController : ControllerBase
    {
        private readonly ILogger<TwitterController> _logger;
        private IMediator _mediator;

        public TwitterController(IMediator mediator, ILogger<TwitterController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        /// <summary>
        /// Api to get stream from twitter - Invoke me first to fetch twitter streams
        /// </summary>
        /// <returns></returns>
        //[HttpPost(Name = "Store")]
        //public IActionResult StoreTweets()
        //{
        //    try
        //    {

        //        var command = new StoreTweetsCommand() { BearerToken = ApplicationSettings.GetAppSettings("TwitterBearerToken") };
        //        var res =  _mediator.Send(command);

        //        //_queue.QueueBackgroundWorkItem(async token =>
        //        //{
        //        //    using (var scope = _serviceScopeFactory.CreateScope())
        //        //    {
        //                    //var scopedServices = scope.ServiceProvider;
        //        //    }
        //        //});

        //        return Ok();
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}

        /// <summary>
        /// Api to Get status of tweets received from twitter
        /// </summary>
        /// <returns></returns>
        [HttpGet(Name = "Stats")]
        public IActionResult Stats()
        {
            try
            {
                var query = new GetTwitterStatsQuery();
                var res = _mediator.Send(query).Result;
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

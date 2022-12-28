using FluentScheduler;

namespace Scheduler
{
    internal class TweetsScheduler : Registry
    {
        public TweetsScheduler()
        {
            Schedule<MinuteTask>().ToRunEvery(19).Seconds();
        }
    }


    internal class MinuteTask : IJob
    {
        public void Execute()
        {
            //invoke api to fetch tweets and store
            Console.WriteLine($"Triggered at {DateTime.Now}");
            //not required
        }
    }

}

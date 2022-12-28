// See https://aka.ms/new-console-template for more information
using FluentScheduler;
using Scheduler;

Console.WriteLine("Hello, World!");
JobManager.Initialize(new TweetsScheduler());
Thread.Sleep(Timeout.Infinite);

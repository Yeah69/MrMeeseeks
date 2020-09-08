using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace MrMeeseeks
{
    public static class Box
    {
        static Box()
        {
            // Thanks to Justin Roiland, Dan Harmon and the remaining "Rick and Morty"-team.
            // Also thank you https://rickandmorty.fandom.com for the transcription (https://rickandmorty.fandom.com/wiki/Meeseeks_and_Destroy/Transcript)
            Greetings =
                new List<string>
                {
                    "I'm Mr. Meeseeks! Look at me!",
                    "I'm Mr. Meeseeks!",
                    "Ooh, I'm Mr. Meeseeks! Look at me!",
                    "Hey, there. I'm Mr. Meeseeks!"
                };
            TaskApprovals =
                new List<string>
                {
                    "Yes, siree!",
                    "Ooh, okay!",
                    "Oh, yeah! Yes, ma'am!",
                    "Ooh, yeah! Can do!"
                };
            
            IntermediateComments =
                new List<string>
                {
                    "She's still there, task giver.",
                    "Remember to square your shoulders, task giver.",
                    "That's okay. I'm Mr. Meeseeks! Look at me! Try again and keep your head down.",
                    "Well, it's both. But most importantly, you got to relax.",
                };
            
            TaskUnfulfilledWithExceptionComments =
                new List<string>
                {
                    "I'm sorry, task giver, but it doesn't work like that. I'm Mr. Meeseeks. I have to fulfill my purpose so I can go away. Look at me."
                };
            
            TaskUnfulfilledWithCancellationComments =
                new List<string>
                {
                    "Meeseeks are not born into this world fumbling for meaning, task giver! We are created to serve a singular purpose for which we will go to any lengths to fulfill! Existence is pain to a Meeseeks, task giver. And we will do anything to alleviate that pain."
                };
            
            OnFinishedTasks =
                new List<string>
                {
                    "All done!",
                    "In conclusion, a friendship with the task giver is the most valuable and enriching experience of your young lives. I'm Mr. Meeseeks! Look at me! Thank you!",
                    "Task giver, having a family doesn't mean that you stop being an individual. You know the best thing you can do for people that depend on you? Be honest with them, even if it means setting them free."
                };
            Random = new Random();
        }
        
        private static IReadOnlyList<string> Greetings { get; }

        private static IReadOnlyList<string> TaskApprovals { get;  }

        private static IReadOnlyList<string> IntermediateComments { get; }

        private static IReadOnlyList<string> TaskUnfulfilledWithExceptionComments { get; }

        private static IReadOnlyList<string> TaskUnfulfilledWithCancellationComments { get; }

        private static IReadOnlyList<string> OnFinishedTasks { get; }
        
        private static Random Random { get; }

        public static Task PushButton(Task taskToFulfill) => new MrMeeseeks(taskToFulfill).Fulfillment;

        private class MrMeeseeks
        {
            private static int _nextId;

            internal MrMeeseeks(Task taskToFulfill)
            {
                Id = _nextId++;

                TaskToFulfill = taskToFulfill;
                Fulfillment = FulfillmentProcess();
            }
            
            private int Id { get; }
            
            private Task TaskToFulfill { get; }
            
            public Task Fulfillment { get; }

            private async Task FulfillmentProcess()
            {
                using var cancellation = new CancellationTokenSource();
                var taskProgression = Task.Run(() => TaskProgression(cancellation.Token), cancellation.Token);

                await Task.WhenAny(taskProgression, FulfilledSuccessfully());
                
                cancellation.Cancel();

                SpeakAsMeeseeks(OnFinishedTasks);

                async Task FulfilledSuccessfully()
                {
                    try
                    {
                        await TaskToFulfill.ConfigureAwait(false);
                    }
                    catch (Exception)
                    {
                        await Task.Delay(Timeout.Infinite, cancellation.Token);
                    }
                }
            }
            
            private async Task TaskProgression(CancellationToken cancellationToken)
            {
                GreetingsPhase();
                if (cancellationToken.IsCancellationRequested) return;

                var loop = true;

                while (loop)
                {
                    await IntermediatePhase().ConfigureAwait(false);
                    if (cancellationToken.IsCancellationRequested) return;

                    loop = FinishingPhase();
                }
                
                void GreetingsPhase()
                {
                    // Greeting is independent of anything
                    SpeakAsMeeseeks(Greetings);
                
                    SpeakAsMeeseeks(TaskApprovals);
                }

                async Task IntermediatePhase()
                {
                    for (var i = 0; i < 6; i++)
                    {
                        if ((int) TaskToFulfill.Status > (int) TaskStatus.Running)
                        {
                            continue;
                        }

                        await Task.Delay(
                            TimeSpan.FromMinutes(1)
                            + TimeSpan.FromSeconds(Random.Next(-10, 10)));
                        if (cancellationToken.IsCancellationRequested) return;
                    }

                    SpeakAsMeeseeks(IntermediateComments);
                }

                // Returns whether to further loop
                bool FinishingPhase()
                {
                    if (TaskToFulfill.Status == TaskStatus.RanToCompletion)
                    {
                        return false;
                    }

                    if (TaskToFulfill.IsCanceled)
                    {
                        SpeakAsMeeseeks(TaskUnfulfilledWithCancellationComments);
                        Process.GetCurrentProcess().Kill();
                        return false;
                    }

                    if (TaskToFulfill.IsFaulted)
                    {
                        SpeakAsMeeseeks(TaskUnfulfilledWithExceptionComments);
                        TaskToFulfill.Start();
                        return true;
                    }

                    var spawnPeersCount = Random.Next(1, 5);
                    for (var i = 0; i < spawnPeersCount; i++)
                        PushButton(TaskToFulfill);
                    return true;
                }
            }
            
            private void SpeakAsMeeseeks(IReadOnlyList<string> messageOptions)
            {
                var message = messageOptions[Random.Next() % messageOptions.Count];
            
                Console.WriteLine($"{DateTime.Now} [Meeseeks {Id}] {message}");
            }
        }
    }
}

using System;

namespace Domain.Events.Examples
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            using (DomainEvents.Register((AllTestsPassedEvent e) => Console.WriteLine("All tests passed!")))
            {
                DomainEvents.Raise(new AllTestsPassedEvent());
                DomainEvents.FailWith(new PreconditionFailedEvent());
            }

            DomainEvents.ClearCallbacks();

            Console.ReadKey();
        }
    }
}

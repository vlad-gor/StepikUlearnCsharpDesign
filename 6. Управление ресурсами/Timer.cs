using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Memory.Timers
{
    public class Timer : IDisposable
    {
        public static Stack<Timer> stack = new Stack<Timer>();

        public static string Report { get; set; } = "";
        public string selfReport = "";
        private Stopwatch watch;       

        private string tableLine;
        private static int counter = 0;
        private int? childrenTime = null;

        public static Timer Start(string name = "*")
        {
            counter++;
            return new Timer(name);
        }

        public Timer(string name)
        {            
            tableLine = name;
            watch = new Stopwatch();
            watch.Start();
            stack.Push(this);
        }

        private bool disposedValue = false;
        
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    stack.Pop();
                    counter--;
                    watch.Stop();

                    var currentTime = watch.Elapsed.Milliseconds;



                    tableLine = tableLine.PadLeft(counter * 4 + 1);
                    tableLine = tableLine.PadRight(20);
                    tableLine += $": {currentTime}\n";



                    string rest = "";

                    if (childrenTime != null)

                    {

                        rest = ("".PadLeft((counter + 1) * 4) + "Rest").PadRight(20);

                        rest += $": {currentTime - childrenTime}\n";

                    }



                    if (stack.Count > 0)

                    {

                        if (stack.Peek().childrenTime != null)

                            stack.Peek().childrenTime += currentTime;

                        else stack.Peek().childrenTime = currentTime;



                        stack.Peek().selfReport = stack.Peek().selfReport + tableLine + selfReport + rest;

                    }

                    else

                        Report = tableLine + selfReport + rest;

                }

                disposedValue = true;

            }

        }

                

        ~Timer()
        {
            Dispose(false);
        }



        public void Dispose()
        {            
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
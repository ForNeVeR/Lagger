// See https://aka.ms/new-console-template for more information

using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;

if (args.Length > 0 && args[0] == "--help") 
{
    Console.WriteLine("Arguments: [process-name suspend-time]");
    return 0;
}

var arguments = ReadArgs(); 

while (true) 
{
    var processes = Process.GetProcessesByName(arguments.ProcessName);
    if (processes.Length == 0) 
    {
        Console.WriteLine("No process found.");
        Thread.Sleep(arguments.SuspendTime + arguments.FreeTime);
        continue;
    }

    foreach (var process in processes)
    {
        Console.WriteLine($"Suspending process {process.Id} {process.ProcessName}…");
        SuspendProcess(process);
    }
    
    Thread.Sleep(arguments.SuspendTime);
    foreach (var process in processes)
    {
        Console.WriteLine($"Resuming process {process.Id} {process.ProcessName}…");
        ResumeProcess(process);
    }

    Thread.Sleep(arguments.FreeTime);
}


Arguments ReadArgs() 
{
    var processName = args.Length > 0 ? args[0] : "Rider.Backend";
    var timeSpanStr = args.Length > 1 ? args[1] : "00:00:05";
    var timeSpan = TimeSpan.Parse(timeSpanStr, CultureInfo.InvariantCulture);
    return new Arguments(processName, timeSpan, timeSpan);
}

[DllImport("ntdll.dll", SetLastError = true)]
static extern int NtSuspendProcess(IntPtr processHandle);

[DllImport("ntdll.dll", SetLastError = true)]
static extern int NtResumeProcess(IntPtr processHandle);

void SuspendProcess(Process process)
{
    var exitCode = NtSuspendProcess(process.Handle);
    Console.WriteLine($"SuspendProcess returned {exitCode}.");
}

void ResumeProcess(Process process)
{
    var exitCode = NtResumeProcess(process.Handle);
    Console.WriteLine($"ResumeProcess returned {exitCode}.");
}

record Arguments(string ProcessName, TimeSpan SuspendTime, TimeSpan FreeTime);

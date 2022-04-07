namespace Taynftmf.Executr

[<AutoOpen>]
module Lib =
    
    open System.Diagnostics
    open System.Threading.Tasks        

    (*
    Based on Alexandru Nedelcu's (alexn.org) code snippet
    "2020-12-06-execute-shell-command-in-fsharp" at
    https://github.com/alexandru/alexn.org/blob/de0418325f6e93e4678afce359ded255a8d454cc/_posts/2020-12-06-execute-shell-command-in-fsharp.md
    and slightly adapted to include a synchronous wrapper.
    Used with permission by Alexandru.
    *)
    
    type CommandResult =
        { ExitCode: int
        ; StandardOutput: string
        ; StandardError: string }

    let executeCommandAsync executable args =
        async {
            let startInfo = ProcessStartInfo()
            startInfo.FileName <- executable
            for a in args do
                startInfo.ArgumentList.Add(a)
            startInfo.RedirectStandardOutput <- true
            startInfo.RedirectStandardError <- true
            startInfo.UseShellExecute <- false
            startInfo.CreateNoWindow <- true
            use p = new Process()
            p.StartInfo <- startInfo
            p.Start() |> ignore

            let outTask = Task.WhenAll([|
                p.StandardOutput.ReadToEndAsync();
                p.StandardError.ReadToEndAsync()
            |])

            p.WaitForExit()
            let! out = outTask |> Async.AwaitTask
            return {
                ExitCode = p.ExitCode;
                StandardOutput = out.[0];
                StandardError = out.[1]
            }
        }
        
    let executeCommand executable args =
        executeCommandAsync executable args
        |> Async.RunSynchronously
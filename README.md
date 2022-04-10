# Taynftmf.Executr
Command execution from F# based on Alexandru Nedelcu's (www.alexn.org) [code snippet](https://github.com/alexandru/alexn.org/blob/de0418325f6e93e4678afce359ded255a8d454cc/_posts/2020-12-06-execute-shell-command-in-fsharp.md "code snippett").

## Usage Example

```
let executeShellCommand command =
  executeCommand "/usr/bin/env" [ "-S"; "bash"; "-c"; command ]

// Invocation sample
let r = executeShellCommand "ls -alh"
if r.ExitCode = 0 then
  printfn "%s" r.StandardOutput
else
  printfn "%s" r.StandardError
  Environment.Exit(r.ExitCode)
```

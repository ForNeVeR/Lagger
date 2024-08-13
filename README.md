Lagger
======
A program to auto-suspend a named process on Windows. **Use with caution!**

Syntax:

```console
$ dotnet run [process-name sleep-span]
```

where
- `process-name` is Windows process name (without `.exe` suffix), by default `Rider.Backend` (to suspend `Rider.Backend.exe`)
- `sleep-span` is a .NET-formatted timespan object, `00:00:05` by default, i.e. 5 sec

The program will do the following in endless loop:
- find all processes named as `process-name`
- suspend them
- wait for `sleep-span`
- resume them
- wait for `sleep-span`

License
-------
[MIT][docs.license]

[docs.license]: LICENSE.md

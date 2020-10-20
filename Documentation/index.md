# Romanization.NET
Welcome to Romanization.NET! This is a library for romanizing (converting to the latin alphabet) languages and writing systems that use different alphabets.

## Getting Started
Below is a brief example of how you might use the library to romanize some Korean Hangeul text:
```csharp
using Romanization;
...
string romanizedText = Korean.RevisedRomanization.Value.Process("한글");
Console.WriteLine(romanizedText);
// Outputs: han-geul
```

Every language system is lazily initialized - this means only the romanization systems you use will be loaded.

To see all available languages and systems as well as implementation details and further options, please visit the [API Documentation](/api).

## Installation
Because it is [available on NuGet.org](https://www.nuget.org/packages/Romanization.NET/), you can install it in any typical way you install any other NuGet package.

If you'd rather install it manually, the package is also available on [GitHub Packages](https://github.com/zedseven?tab=packages&repo_name=Romanization.NET).

### Command Line
To get started, just use:
```
dotnet add package Romanization.NET
```

Or in the Package Manager Console:
```
Install-Package Romanization.NET
```

### Visual Studio UI
You can also install through `Tools > NuGet Package Manager > Manage NuGet Packages for Solution...`, and search `Romanization.NET`.

## More Information
For more detailed information about the package and what it supports, please visit the [Articles Section](/articles/supported.html).
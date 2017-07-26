# CMSEngine
## What is it?
This the new code-base of my website. The old version can be seen at [http://davidsonsousa.net](http://davidsonsousa.net "My website")

## Why?
Because I need a pet project to study a couple of things. And do it with my own website seems to be the right choice.

## What is this project using? (or: _What I intend to study and practice with this project?_)
* [ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/)
* [C#](https://www.microsoft.com/net/tutorials/csharp/getting-started)
* [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/)
* [AutoMapper](http://automapper.org/)
* [xUnit](https://xunit.github.io/)
* [Angular 4](https://angular.io/)
* [TypeScript](http://www.typescriptlang.org/)

## Running the project
Since this is a .NET Core project you can run in 2 ways:

### .NET Core CLI
- Set the development environment (must be set for every session)
  - PowerShell: `$Env:ASPNETCORE_ENVIRONMENT = "Development"`
- Run the project
  - `cd ..\CmsEngine.Ui`
  - `dotnet watch run`
- Create and run migrations
  - `cd ..\CmsEngine`
  - `dotnet ef --startup-project ..\CmsEngine.Ui\ migrations add MIGRATION_NAME`
  - `cd ..\CmsEngine.Ui`
  - `dotnet ed database update`

### IIS Express
- Just run with <kbd>Ctrl</kbd> + <kbd>F5</kbd> (or <kbd>F5</kbd> for debugging) and let everything happens

### IIS
- You will need to setup your own IIS
- <kbd>Ctrl</kbd> + <kbd>Shift</kbd> + <kbd>P</kbd> to attach the process and debug

---

_Note: I am not following any particular pattern nor best practice in this project. Feel free to help._

# CMSEngine
## What is it?
This the new code-base of the CMS I am using on my website [http://davidsonsousa.net](http://davidsonsousa.net "my website")

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

### .NET Core CLI (using PowerShell)
- Set the development environment
  - Go to the solution folder
  - Execute the file `ConfigDevEnv.ps1` (must be set every time you open an instance of PowerShell)
- Run the project
  - `cd ..\CmsEngine.Ui`
  - `dotnet -d watch run`
    - `-d` shows the PID for debugging purposes
  - Open your favorite browser and load `http://cmsengine.dev:5000`.
    - Remember to map this url in your hosts file.
- Create and run migrations
  - `cd ..\CmsEngine`
  - `dotnet ef --startup-project ..\CmsEngine.Ui\ migrations add MIGRATION_NAME`
  - `cd ..\CmsEngine.Ui`
  - `dotnet ef database update`

### IIS Express
- Just run with <kbd>Ctrl</kbd> + <kbd>F5</kbd> (or <kbd>F5</kbd> for debugging) and let everything happens

### IIS
- You will need to setup your own IIS
- <kbd>Ctrl</kbd> + <kbd>Shift</kbd> + <kbd>P</kbd> to attach the process and debug

---

_Note: I am not following any particular pattern nor best practice in this project. Feel free to help._

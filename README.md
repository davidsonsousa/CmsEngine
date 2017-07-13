# CMSEngine
## What is it?
This the new code-base of my website. The old version can be seen at [http://davidsonsousa.net](http://davidsonsousa.net "My website")

## Why?
Because I need a pet project to study a couple of things. And do it with my own website seems to be the right choice.

## What I intend to study and practice with this project
* C#
* ASP.NET Core
* Entity Framework Core
* xUnit
* Angular 4
* SASS

## Running the project
Since this is a .NET Core project you can run in 2 ways:

### .NET Core CLI
- Set the development environment (using _PowerShell_)
  - `$Env:ASPNETCORE_ENVIRONMENT = "Development"`
- Run the project (on _CmsEngine.Ui_)
  - `dotnet watch run`
- Run migrations (on _CmsEngine_)
  - `dotnet ef --startup-project ..\CmsEngine.Ui\ migrations add MIGRATION_NAME` (Replace _MIGRATION_NAME_ with a name which makes sense)

### IIS Express
- Just run with <kbd>Ctrl</kbd> + <kbd>F5</kbd> (or <kbd>F5</kbd> for debugging) and let everything happens

### IIS
- You will need to setup your own IIS
- <kbd>Ctrl</kbd> + <kbd>Shift</kbd> + <kbd>P</kbd> to attach the process and debug

---

_Note: I am not following any particular pattern nor best practice in this project. Feel free to help._

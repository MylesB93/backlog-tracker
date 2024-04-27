# Backlog tracker

A web application that allows user's to search for games and add them to their virtual backlog. User's can create an account so their data persists. Search data is pulled in from the [Giantbomb API](https://www.giantbomb.com/api/documentation/)

## User Secrets

Ensure to run the following command to initialise the user secrets:

`dotnet user-secrets init`

Then run the following command to add each required user secret:

`dotnet user-secrets set` *name-of-secret*

## Project Overview
A .NET core application that allows gamers to search for video games, and add them to their "backlog". The application is made up of the following:
- [.NET Core Identity](https://learn.microsoft.com/en-us/aspnet/core/security/authentication/identity?view=aspnetcore-8.0&tabs=visual-studio) to allow user's of the application to register an account
- [Entity Framework Core (version 8)] (https://learn.microsoft.com/en-us/ef/core/) for persisting user data
- [GiantBomb API](https://www.giantbomb.com/api/documentation/) for getting video game data
- [Bootstrap](https://getbootstrap.com/docs/5.3/getting-started/introduction/) for styling the frontend

## Database structure
Entity Framework Core database. Key table is AspNetUsers which contains a column called "GameIDs". When user's add a game to their backlog, the ID of that game is added to this column.

## GiantBomb service 
Interacts with GiantBomb API. 
- GetGamesAsync(): gets all the games from the API based on a query passed as a query string (which is pulled from the search bar on the frontend
- GetUsersGamesAsync(): gets all the game's a user has in their backlog based on a list of game ID's (which are stored in the database)

## Backlog service 
Interacts with the database. 
- AddToBacklog(): gets passed an ID and adds it to the "GameIDs" column in the User's table in the database
- RemoveFromBacklog(): gets passed an ID, checks if it is present in the "GameIDs" column in the User's table in the database, and removes it if it is
- GetBacklog(): returns all the items in the "GameIDs" column in the User's table as a List<string>

## BacklogController - .NET Web API Controller
Called from JavaScript code to add and remove games from backlog. Backlog service injected in to make use of add/ remove/ get methods.
- AddToBacklog(): PATCH method for adding game ID's to user table. Calls Backlog service's AddToBacklog() method.
- RemoveFromBacklog(): PATCH method for removing game ID's from user table. Calls Backlog service's RemoveFromBacklog() method.
- GetUsersBacklog(): GET method for retrieving a user's games. Calls Backlog service's GetBacklog() method.

## JavaScript
Methods using the Fetch API to make API calls to BacklogController.
- addToBacklog(): Calls BacklogController's AddToBacklog() method to update user's GameID's
- removeFromBacklog(): Calls BacklogController's RemoveFromBacklog() method to update user's GameID's

## Razor pages
Games:
- Games.cshtml: Displays list of user's games
- Games.cshtml.cs: Contains OnGet() method that grabs current user's backlog from GameIDs table (using the backlog service), then passes that list to GetUsersGamesAsync() to send it to the GiantBomb API and retrieve information (e.g. name and description) about each game

Homepage:
- Index.cshtml: Contains search bar where user's can search for games
- Index.cshtml.cs: OnPost() method that is passed a string from the search bar, then calls the GetGamesAsync() method from the game service to get information from the GiantBomb API based on that string. The page is then updated to display this information
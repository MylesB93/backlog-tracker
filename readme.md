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
- GetGamesAsync(): Gets all the games from the API based on a query passed as a query string (which is pulled from the search bar on the frontend.
- GetUsersGamesAsync(): .ets all the game's a user has in their backlog based on a list of game ID's (which are stored in the database).

## Backlog service
Gets information about user's backlogs.
- AddToBacklog(): Gets passed a UserDto (continaing a user's email and a game ID) which it then passes to the user repository to update that user, and add the relevant game to their backlog.
- RemoveFromBacklog(): Gets passed a UserDto (continaing a user's email and a game ID) which it then passes to the user repository to update that user, and remove the relevant game from their backlog.
- GetBacklog(): Takes an email address which it passes to the user repository to get that user's backlog as a list of strings. This list is then passed to the calling method.

## User service
Gets information about users from the user repository.
- GetAllUsers(): Calls the user repositories GetAllUsers() method and is returned a list of BacklogTrackerUser objects, which it passes to the calling method.
- GetUser(): Takes a single ID, then calls the user repositories GetUser() method, which returns a BacklogTrackerUser object, which it passes to the calling method.

## User repository
Interacts with the AspNetUsers table in the database.
- AddToUsersBacklog(): Takes a UserDTO object which includes the user's email and the game ID they want to add to their backlog. This is then used to update the database and add the ID to the user's GameIDs table.
- RemoveFromUsersBacklog(): Takes a UserDTO object which includes the user's email and the game ID they want to remove from their backlog. This is then used to update the database and remove the ID from the user's GameIDs table.
- GetUsersBacklog(): Takes the user's email address, then uses that to retrieve all the ID's in the user's GameIDs table as a list of strings. This list is then returned to the calling method.
- GetAllUsers(): Returns all of the user's in the AspNetUsers table (as a list of BacklogTrackerUser objects) to the calling method.
- GetUser(): Takes a single user ID which it uses to get the relevant user from the AspNetUsers table, which is then returned to the calling method as a BacklogTrackerUser object.

## BacklogController - .NET Web API Controller
Called from JavaScript code to add and remove games from backlog. Backlog service injected in to make use of add/ remove/ get methods.
- AddToBacklog(): PATCH method for adding game ID's to user table. Calls Backlog service's AddToBacklog() method.
- RemoveFromBacklog(): PATCH method for removing game ID's from user table. Calls Backlog service's RemoveFromBacklog() method.
- GetUsersBacklog(): GET method for retrieving a user's games. Calls Backlog service's GetBacklog() method.

## JavaScript
Methods using the Fetch API to make API calls to BacklogController.
- addToBacklog(): Calls BacklogController's AddToBacklog() method to update user's GameID's.
- removeFromBacklog(): Calls BacklogController's RemoveFromBacklog() method to update user's GameID's.

## Razor pages
### Games:
- Games.cshtml: Displays list of user's games.
- Games.cshtml.cs: Contains OnGet() method that grabs current user's backlog from GameIDs table (using the backlog service), then passes that list to GetUsersGamesAsync() to send it to the GiantBomb API and retrieve information (e.g. name and description) about each game.

### Homepage:
- Index.cshtml: Contains search bar where user's can search for games.
- Index.cshtml.cs: OnPost() method that is passed a string from the search bar, then calls the GetGamesAsync() method from the game service to get information from the GiantBomb API based on that string. The page is then updated to display this information.

### Users:
- Index.cshtml: Displays a list of all user's that have registered to the site. Clicking a user opens the user page.
- UserPage.cshtml: Displays a list of all the games in that user's backlog.
- UserPage.cshtml.cs: OnGet() async method that uses the UserID query string to get the relevant user from the user service. A list of all the game ID's in that user's GameIDs column is then used to get information on those games using the game service.
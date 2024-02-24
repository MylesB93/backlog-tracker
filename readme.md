# Backlog tracker

A web application that allows user's to search for games and add them to their virtual backlog. User's can create an account so their data persists. Search data is pulled in from the [Giantbomb API](https://www.giantbomb.com/api/documentation/)

## User Secrets

Ensure to run the following command to initialise the user secrets:

`dotnet user-secrets init`

Then run the following command to add each required user secret:

`dotnet user-secrets set` *name-of-secret*
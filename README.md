# Vintri Demo Beer Api

This is demo app for Beer Api with features to add rating to existing beer from Punk Api with information like
username: (valid email)
rating: (1-5)
comments: (text)

These are stored locally within the API, and users can quesry to get the rating provided to the Punk Api beers.

# How to start running the application

Open the Vintri.Beers.Api.sln with any Visual studio with dotnet framework 4.8 support.
After build please compile and run with IISExpress with url as
https://localhost:44315

Change the url to https://localhost:44315/swagger to open the swagger page

![Alt text](ApiSwaggerImage.PNG?raw=true "Api Swagger")

# How to run the unit test project

Open the Test virtual folder to run the unit test project
![Alt text](ApiUnitTestImage.PNG?raw=true "Unit Test Api")

# Logging File Location

C:\Logs\VintriBeersApi.log


# Assumptions Made

The source of any beer is still PunkApi, which users can query to get any beer by Id or Name.

If user add any Rating then that information is stored in local database.json.

User have an endpoint to query see all beer which have rating, which combines data from both PunkApi and local json file.

The joining link between beer from PunkApi and local database.json, is the beer id.

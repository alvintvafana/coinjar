# coinjar
## How to run the program

 - Folder to be in : CoinJar.Api 
 - On your command window : dotnet run
 - To view swagger : [https://localhost:5001/swagger/index.html](https://localhost:5001/swagger/index.html)
 
 ## Concepts and patterns used
 - I made use of DDD, which has the benefit of having all your logic sit
   in one place rather than scatted throughout your system.  
 - I also made use of CQRS, the repository pattern, and the Unit of Work.
 
  ### Other Details
- The project stores data in SQlite.
- Denomination and volumes of the coins are kept in the appsetting.json file to make it easy to change coin denomination or volume of a given coin.
- a Jar is associated with one given username, and keeps track of the balance and current volume inside the Jar table and a list of all the transactions in the  Transaction table.

### Where Improvements can be made
 - The application could be improved by placing it in a container like docker
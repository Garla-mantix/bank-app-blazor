# Bankster - A Banking Webapp in Blazor (.NET)

## Description

Welcome to this project for a Blazor WebAssembly application using .NET 8.  
I have previously done some websites and apps using HTML, CSS and Javascript (with Tailwind, React, Bootstrap, SCSS etc)  
– but the goal during this project was to get familiar with Blazor and learn more about clean architecture principles.  
The app simulates a simple banking system, where data persist using the browser's local storage. 

### Features

- Create new accounts  
- View existing accounts  
- Deposit and withdraw funds  
- Transfer money between accounts  
- Browse, sort and filter transaction history 

### Technologies used

* .NET 8 / C# 12
* Blazor WebAssembly (standalone)
* HTML
* CSS (including Bootstrap)
<br/>

## Architecture overview

### Domain – _Core business logic and data models_
* Business logic: BankAccount and Transaction (handles deposit/withdrawals and transfers,  
  maintaining a consistent balance and transaction list).
* Data models: Enums for AccountType, TransactionType and CurrencyType.

### Interfaces – _Abstractions for services_
* IAccountService: Defines what services can do.
* IStorageService: Defines how saving and retrieving data works.
* IBankAccount: Defines structure and behaviour of accounts.

### Services – _Implementation of application workflow and data persistence_
* AccountService: Implements workflow of banking operations.
* StorageService: Saving and retrieving data from local storage.

### Pages – _UI_
* Blazor-components for displaying and user interaction.

### Data Flow
1. User interacts with a Razor-page in the UI.
2. The page calls methods from services (AccountService).
3. AccountService updates the relevant BankAccount entities.
4. Data is persisted via StorageService.
5. The UI re-renders and changes are visible to the user.
<br/>   

## Why this structure?
*  Using this type of architecture, the goal is to create independent layers that are easier to maintain.  
   For example we could replace parts of the infrastructure, e.g. swap the use of local storage for a real database later.  
   The connections remain the same, we only need to change the end point, so to speak.
* We divide responsibilities between several classes, so that each class does one thing well.  
  This creates better clarity, and if something breaks it probably easier to figure out where to look for the solution,  
  compared to if we had one single class stuffed with all the logic of the whole app.
* It will hopefully also be easier to extend the functionality of the app without rewriting too much of the old code.
<br/>

## What's next?
For the future there are some things that we could improve on:
* User login and authentication.
* Integration with a real database (e.g. SQLite).
* Exporting/importing data via JSON.
* Improved responsiveness and UI.
* Make it possible to navigate to a specific account's transaction history from the My Accounts-page.
* Make currency exchange possible when transferring funds between accounts with different currencies.
* Budget categories for withdrawals and exportable monthly expense reports.
* Interest rates on savings accounts.
<br/>

## How to check out the app
1. Install the .NET 8 SDK.
2. Clone the repository from GitHub.
3. Run the bash-command "dotnet run" in your terminal (at the correct folder location).
4. Open your browser at "http://localhost:5001".
<br/>

## Author
**Luca Pirro**  
_Built as a learning project and reference implementation for Clean Architecture in Blazor._





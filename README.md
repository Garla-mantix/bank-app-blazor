# Bankster - a banking webapp in Blazor (.NET)

## Description

Welcome to this project for a Blazor WebAssembly application using .NET 8.
I have previously done some websites and apps using HTML, CSS and Javascript (with Tailwind, React, Bootstrap, SCSS etc) 
– but the goal during this project was to get familiar with Blazor and learn more about clean architecture principles.
The app simulates a simple banking system, where data persist using the browser's local storage. 

### The functionality of the app includes:
* Creating an account.
* Viewing existing accounts.
* Making deposits and withdrawals to those accounts.
* Transfer funds between accounts.
* Viewing transaction history, including sorting and filtering.

### Technologies used
* .NET 8 / C# 12
* Blazor WebAssembly (standalone)
* HTML
* CSS
<br/>

## Architecture overview

### Domain – Core business logic and data models
* Business logic --> BankAccount and Transaction (handles deposit/withdrawals and transfers, maintaining a consistent balance and transaction list).
* Data models --> Enums for AccountType, TransactionType and CurrencyType.

### Interfaces – Abstractions for services
* IAccountService --> Defines what services can do.
* IStorageService –-> Defines how saving and retrieving data works.
* IBankAccount –-> Defines structure and behaviour of accounts.

### Services – Implementation of application workflow and data persistence
* AccountService --> Implements workflow of banking operations.
* StorageService --> Saving and retrieving data from local storage.

### Pages – UI
* Blazor-components for displaying and user interaction.

### Data Flow
1. User interacts with a Razor-page in the UI.
2. The page calls methods from services (AccountService).
3. AccountService updates the relevant BankAccount entities.
4. Data is persisted via StorageService.
5. The UI re-renders and changes are visible to the user.
<br/>   

## So, why this structure?
*  Using this type of architecture, the goal is to create independent layers that are easier to maintain. For example we could replace parts of the infrastructure, e.g. swap the use of local storage for a real database later. The connections remain the same, we only need to change the end point, so to speak.
* We divide responsibilities between several classes, so that each class does one thing well. This creates better clarity, and if something breaks it probably easier to figure out where to look for the solution, than if we had one single class stuffed with all the logic of the whole app.
* It will hopefully also be easier to extend the functionality of the app without rewriting too much of the old code.
<br/>

## What's next?
For the future there are some things that we could improve on:
* Create a login with user accounts and authentication.
* Integrate a real database (like SQLite) instead of using local storage for persistence.
* Exporting/importing data via JSON.
* Improve responsiveness and UI.
* Make it possible to navigate to a specific accounts transaction history from the My Accounts-page.
* Make currency exchange possible when transferring between accounts with different currencies.
* Create budget categories for withdrawals and create exportable monthly expense reports.
* Implement interest rates on the savings accounts.
<br/>

## How to check out the app
1. Make sure to have .NET 8 SDK installed.
2. Clone the repo from GitHub.
3. Run the command "dotnet run" in your terminal (at the correct folder location).
4. Open your browser at "http://localhost:5001".
<br/>

## Author
**Luca Pirro**  
_Built as a learning project and reference implementation for Clean Architecture in Blazor._





# To-Do Application
ToDo application created with **ASP.NET Web API** on the backend and **React** on the frontend. 

## Used technologies
On the frontend just `React` was used. On the backend, following technologies were used:
- AspNetCore
- EntityFramework
- JWT tokens
- NUnit
- StyleCop

## Install
First of all, you will need to download project code. The easiest way is to clone it using git:
```cmd
cd <folder_where_you_want_to_place_project>
git clone https://github.com/evgenius081/To-Do.git
```
### Backend
You need Visual Studio 2022 to run this application, and that's it. You can open the solution double-clicking on the `ToDoApplication.sln` file in the main folder.

### Frontend
You will need **Node.js** installed to run this project. Open command line in the main project folder and then run following commands:
```cmd
cd ClientApp
npm install
```

## Startup
### Backend
You need to choose `IIS Express` as Startup if it is not selected by default and `ToDoApplication` as a Startup project. Then just press the launch button or press `Ctrl + F5`, or `F5` if you do not want to start the debugging.
### Frontend
1. Open command line in the main project folder.
2. Run
```cmd
cd ClientApp
```
3. Run
```cmd
npm run start
```
4. If the browser window does not open itself, go to `http://localhost:3000`

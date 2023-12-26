# Advanced To-Do Application
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
npm install
```
4. Run
```cmd
npm run start
```
5. If the browser window does not open itself, go to `http://localhost:3000`
### Docker
There is also a possibility to run the entire solution in Docker. To do so, just run 
```cmd
docker compose up
```
in main folder and wait until all docker containers will be ready.

## Roadmap
- [x] ~~Separate controllers into controllers and services~~
- [x] ~~Separate Application project into Application and Services~~
- [x] ~~Separate DomainModel project into DomainModel and infrastructure~~
- [x] ~~Dockerize solution~~
- [x] ~~Change `IdentityUser` to my own implementation~~
- [x] ~~Change security system~~
- [ ] Remake frontend to Angular and redesign it (in separate branch)
- [ ] Change reminder notification system
- [ ] Add images to items and lists
- [ ] Turn solution into microservices (in separate repo)

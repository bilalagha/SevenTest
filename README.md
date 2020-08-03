# SevenTest
Repository for Seven West Media Technical assignment 
=====================================================

## Authors

* **Milos Nikolic** - *Assignment Sent By* 
* **Bilal Agha** - *Solution Development* - [Bilal Agha](https://github.com/bilalagha)

## Resources
For Assingment Document See : [Document Core Technical Assignment_20200228.docx]

## Prerequisite
The project is developed in Visual studio 2019 Preview , 
dotNet core 3.1 is used
Redis
If "DistributedCacheEnabled": true then redis server needs to be installed , Please follow (https://riptutorial.com/redis/example/29962/installing-and-running-redis-server-on-windows) and configure 
"ConnectionStrings": {
    "Redis": "127.0.0.1:6379"
  },

## Startup
If Mutliple Statup project is not configured already :
After opening the solution Go To Solution Property and select MultipProjects in statup projects
Select Start in Action Drop Downs of SevenTest.WebApi and SevenTest.ConsoleOutput project and Click Apply So that Both project Run At A time


## Questions/Assumptions
Some Question arose and I have developed is as per my assumption, I can fix it if after clearification found out that assumption are wrong.
1. As the document example only shows Male and Female Gender in Response to Get Number of Gender Per Age so the solution is designed with the assumption that other gender should not be displayed
2. How frequently the source is updated. This is important to know for the performance tuning. currently I have set all the cache timeouts to 30 in json config file of API Project can be configured after the source update frequency is know

## Structure of your application
The Application is Structured in separate project for separation of concerns. and reusability 
Projects
1.	SevenTest.Core : Project is the base class library project which is used all of the project it contains common interface, Exceptions and Models, It also contain the IPersonRepository which enable the source to be changed
2.	SevenTest.ApiRepository : is the project which contain is Api implementation of IPersonRepository (in future it can contain other api implementation of repositories if needed) which uses the api calls and relevant error and timeout handling
3. 	SevenTest.Business : is the project where main reusable business logic exists, it contains to conditional and filtration logic as well as Distributed cache handling if enabled through configuration, The main class is Business Service
4.	SevenTest.WebApi : Project is the gateway to be utilised for output projects. as output project is not limited to console , WebApi project will serve as a versatile gateway for several kind of outputs , e.g Console, Web , Mobile, Smart Devices, Web Api Project is also handles the https errors specially timouts
5.	SevenTest.ConsoleOutput : this project is for now the primary output project. The effort was given to have least amount of logic in the project the only logic this project have is necessary for the output and also functional reusablity is considered so that in an event the Parameters like Id, age is changed the project can handle it
6.	SevenTest.PersonTest : The project contain unit test for the SevenTest.Business Project PersonService Class, for now 6 unit tests are done, more can be added to have better code coverage, moq is used for mocking the parameter required by service to have clear unit testing
	


## Configuration/Performance Tunning
For Performance , Timeouts and Distributed Cache is implemented
DistributedCacheEnable, Cache Expiry and Timeouts for Each function can be configured in appsettings.json file of SevenTest.WebApi Project
If DistributedCache is enabled then care should be given to each function cache expiry , which will mostly be depends on how frequently each type of data is updated on source 
The sourceApiTimeout can be configured to set the timeout as the source is known to be slow so don’t want user to wait for output of the source server do not respond
Following Configuration can be changed for Performance Tunning and changing the source URL and Enableing Disabling Distrubuted cace in appsettings.json file of SevenTest.WebApi Project
"ConnectionStrings": {
    "Redis": "127.0.0.1:6379"
  },
  "DistributedCache": {
    "DistributedCacheEnabled": true,
    "GetFullNameByIdExpirationSeconds": 120,
    "GetFirstNameByAgeExpirationSeconds": 120,
    "GetGenderPerAgeExpirationSeconds": 120,
    "GetPersonsExpirationSeconds": 120
  },
  "SourceApiTimeoutInSeconds": 30,
  "SourceApiUrl": "https://f43qgubfhf.execute-api.ap-southeast-2.amazonaws.com/sampletest"
Also urs of Developed WebApi Project is 





## Testability
Testability is achieved by using Interface and dependency injection and separation of concerns , There are layer of abstraction and dependency injection is used to map the implementation and because of this reason we are able to use mocking to have better testability

## Reusability
Separate Project Layers and Separation of concern is implemented for Reusability 
Its done at various Level
1. Project Level: Frontend Output project can be replaced with Mobile of Web Project, Business Layer can be replace or recuse in other project by different business layer as its implemented as implentation of IPersonService Interface, and All Other code can be used if we change the source . The Logic of other projects will still be reusable.
2. Interface and Classes Leve. The Classes are made in way that can be recuse. Effort have been given to not have repeated code and code can be reused now and in future. 
3. Function Level. For example in ConsoleOutput project even not much oops is used but PersonConsoleHelper is written in a way that if variable like id or age is changed in future it can be handled easily.

## Readability
The code is written in expressive variable name and test function name so that good readability of code can be achieve, there is still room for improvement by refactoring the code and adding the comment which is not done because of time constraint.

## Performance 
It is handled in Configuration/Performance Tunning section above

## Considerations
1.	The data source may change   					: IPersonRepository is used in service , if a new data source needs to be used it matter of have another implementation of IPersonRepository and using it in Dependency injection via startup.cs file of SevenTest.WebApi Project
2.	The endpoint could go down   					: If endpoint is down the cache will come to rescue it , if cache don’t have data then error handling have been done and user will be notified gracefully, May be default data implementation can be done depending on business scenario and further discussion
3.	The endpoint has known to be slow in the past.			: Timeouts is used and Cached is used to handle this purpose. May be Polly or alternate methodology can be used to have varied retry policy on different type of errors.
4.	The way source is fetched may change 				:IPersonService have the main business logic which contain the way data is retrieved. if its change we can implement new  Class that inherits IPersonService and push it through dependency injectin in startup.cs class of SevenTest.Api Project
5.	The number of records may change (performance)			:Cache and Timeouts are used to handle the performance issue, Paging may be used after further discussion
6.	The functionality may not always be consumed in a console app.	: SevenTest.WebApi Project is used as web api gateway are wrapper to the personserver(SevenTest.Business), The Gateway can be used with any project .Net or any other technology to have the output as webapi is not a day most common way for reusable service architecture. e.g webapi can be client of Console, Web, Mobile, Smart Devices and other type of output projects.

## ToDo 
1.More Unit Tests
2.Page based retrieval may be used after further discussion
3.More Error Handling
4.Poly or alternative tool can be used to have policy base http response handeling and retries
5.Generic IAsyncRepository can be implemented to have more reusablity.
6.The question mentioned above can be cleared and implement the detail accordingly

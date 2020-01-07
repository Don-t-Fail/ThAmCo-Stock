ThAmCo.Stock
=====
| Master | Develop | CodeFactor
|--|--|--|
| [![Build Status](https://dev.azure.com/Stedoss/ThAmCo.Stock/_apis/build/status/Don-t-Fail.ThAmCo-Stock?branchName=master)](https://dev.azure.com/Stedoss/ThAmCo.Stock/_build/latest?definitionId=1&branchName=master) | [![Build Status](https://dev.azure.com/Stedoss/ThAmCo.Stock/_apis/build/status/Don-t-Fail.ThAmCo-Stock?branchName=develop)](https://dev.azure.com/Stedoss/ThAmCo.Stock/_build/latest?definitionId=1&branchName=develop) | [![CodeFactor](https://www.codefactor.io/repository/github/don-t-fail/thamco-stock/badge)](https://www.codefactor.io/repository/github/don-t-fail/thamco-stock)



## Overview
This web service handles stock and pricing within the ThAmCo project. Whilst this was mainly developed as an API controller, more views have been added to help finish some requirements.
## Branch Management
The main branch is `Master`, and should only be committed to/pushed to when the service has been confirmed working (this includes passing tests and human testing). All pushes to `Master` will be pushed into production.  
`Develop` is where features may be added (only by means of feature branches), tests should all pass when being pushed to this branch, however some things may not work fully.  
`Feature` branches are assumed unstable, and are actively being worked on. CI does not build these branches.

# Assignment Checklist
## System Architecture 

### Peer Point Distribution
Dylan: 7  
Steven: 6

## System Implementation
### Security Topics
Due to no central Login/Accounts for the project, please view the Accounts controller in `Controllers/AccountController.cs` as this is where authentication takes place within this service. For this service, most functions have a requirement that staff authentication is required. This is because this service (particularly the views) will be used by mainly staff, and regular customers should not have access to these. You can see authentication cookies within `Startup.cs` along with the function tag `[Authorize]` with a policy of `(Policy = "StaffOnly")`, meaning only people with the staff role can gain access to the endpoint/view.

### Data Distribution
Data Distribution within this project mainly relies on making HTTP requests. Little information is duplicated across services (particularly saved to databases) to try to make data as up to date as possible. Due to this, pages may take a little longer to load (or fail to load), however these would generally occur for Staff and Admin pages, as customer pages are optimised to display as much data as they can; loading pages without some elements (like reviews).

### Network Resilience
For network resilience, `Polly` is used for creating a HTTPClient factory as a service in the project. There is only one client builder defined within `Startup.cs`, being `StandardRequest`. This contains certain properties, `StandardRequest` deals with most of the backend requests that aren't too reliant of customer pages. The main requests sent from the customer pages are retrieving `Third Party APIs` and `Products`, if these services are unable to be reached after the third try, product data will be displayed without this extra data. This makes sure all data retrieved is live and accurate.

### Tools and Frameworks
Multiple tools and frameworks are used within this project. These are listed below.
#### Language and Environment
* C# with ASPNET Core version 2.2.
* Visual Studio 2019 (with ReSharper) was used for most of the development, debugging builds were usually ran in ISS.
* IntelliJ Rider was also used as it has a very good testing interface, along with ReSharper built in.
* Visual Studio Code was used to write readme files, along with some tests to make sure the `dotnet test` functionality worked within the project.
* Windows 10 - Development Environment.
* Linux Ubuntu and Debian, including Docker.
* MSSQL Database. (Running in Docker for production).
* GitKraken was mainly used as the Git client for this project, however GitHub Desktop was also used for some commits.
* CodeFactor was used just to check code quality on pull request.

#### Testing
* Visual Studio Test Tools (Microsoft.VisualStudio.TestTools.UnitTesting). Testing can be run by going to the root directory of this project and using
>dotnet test  

* Moq is used for a HttpClient mock. This can be seen in `ThAmCo.Stock.Tests/Controllers/StockControllerTests.cs lines 75-85`. These get given data depending on the test, as can be seen at the start of the test `VendorProducts_ValidSupplierPassed_ReturnAllSupplierProducts() lines 506-518`.
* Test doubles (test data) are available within the test cs file (`ThAmCo.Stock.Tests/Controllers/StockControllerTests.cs`), and are inside of their own `Data` class. These are located at the start of the file, in `lines 26-71`. These were then used within the `ThAmCo.Stock/Data/StockContext/MockStockContext.cs` to act as the context fake.
* ARC (Advanced REST Client) and Postman were used to test API endpoints within this project. They were useful to test endpoints within my own services, along with creating the DTOs required for retrieving data from other services.

#### Continuous Integration
* Azure Pipelines running on a Windows agent. The Azure Pipelines configuration can be found in the root directory, in `azure-pipelines.yml`. A build is scheduled when a commit is pushed to the Master or Develop branch. A build is also scheduled when a pull request is made to the Develop branch, and is checked by means of a Github Status Check. This means when a pull request is made, Github requests a build from Azure Pipelines, and will now allow the branch to be merged unless the build passes.
* AppVeyor was used for a short period as an addition to Azure Pipelines, however was removed due to compatibility issues with build images.

### Configuring Deployment
* A `Dockerfile` is included within this repo as a means of deploying this service. This `Dockerfile` is configured to be used with the produced artefact from Azure Pipelines, and has not been designed to be used for debugging, building, or run directly inside of the repo. To deploy the docker image, please follow these instructions:
>Take produced artefact from Azure Pipelines (ThAmCo-Stock.zip)  
Extract to a folder called stock  
Place Dockerfile one up from the stock directory  
docker build thamco-stock .  

This should produce a docker image that can then be launched.  
`Docker-compose` would also be used when all services are available with their dockerfile. This would be the ideal way of deploying all services at once, along with their database.

## System Demonstration

### Peer Point Distribution
Steven: 7  
Dylan: 6

ThAmCo.Stock
=====
| Master | Develop | CodeFactor
|--|--|--|
| [![Build Status](https://dev.azure.com/Stedoss/ThAmCo.Stock/_apis/build/status/Don-t-Fail.ThAmCo-Stock?branchName=master)](https://dev.azure.com/Stedoss/ThAmCo.Stock/_build/latest?definitionId=1&branchName=master) | [![Build Status](https://dev.azure.com/Stedoss/ThAmCo.Stock/_apis/build/status/Don-t-Fail.ThAmCo-Stock?branchName=develop)](https://dev.azure.com/Stedoss/ThAmCo.Stock/_build/latest?definitionId=1&branchName=develop) | [![CodeFactor](https://www.codefactor.io/repository/github/don-t-fail/thamco-stock/badge)](https://www.codefactor.io/repository/github/don-t-fail/thamco-stock)



## Overview
This web service handles stock and pricing within the ThAmCo project. Whilst this was mainly developed as an API controller, more views have been added to help finish some requirements.
## Tools Used
* Visual Studio 2019 (With ReSharper)
* Visual Studio Test Suite (Microsoft.VisualStudio.TestTools.UnitTesting)
* Visual Studio Code
* Jetbrains Rider
* Azure Pipelines (Self-Hosted Windows Agent)
* ARC (Advanced REST Client)
* GitKraken
* GitHub Desktop
* CodeFactor
* Docker and Docker-Compose
## Testing
### CI
Azure Pipelines was the CI chosen for this project, and is triggered on commit and pull request to the master and develop branch. Merging to master and develop is not allowed if the solution does not build and tests do not pass.
### Test Suite
Testing is handled through the default Visual Studio test suite.
To run all the tests in the project, use 
>dotnet test


Or use the test suite within Visual Studio or Rider.
### Mocks
Mock data contexts are included as an interface; and are generally created with simple lists (LINQ makes this simple, as data contexts are treated like lists). Mock HTTP clients are formed using the Moq package.

# Assignment Checklist
## System Architecture 
### Original Architecture Diagram with Feedback

### Updated Architecture Diagram with Comments

### Solo Development Plan

### Peer Point Distribution

## System Implementation
### Security Topics

### Data Distribution

### Network Resilience

### Tools and Frameworks

### Unit Testing

### Test Doubles

### Configuring Deployment

### Deployment Video

## System Demonstration
### Component Demonstration

### Peer Point Distribution

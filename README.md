Coding-Test-July-2014
=====================

Contains the results of a coding test.  The brief for which was:

Using GitHub public API, Repositories Search 

[https://developer.github.com/v3/search/#search­repositories](https://developer.github.com/v3/search/#search­repositories)

endpoint:

[https://api.github.com/search/repositories](https://api.github.com/search/repositories)   
 
Implement in C# a working solution that 

* Search for repositories matching the query string “met” 
* Returns *all* the repositories matching the criteria 
* For each repository, only Name and Description needs to be populated in the result 
 
 
**CONTEXT**
 
* No more than N concurrent requests can be made to the GitHub api 
* per_page parameter of API is a given constant (e.g. 100) 
* Assume GitHub API responses take the same time, independent of the api parameters 
* Assume it's not considered a bad practice to query the GitHub API even if an empty 
result might be returned 
* Code­readability is more important than over­optimisation 
* Project type can be a console application or a ASP.NET MVC/WebApi 
 
**GOAL**
 
* The solution should be optimised for the overall time spent on network I/O (CPU­bound 
optimization is not relevant) 
* Consider the solution a releasable functionality 
* Should satisfy what you consider being best coding practices, with TDD in mind 
* Provide rationale behind coding decision, if relevant  

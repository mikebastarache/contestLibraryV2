**

Modern Media Contest Library - Version 2.0
----------------------------------------

**

This library contains the core business logic for all three of the Irving Tissue companies to manage their CRM and contests.  

What the library manages with the database:

 - Users
 - Minor Users
 - Contest
 - Contest Registrations
 - Minor Contest Registratiosn
 - Friends
 - Instant Wins
 - Pins
 - Provinces
 - Salutation
 - Votes


----------


Where can this library be used?
-------------------------------

This library can be installed within an ASP.net Web application running .NET 4.0 or 4.5 Framework.



----------

What classes are available to use?
----------------------------------

The logic of the library is broken up into 3 classes.

 - MicrositeLogic()
 - FacebookLogic()
 - ChildLogic()

The *MicrositeLogic()* class is to be used for a standard contest microsite.

The *FacebookLogic()* class is to be used for a Facebook application page.

The *ChildLogic()* is unique logic to allow a minor under the age of majority to participate within an application with the consent of a registered guardian.

----------

Database connection
-------------------

The library reqires that the web application has a database connection string named 'crmContext'.

    <add name="crmContext" connectionString="Data Source=SERVER;Initial Catalog=IrvingDbName;Persist Security Info=True;User ID=username;Password=password" providerName="System.Data.SqlClient" />
      


----------


How to use library
-----------------
To use the library, add the NuGet package available from the Modern Media NuGet server.  After the installation, go into the code and use the library with the following...

    using MMContest;
    using MMContest.Models;
    using MMContest.Dal;

At the beginning of class, you need to include  

**Example:**

    private readonly CrmContext _db = new CrmContext();
    private readonly UnitOfWork _unitOfWork = new UnitOfWork();


**To use the library, see example:**

    var finalist = new MicrositeLogic();
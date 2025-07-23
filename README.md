# blazor-app

## EF Migrations

To add a new migration run the following command at the root of the project (c:\git\online-shop):

> dotnet ef migrations add <insert name> --startup-project Api --output-dir Data/Migrations
>
> dotnet ef migrations add InitialCreate --startup-project Api --output-dir Data/Migrations

Once create run the following command to apply the changes from within the /Api directory:

> dotnet ef database update --project Api

### Pre-requisities

Install dotnet CLI & ef CLI.  The following command installs the ef CLI.

> dotnet tool install --global dotnet-ef

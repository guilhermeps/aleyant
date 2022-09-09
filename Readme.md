# Aleyant category manager API

This API provides some resources to manage a list of categories of a printing company. This is an ongoing project.

## Technologies

- Dapper ORM
- .Net 6
- SQL Server 2022 preview

## Decisions and it's drawbacks

I decided to work with a not normalized table to achieve faster data input. It got some drawbacks on how to delete and update categories. The duplicity was not avoided on insertion, but on the tree building process using `HashSet`data structure which provides an O(1) time complexity on those operations. To build the tree structure we have an O(N) time complexity.

There are some automated tests, mostly unit tests to ensure some validations.

Integration tests were not built, specially to respect the 4 hours rule.

## Running from source

To run this API you must, at this point, you must create a local database in you machine or running in a docker container. You may run SQL Server from docker using the following command:

```sh
sudo docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=Gu1lh3rm3"    -p 1433:1433 --name sql1 --hostname sql1    -d    mcr.microsoft.com/mssql/server:2022-latest
```
To check if the container is running you should look at the STATUS column after running the following command:

```sh
sudo docker ps -a
```

Once the database container is running you need to create a not normalized table called `category` with two columns `categoryName`and `childCategoryName`.

After that you are ready to go running the minimal API running the following command from `src/AleyantCategoryManager.Api` folder:

```sh
dotnet run
```

The runner will provide an URL to access the endpoints. There is a swagger interface that allows you to interact with the API, you just need to point to the route `/swagger`.

## Running tests

To run all aumated tests you can run the `dotnet test`directly from `src` folder.


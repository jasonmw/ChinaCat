## Create the database and a user in postgres



*this is probably inefficient and an improper use of the principal of least privelege for the database user, but this is just a demo, so ... deal*

```SQL
CREATE DATABASE ChinaCat;

CREATE ROLE chinacat_sa WITH login "pa$$w0rd"

ALTER ROLE chinacat_sa WITH LOGIN;

GRANT CREATE ON DATABASE ChinaCat to chinacat_sa;

GRANT ALL PRIVILEGES on ChinaCat to chinacat_sa;

GRANT ALL on schema public to chinacat_sa;
```

## in the application

in the appsettings.json ensure the connection string points to your postgres instance

go into program.cs and find where the admin user is created and change the email and password to something you want to use.

then run the dotnet ef database migrate to create the database








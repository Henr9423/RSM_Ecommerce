# \## Configuration



The application requires configuration for the database, email service, JWT authentication, frontend origin, and local development ports.



An example Docker environment configuration is provided in .env.example.



##### \### Docker configuration



Create your local .env file from the example:





cp .env.example .env





On Windows PowerShell, you can use:



Copy-Item .env.example .env





Update `.env` with your local values and secrets, especially:



env

DB\_USER=your\_database\_username

DB\_PASSWORD=your\_database\_password

RESEND\_API\_KEY=your\_resend\_api\_key

GUEST\_JWT\_SECRET=your\_secret\_key

PGADMIN\_PASSWORD=your\_pgadmin\_password





The .env file must not be committed to source control.



The .env.example file contains only example/default values and should remain committed so developers can see which environment variables are required.



When the backend runs inside Docker, PostgreSQL is reached through the Docker network using:



env

DB\_HOST=db

DB\_PORT=5432





Start the development environment with:



docker compose up --build



##### \### Running the backend locally



When running the ASP.NET backend directly on your machine instead of inside Docker, use .NET User Secrets for sensitive development configuration.



If User Secrets have not already been initialized for the API project:



dotnet user-secrets init --project rsm\_backend.Api





Configure the local database connection:





dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Host=localhost;Port=5433;Database=rsm\_ecommerce;Username=YOUR\_DATABASE\_USERNAME;Password=YOUR\_DATABASE\_PASSWORD" --project rsm\_backend.Api





Configure the Resend API key:





dotnet user-secrets set "Resend:ApiKey" "YOUR\_RESEND\_API\_KEY" --project rsm\_backend.Api





Configure the JWT secret:



dotnet user-secrets set "GuestJwt:Secret" "YOUR\_GUEST\_JWT\_SECRET" --project rsm\_backend.Api





When PostgreSQL is running through Docker Compose but the backend is running locally, use:



Host: localhost

Port: 5433



When both the backend and PostgreSQL are running through Docker Compose, Docker uses:



Host: db

Port: 5432



Run the backend locally with:



dotnet run --project rsm\_backend.Api

##### 

##### \### Running tests



Run all test projects from the backend solution directory with:



dotnet test



Integration and acceptance tests use Testcontainers to create isolated PostgreSQL containers automatically, so the normal development database does not need to be used for those tests.




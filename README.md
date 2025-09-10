# BookLendingService

What you can do (requirements)

- Add a book – POST /books

- List books – GET /books?showAll=false
By default returns only available books.
If showAll=true, returns all books.

- Check out a book – POST /books/{id}/checkout

- Return a book – POST /books/{id}/return

- Health check – GET /health

- Swagger/OpenAPI in Development – /swagger

- Uses SQLite for storage (EF Core), controllers (no minimal endpoints), and DI with a small service layer.

Endpoints:

- Add a book POST /books
      {
        "title": "The Pragmatic Programmer",
        "author": "Hunt & Thomas",
        "isbn": "9780201616224"
      }
  201 Created -> the created book
  400 if validation fails

- List books GET /books?showAll=false
  - Default showAll=false -> only available.
  - showAll=true -> all books.
  - 200 OK -> list sorted by Title.

- Check out POST /books/{id}/checkout
  200 OK -> book becomes isAvailable: false
  404 if not found
  409 if already checked out

- Return POST /books/{id}/return
  200 OK → book becomes isAvailable: true
  404 if not found
  409 if already available

Run in Docker
docker build -t booklend-api:local .
docker run --rm -p 8080:8080 --name booklend booklend-api:local

Deploy later (Terraform on AWS)
terraform/ contains a simple ECS Fargate + ALB setup.
Once you have an image in ECR:
cd terraform
terraform init
terraform apply -var "region=eu-central-1" -var "image_url=<ACCOUNT>.dkr.ecr.eu-central-1.amazonaws.com/<repo>:<tag>"

Nice to have in future:
- Pagination for GET /books
- Concurrency token to guard double checkout
- Logging

start:
	dotnet watch --launch-profile dev

build:
	dotnet build

test:
	dotnet test

docker:
	docker build -t art-tattoo-be -f Dockerfile .

deploy:
	docker-compose up -d

rebuild:
	docker-compose up --no-deps --build webapi

new-migration:
	dotnet ef migrations add $(name) --verbose

migration:
	dotnet ef database update

drop-database:
	dotnet ef database drop

.PHONY: start build docker new-migration migration test drop-database
start:
	dotnet watch

build:
	dotnet build

test:
	dotnet test

docker:
	docker build -t art-tattoo-be -f Dockerfile .

new-migration:
	dotnet ef migrations add $(name)

migration:
	dotnet ef database update

drop-database:
	dotnet ef database drop

.PHONY: start build docker new-migration migration test drop-database
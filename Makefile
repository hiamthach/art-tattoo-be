start:
	dotnet watch

build:
	dotnet build

docker:
	docker build -t art-tattoo-be -f Dockerfile .

new-migration:
	dotnet ef migrations add $(name)

migration:
	dotnet ef database update

.PHONY: start build docker new-migration migration
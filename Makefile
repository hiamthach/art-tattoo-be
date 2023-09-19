start:
	dotnet watch

build:
	dotnet build

docker:
	docker build -t art-tattoo-be -f Dockerfile .

.PHONY: start build docker
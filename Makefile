start:
	dotnet watch

docker:
	docker build -t art-tattoo-be -f Dockerfile .

.PHONY: start
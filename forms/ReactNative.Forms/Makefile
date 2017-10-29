DOTNET=dotnet
MSBUILD=msbuild

all: pack

build:
	$(DOTNET) restore
	$(MSBUILD) /p:Configuration=Release /t:Clean /t:Build

pack:
	$(MAKE) build
	$(DOTNET) pack --no-build -c release

clean:
	rm -rf bin/ obj/

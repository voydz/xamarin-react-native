NUGET=nuget
MSBUILD=msbuild
SOLUTION=ReactNative.Forms.Droid.csproj
NUSPEC=ReactNative.Forms.Droid.nuspec

all: nuget

build:
	$(MSBUILD) $(SOLUTION) /t:Restore
	$(MSBUILD) $(SOLUTION) /p:Configuration=Release /t:Clean /t:Build

nuget:
	$(MAKE) build
	$(NUGET) pack $(NUSPEC)
	mv *.nupkg bin/

clean:
	rm -rf bin/ obj/

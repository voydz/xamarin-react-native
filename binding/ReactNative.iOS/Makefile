NUGET=nuget
MSBUILD=msbuild
SOLUTION=ReactNative.iOS.csproj
NUSPEC=ReactNative.iOS.nuspec
DBG_NUSPEC=ReactNative.iOS.Debug.nuspec

all: nuget

build:
	./buildLibs.sh $(CONFIGURATION)
	$(MSBUILD) $(SOLUTION) /t:Restore
	$(MSBUILD) $(SOLUTION) /p:Configuration=$(CONFIGURATION) /t:Clean /t:Build

nuget:
	$(MAKE) build CONFIGURATION=Release
	$(NUGET) pack $(NUSPEC)
	$(MAKE) build CONFIGURATION=Debug
	$(NUGET) pack $(DBG_NUSPEC)
	mv *.nupkg bin/

clean:
	rm -rf *.a bin/ obj/

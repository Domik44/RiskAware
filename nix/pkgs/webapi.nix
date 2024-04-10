{pkgs, lib, sourceInfo, ...}:
with pkgs;
  buildDotnetModule rec {
    pname = "RiskAware.Server";
    version = lib.strings.fileContents ../../version;

    src = ../../sources;

    projectFile = "./${pname}/${pname}.csproj"; # path to csproj or sln to build
    nugetDeps = ../deps.nix;

    dotnet-sdk = dotnetCorePackages.sdk_8_0;
    dotnet-runtime = dotnetCorePackages.aspnetcore_8_0;

    runtimeDeps = [
      # add any native deps here
    ];

    buildInputs = [
      nodejs_18
    ];

    useAppHost = false;

    postFixup = ''
      wrapProgram $out/bin/${meta.mainProgram} --chdir $out/lib/${pname}
    '';

    meta.mainProgram = pname;

    passthru = {
      devShell = mkShell {
        inputsFrom = [RiskAwareUnwrapped];
        shellHook = ''
          export DOTNET_ROOT=${RiskAwareUnwrapped.dotnet-runtime}
          export LD_LIBRARY_PATH="${RiskAwareUnwrapped.dotnet-sdk.icu}/lib:${lib.makeLibraryPath RiskAwareUnwrapped.runtimeDeps}"
          unset DOTNET_SKIP_FIRST_TIME_EXPERIENCE
        '';
      };
    };
  }

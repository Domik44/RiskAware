{
  self',
  pkgs,
  ...
}:
with pkgs; let
  RiskAware = buildDotnetModule rec {
    pname = "RiskAware";
    version =  "0.1";# lib.strings.fileContents ../../version;

    src = self'.packages.source;

    projectFile = "${pname}.sln"; # path to csproj or sln to build
    nugetDeps = ../deps.nix;

    dotnet-sdk = with dotnetCorePackages;
      (combinePackages [
        sdk_8_0
      ])
      .overrideAttrs {
        allowSubstitutes = true;
        preferLocalBuild = false;
      };
    dotnet-runtime = with dotnetCorePackages;
      (combinePackages [
        aspnetcore_8_0
      ])
      .overrideAttrs {
        allowSubstitutes = true;
        preferLocalBuild = false;
      };

    runtimeDeps = [ ];

    useAppHost = false;

    buildPhase = ''
      dotnet build ${projectFile} -maxcpucount:1 -p:BuildInParallel=false -p:ContinuousIntegrationBuild=true -p:Deterministic=true --configuration Release --no-restore -p:InformationalVersion=${version} -p:Version=${version}.0
    '';

    installPhase = ''
      mkdir -p $out # ignores outputs
    '';

    dontDotnetCheck = true;
    dontFixup = true;

    passthru = {
      devShell = mkShell {
        inputsFrom = [RiskAware];
        nativeBuildInputs = with pkgs; [
          powershell
          playwright-driver.browsers
        ];
        shellHook = ''
          export DOTNET_ROOT=${RiskAware.dotnet-runtime}
          export LD_LIBRARY_PATH="${RiskAware.dotnet-sdk.icu}/lib:${lib.makeLibraryPath RiskAware.runtimeDeps}"
          unset DOTNET_SKIP_FIRST_TIME_EXPERIENCE
        '';
      };
    };
  };
in
  RiskAware

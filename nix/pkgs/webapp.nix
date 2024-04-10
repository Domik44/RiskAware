{pkgs, lib, dream2nix, ...}:
with pkgs;
  buildNpmPackage rec {
    pname = "riskaware.client";
    version = lib.strings.fileContents ../../version;

    nodejs = nodejs_18;

    # The code sources for the package
    src = ../../sources/${pname};
    npmDepsHash = "sha256-BNHGVdg9zCGtOZYTqm16Z1cLc3979wgbUPVwW7NO2+I=";

    # The packages required by the build process
    buildInputs = with pkgs; [
    ];

    postPatch = ''
        cp ${src}/package-lock.json package-lock.json
      '';

    passthru = {
      devShell = mkShell {
        inputsFrom = [RiskAwareUnwrapped];
        shellHook = ''
        '';
      };
    };

  }

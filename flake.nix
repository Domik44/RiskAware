{
  inputs = {
    nixpkgs.url = "github:NixOS/nixpkgs/nixos-23.11";
    flake-parts.url = "github:hercules-ci/flake-parts";
    haumea.url = "github:nix-community/haumea/v0.2.2";
    process-compose-flake.url = "github:Platonic-Systems/process-compose-flake";
    nuget-config.url = "github:nesfit/nixpkgs/fetchnuget-support-nuget.config";
  };

  outputs = {self, ...} @ inputs: let
    lib = inputs.nixpkgs.lib;
    h = inputs.haumea.lib;
  in
    inputs.flake-parts.lib.mkFlake {inherit inputs;} ({config, ...}: {
      imports = [
        inputs.process-compose-flake.flakeModule
        (inputs.flake-parts.lib.mkTransposedPerSystemModule {
          name = "bundlers";
          option = lib.mkOption {
            type = lib.types.anything;
            default = {};
          };
          file = "bundlers.nix";
        })
      ];

      systems = ["x86_64-linux" "aarch64-darwin"];

      flake = {
        inherit lib;
      };

      perSystem = {
        self',
        inputs',
        pkgs,
        system,
        dream2nix,
        ...
      }: {
        formatter = pkgs.alejandra;

        apps = h.load {
          src = ./nix/apps;
          loader = h.loaders.default;
          inputs = {inherit self' inputs' pkgs;};
        };

        packages = h.load {
          src = ./nix/pkgs;
          inputs = {inherit self' inputs' pkgs; inherit (self) sourceInfo lib; inherit dream2nix; };

        };

        devShells = {
          default = self'.packages.default.devShell;
        };

        process-compose = h.load {
          src = ./nix/processes;
          loader = h.loaders.default;
          inputs = {inherit pkgs inputs' self';};
        };

        # bundlers = {
        #   docker = {...} @ drv: (pkgs.dockerTools.buildLayeredImage {
        #     name = drv.name or drv.pname or "image";
        #     tag = "latest";
        #     contents =
        #       if drv ? outPath
        #       then drv
        #       else throw "provided installable is not a derivation and not coercible to an outPath";
        #     config = {
        #       Env = ["DOTNET_EnableDiagnostics=0"];
        #       Cmd = ["${drv.outPath}/bin/${drv.meta.mainProgram}"];
        #     };
        #   });
        # };        
      };
    });
}

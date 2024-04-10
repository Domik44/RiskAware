{
  self',
  pkgs,
  ...
}: let
  port = 8081;
  command = pkgs.writeShellScriptBin "webapp" ''
    ${pkgs.static-web-server}/bin/static-web-server -p ${toString port} -d ${self'.packages.webapp} --page-fallback "${self'.packages.webapp}/index.html"
  '';
  readinessProbe = pkgs.writeShellScript "app-ready.sh" ''
    ${pkgs.lib.getExe pkgs.curl} -sf localhost:${toString port}
  '';
in {
  inherit command;
  environment = [
  ];
  depends_on = {
    webapi.condition = "process_healthy";
  };
  readiness_probe = {
    exec.command = "${readinessProbe}";
    initial_delay_seconds = 5;
    period_seconds = 10;
    failure_threshold = 3;
  };
}

{
  self',
  pkgs,
  ...
}: let
  port = 5126;
  readinessProbe = pkgs.writeShellScript "api-ready.sh" ''
    ${pkgs.lib.getExe pkgs.curl} -sf localhost:${toString port}/health
  '';
in {
  command = pkgs.lib.getExe self'.packages.webapi;
  environment = [
    "ASPNETCORE_URLS=http://127.0.0.1:${toString port}"
  ];
  depends_on = {
    db.condition = "process_healthy";
  };
  readiness_probe = {
    exec.command = "${readinessProbe}";
    initial_delay_seconds = 2;
    period_seconds = 2;
    failure_threshold = 20;
  };
}

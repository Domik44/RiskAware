{
  pkgs,
  sourceInfo,
  ...
}:
with pkgs;
  runCommand
  "csharpSource"
  {__contentAddressed = true;}
  ''
    mkdir $out
    cp -r ${sourceInfo.outPath}/sources/.config $out
    cp -r ${sourceInfo.outPath}/sources/RiskAware.Server $out
    cp -r ${sourceInfo.outPath}/sources/riskaware.client $out
    cp ${sourceInfo.outPath}/sources/RiskAware.sln $out
    cp ${sourceInfo.outPath}/sources/riskaware.client/package.json
    cp ${sourceInfo.outPath}/sources/riskaware.client/package-lock.json
  ''

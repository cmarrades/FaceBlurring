$solutionToBuild="..\sources\CMarrades.Faceblurring.sln"
$isBuildServer = ($env:BUILD_NUMBER)

& $PSScriptRoot\tools\nuget.exe restore $PSScriptRoot\$solutionToBuild -configFile $PSScriptRoot\nuget.config

$msbuildPath = "C:\Program Files (x86)\MSBuild\14.0\Bin\msbuild.exe"		

& $msbuildPath $PSScriptRoot\$solutionToBuild /t:Rebuild /p:Configuration=Release 
 
#This build assumes the following directory structure
#
#  \Build          - This is where the project build code lives
#  \BuildArtifacts - This folder is created if it is missing and contains output of the build
#  \Code           - This folder contains the source code or solutions you want to build
#
Properties {
	$build_dir = Split-Path $psake.build_script_file	
	$build_artifacts_dir = "$build_dir\..\BuildArtifacts\"
	$nuget_dir = "$build_dir\..\Nuget\"
	$code_dir = "$build_dir\..\Source"
}

FormatTaskName (("-"*25) + "[{0}]" + ("-"*25))

Task Default -Depends Entile

Task Entile -Depends Clean, Build, RunTests, NuGet

Task Build -Depends Clean {	
	Write-Host "Building Entile.sln" -ForegroundColor Green
	Exec { msbuild "$code_dir\Entile.sln" /t:Build /p:Configuration=Release /v:quiet /p:OutDir=$build_artifacts_dir } 
}

Task RunTests -Depends Build {	
	Write-Host "Running tests" -ForegroundColor Green
	Exec { msbuild "$build_dir\run-tests.proj" /t:Test /p:OutputPath=$build_artifacts_dir } 
}

Task NuGet -Depends Build {
	Write-Host "Creating NuGet package" -ForegroundColor Green

	.\NuGet.exe pack $nuget_dir\entile-server.nuspec
}

Task Clean {
	Write-Host "Creating BuildArtifacts directory" -ForegroundColor Green
	if (Test-Path $build_artifacts_dir) 
	{	
		rd $build_artifacts_dir -rec -force | out-null
	}
	
	mkdir $build_artifacts_dir | out-null
	
	Write-Host "Cleaning Entile.sln" -ForegroundColor Green
	Exec { msbuild "$code_dir\Entile.sln" /t:Clean /p:Configuration=Release /v:quiet } 
}

$scriptPath = Split-Path $MyInvocation.InvocationName
Remove-Module psake
Import-Module (join-path $scriptPath '..\Source\packages\psake.4.0.1.0\tools\psake.psm1')
invoke-psake -framework '4.0' (join-path $scriptPath 'default.ps1')

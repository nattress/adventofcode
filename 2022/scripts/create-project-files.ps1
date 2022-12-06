#
# Sets up the folder structure and project files for Advent Of Code 2022
#

$currentYearSourceRoot = $PSScriptRoot | Split-Path -Parent

Write-Output "Creating projects under $currentYearSourceRoot"

for ($i = 1; $i -lt 26; $i++)
{
    $dayFolder = $i.ToString().PadLeft(2, '0');
    $daySourceRoot = Join-Path -Path $currentYearSourceRoot -ChildPath $dayFolder
    Write-Output $daySourceRoot

    New-Item -Path $daySourceRoot -ItemType Directory
    Set-Location $daySourceRoot
    dotnet new console

    $templateProgram = Join-Path -Path $currentYearSourceRoot -ChildPath "Program.cs"
    $templateInput = Join-Path -Path $currentYearSourceRoot -ChildPath "input_01.txt"
    Copy-Item -Path $templateProgram -Destination $daySourceRoot -Force
    Copy-Item -Path $templateInput -Destination $daySourceRoot -Force

    Set-Location $PSScriptRoot
}

#!/bin/bash

NugetApiKey=$1
NugetSource=$2
PackagesApiKey=$3
PackagesSource=$4

cd Romanization || exit

buildPath="bin/Release"

cp "./Romanization.nuspec" "$buildPath/Romanization.nuspec"

echo "Packing built library..."
dotnet pack ./Romanization.csproj --configuration Release

cd "$buildPath" || exit

files=( Romanization.NET.*.nupkg )
echo "${files[0]}"

echo "Pushing packed package to Nuget..."
dotnet nuget push "${files[0]}" --api-key "$NugetApiKey" --source "$NugetSource" --skip-duplicate
echo "Pushing packed package to GitHub Packages..."
dotnet nuget push "${files[0]}" --api-key "$PackagesApiKey" --source "$PackagesSource" --skip-duplicate

echo "Done!"

#!/bin/bash

NugetApiKey=$1
NugetSource=$2
PackagesApiKey=$3
PackagesSource=$4

cd Romanization

buildPath="bin/Release"

cp "./Romanization.nuspec" "$buildPath/Romanization.nuspec"

echo "Packing built library..."
dotnet pack ./Romanization.csproj --configuration Release

cd "$buildPath"

files=( Romanization.NET.*.nupkg )
echo "${files[0]}"

echo "Pushing packed package to Nuget..."
dotnet nuget push "${files[0]}" --api-key "$NugetApiKey" --source "$NugetSource"
echo "Pushing packed package to GitHub Packages..."
dotnet nuget push "${files[0]}" --api-key "$PackagesApiKey" --source "$PackagesSource"

echo "Done!"
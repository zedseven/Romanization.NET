NugetApiKey=$1
NugetSource=$2
PackagesApiKey=$3
PackagesSource=$4

cd Romanization

#echo "Downloading the latest nuget.exe..."
#curl https://dist.nuget.org/win-x86-commandline/latest/nuget.exe --output ./nuget.exe -s
#chmod +x ./nuget.exe

echo "Packing built library..."
dotnet pack ./Romanization.nuspec --configuration Release

pattern="Romanization.NET.*.nupkg"
files=( $pattern )
echo "${files[0]}"

echo "Pushing packed package to Nuget..."
dotnet nuget push "${files[0]}" --api-key "$NugetApiKey" --source "$NugetSource"
echo "Pushing packed package to GitHub Packages..."
dotnet nuget push "${files[0]}" --api-key "$PackagesApiKey" --source "$PackagesSource"

echo "Done!"
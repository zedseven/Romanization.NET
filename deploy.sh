NugetApiKey=$1
NugetSource=$2
PackagesApiKey=$3
PackagesSource=$4

cd Romanization

#echo "Downloading the latest nuget.exe..."
#curl https://dist.nuget.org/win-x86-commandline/latest/nuget.exe --output ./nuget.exe -s
#chmod +x ./nuget.exe

echo "Packing built library..."
dotnet pack ./Romanization.csproj --configuration Release --verbosity detailed

echo "Pushing packed package to Nuget..."
dotnet nuget push ./Romanization.NET.*.nupkg --api-key "$NugetApiKey" --source "$NugetSource"
echo "Pushing packed package to GitHub Packages..."
dotnet nuget push ./Romanization.NET.*.nupkg --api-key "$PackagesApiKey" --source "$PackagesSource"

echo "Done!"
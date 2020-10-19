NugetApiKey=$1
NugetSource=$2
PackagesApiKey=$3
PackagesSource=$4

cd Romanization

echo "Packing built library..."
nuget pack ./Romanization.csproj -Prop Configuration=Release -Verbosity detailed

echo "Pushing packed package to Nuget..."
nuget push ./Romanization.NET.*.nupkg -Verbosity detailed -ApiKey $NugetApiKey -Source $NugetSource
echo "Pushing packed package to GitHub Packages..."
nuget push ./Romanization.NET.*.nupkg -Verbosity detailed -ApiKey $PackagesApiKey -Source $PackagesSource

echo "Done!"
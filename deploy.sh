NugetApiKey=$1
NugetSource=$2
PackagesApiKey=$3
PackagesSource=$4

cd Romanization

echo "Downloading the latest nuget.exe..."
curl https://dist.nuget.org/win-x86-commandline/latest/nuget.exe --output ./nuget.exe -s
chmod +x ./nuget.exe

echo "Packing built library..."
./nuget.exe pack ./Romanization.csproj -Prop Configuration=Release -Verbosity detailed

echo "Pushing packed package to Nuget..."
./nuget.exe push ./Romanization.NET.*.nupkg -Verbosity detailed -ApiKey "$NugetApiKey" -Source "$NugetSource"
echo "Pushing packed package to GitHub Packages..."
./nuget.exe push ./Romanization.NET.*.nupkg -Verbosity detailed -ApiKey "$PackagesApiKey" -Source "$PackagesSource"

echo "Done!"
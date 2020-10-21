#!/bin/bash

# https://stackoverflow.com/a/33125422/6003488

GitHubApiKey=$1

mkdir ./Documentation/generated
cd ./Documentation/generated
git init
git checkout -b gh-pages

cd ../..
mono ./docfx/docfx.exe ./Documentation/docfx.json --force

cd ./Documentation/generated
git add .
git -c user.name='travisci' -c user.email='travisci' commit -m "Updating Pages with updated documentation."

echo "Pushing to gh-pages..."
git push -f -q https://zedseven:$GitHubApiKey@github.com/zedseven/Romanization.NET gh-pages &>/dev/null

echo "Done!"

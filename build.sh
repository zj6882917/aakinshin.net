#!/bin/bash

if [ "$TRAVIS_PULL_REQUEST" == "false" ]; then

	#Set git user
	git config --global user.email "andrey.akinshin@gmail.com"
	git config --global user.name "Andrey Akinshin"

	echo -e "Cloning gh-pages\n"
	git clone -b gh-pages https://${GH_TOKEN}@github.com/AndreyAkinshin/aakinshin.net.git site
	git clean -x -f -d
	git rm -r -f site/*
	cp .gitignore site/.gitignore

	echo -e "Building Jekyll site\n"
	jekyll build -d site

	cd site

	touch .nojekyll
	echo "aakinshin.net" > CNAME

	echo -e "Committing site\n"
	git add -A
	git commit -m "Travis build $TRAVIS_BUILD_NUMBER"
	git push --quiet origin gh-pages > /dev/null 2>&1

        cd ..
	rm -rf site

fi


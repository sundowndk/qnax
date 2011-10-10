#!/bin/bash
#
# Usage: build.sh [outputdirectory]

####################################################
# INIT                                             #
####################################################
BASEDIR=$(dirname "$1")
OUTPUTDIR="$1"

####################################################
# CLEAN                                            #
####################################################
echo "Cleaning previous build..."
rm "$OUTPUTDIR/html/qnax/" -r
rm "$OUTPUTDIR/cgi-bin/Content/qnax/" -r

####################################################
# SETUP                                            #
####################################################
echo "Setting up build structur..."
mkdir "$OUTPUTDIR/html/qnax/"
mkdir "$OUTPUTDIR/html/qnax/js"

####################################################
# JS                                               #
####################################################
echo "Building JAVASCRIPT..."
jsbuilder qnax.jsb "$OUTPUTDIR/html/qnax/js/"

####################################################
# HTML                                             #
####################################################
echo "Building HTML..."
for file in html/*; do
echo $file
    cp -rv $file "$OUTPUTDIR/html/"
done

####################################################
# CONTENT                                          #
####################################################
echo "Building CONTENT..."
for file in content/*; do
    cp -rv $file "$OUTPUTDIR/cgi-bin/Content/"
done



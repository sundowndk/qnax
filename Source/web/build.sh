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
rm "$OUTPUTDIR/cgi-bin/Lib/data/qnax/content/" -r

####################################################
# SETUP                                            #
####################################################
echo "Setting up build structur..."
mkdir "$OUTPUTDIR/html/qnax/"
mkdir "$OUTPUTDIR/html/qnax/js"

####################################################
# CGI-BIN                                          #
####################################################
echo "Building 'cgi-bin'..."
for file in cgi-bin/*; do
    cp -rv $file "$OUTPUTDIR/cgi-bin/"
done

####################################################
# HTML                                             #
####################################################
echo "Building 'html'..."
for file in html/*; do
echo $file
    cp -rv $file "$OUTPUTDIR/html/"
done

####################################################
# JAVASCRIPT                                       #
####################################################
echo "Building 'javascript'..."
jsbuilder javascript.jsb "$OUTPUTDIR/html/qnax/js/"

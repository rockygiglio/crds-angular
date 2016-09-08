# Group Resources Migration Tool

## Overview
The purpose of this utility is to import the static JSON data from the "V1" Group Resources page,
and provide some scripted output in order to facilitate migrating Group Resources into a target
SilverStripe CMS environment.

## Requirements
This script needs nodejs and npm. 

## Usage
First, run `npm install` to get the required node modules installed.  Once complete, run `npm run start` - this will result in two output files:

1. yyyymmdd_HHMMss_USnnnn-DownloadGroupResources.sh
 * This is a shell script to run curl commands to download all images and PDFs for all group resources.
2. yyyymmdd_HHMMss_USnnnn-InsertGroupResources.sql
 * This file can be used to insert all group resources into a target SilverStripe CMS environment.

## Now What?
Once you have the scripts output from above, you can begin the process to import to CMS.

1. Create a new directory and run the DownloadGroupResources.sh script in the new directory
2. Log in to the target CMS environment
3. Verify that there is a "Group Resources" navigation
 * If there is not, it may mean that the appropriate CMS code has not been deployed to the environment yet, or possibly /dev/build has not been run
4. Navigate to "Files & Images"
 * If "groupresources" folder does not exist, click "Add Folder", type "groupresources", and click "Ok"
 * Navigate to "groupresources"
 * If "images" folder does not exist under "groupresources", click "Add Folder", type "images", and click "Ok"
 * If "pdf" folder does not exist under "groupresources", click "Add Folder", type "pdf", and click "Ok"
5. Navigate to groupresources/images
6. Click "Upload" and choose all of the JPG files that were downloaded in step 1
 * Wait for all files to be uploaded to 100%
7. Navigate to groupresources/pdf
 * Wait for all files to be uploaded to 100%
8. Click "Upload" and choose all of the PDF files that were downloaded in step 1
9. Log in to the SS_mysite database for the CMS (using MySql workbench or similar)
10. Run the InsertGroupResources.sql script

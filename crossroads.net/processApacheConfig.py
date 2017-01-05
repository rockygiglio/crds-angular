#!/usr/bin/python3
# -*- coding: utf-8 -*-

# template file is inside app folder of crossroads.net
# python script will be inside crossroads.net
# file generated based on template will be inside crossroads.net

import string
import os
import sys
import getopt
from os.path import join

TEMPLATE_FILE_DIR = "app"
APACHE_CONFIG_TEMPLATE_FILE = "crossroads.net.conf.template.xml"

DISABLE_REDIRECT = True

def processCmdLineArgs(args):
    global DISABLE_REDIRECT
    exitCode = 0
    try:
        opts, args = getopt.getopt(args, "o:", ["redirect", "output="])
    except:
        exitCode = 1

    print(opts)

    for opt, arg in opts:
        if opt in ("-o", "--output"):
            print("output ", arg)
            output = arg
        elif opt == "--redirect":
            DISABLE_REDIRECT = False
            print(DISABLE_REDIRECT)

    if "output" not in locals():
        output = ""
        exitCode = 1

    return (exitCode, output)

def processTemplateFile(inputFile, outputFile):
    with open(join(os.getcwd(), TEMPLATE_FILE_DIR, inputFile), 'r') as templateFile:
        templateStr = string.Template(templateFile.read())
    
    print("Writing to {}".format(join(os.getcwd(), outputFile)))
    
    with open(join(os.getcwd(), outputFile), 'w') as outputConfigFile:
        placeHolderVars = os.environ
        if not DISABLE_REDIRECT:
            placeHolderVars["CMT"] = ""
        else:
            placeHolderVars["CMT"] = "#"
        outputConfigFile.write(templateStr.safe_substitute(placeHolderVars))

def main(args):
    exitCode, output = processCmdLineArgs(args)

    if exitCode != 0:
        print("[ERROR]: insufficient parameters or no parameters provided")
        print("[USAGE]: python3 processApacheConfig.py --redirect<optional> -o <outputFileName>")
        sys.exit(exitCode)
    else:
        processTemplateFile(APACHE_CONFIG_TEMPLATE_FILE, output)

if __name__ == "__main__":
    main(sys.argv[1:])

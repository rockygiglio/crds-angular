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

def processCmdLineArgs(args):
    exitCode = 0
    try:
        opts, args = getopt.getopt(args, "o:", ["output="])
    except:
        exitCode = 1

    print(opts)

    for opt, arg in opts:
        if opt in ("-o", "--output"):
            print("output ", arg)
            output = arg

    if "output" not in locals():
        output = ""
        exitCode = 1

    return (exitCode, output)

def processTemplateFile(inputFile, outputFile):
    with open(join(os.getcwd(), TEMPLATE_FILE_DIR, inputFile), 'r') as templateFile:
        templateStr = string.Template(templateFile.read())
    
    print("Writing to {}".format(join(os.getcwd(), outputFile)))
    
    with open(join(os.getcwd(), outputFile), 'w') as outputConfigFile:
        outputConfigFile.write(templateStr.safe_substitute(os.environ))

def main(args):
    exitCode, output = processCmdLineArgs(args)

    if exitCode != 0:
        print("[ERROR]: insufficient parameters or no parameters provided")
        print("[USAGE]: python3 processApacheConfig.py -o <outputFileName>")
        sys.exit(exitCode)
    else:
        processTemplateFile(APACHE_CONFIG_TEMPLATE_FILE, output)

if __name__ == "__main__":
    main(sys.argv[1:])

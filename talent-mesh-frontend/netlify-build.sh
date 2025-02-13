#!/bin/bash

# Build the Angular app
npm run build

# Ensure _redirects file exists in the output
echo "/* /index.html 200" > dist/frontend/_redirects

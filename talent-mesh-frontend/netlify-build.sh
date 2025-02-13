#!/bin/bash

# Install dependencies
npm install

# Build the Angular app
npm run build

# Ensure _redirects file exists in the output directory
mkdir -p dist/frontend
echo "/* /index.html 200" > dist/frontend/_redirects

# Copy netlify.toml to the output directory
cp netlify.toml dist/frontend/

module.exports = {
  onPreBuild: async ({ utils }) => {
    console.log('Adding SPA redirect...');
  },
  onBuild: async ({ utils }) => {
    console.log('Ensuring _redirects is in place...');
    const fs = require('fs');
    const path = require('path');
    
    // Ensure _redirects exists in the publish directory
    const publishDir = 'dist/frontend';
    const redirectsPath = path.join(publishDir, '_redirects');
    
    if (!fs.existsSync(redirectsPath)) {
      fs.writeFileSync(redirectsPath, '/*    /index.html   200\n');
      console.log('Created _redirects file');
    }
  }
};

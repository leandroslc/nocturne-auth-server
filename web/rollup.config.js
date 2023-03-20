import fs from 'fs';
import path from 'path';
import config from './config/config';
import sass from './config/sass-build';
import typescript from './config/typescript-build';
import copy from './config/copy-build';

const isProduction = process.env.NODE_ENV === 'production';

const modules = [
  {
    name: 'main',
    path: './src/index.ts',
    output: '',
  },
  ...fs.readdirSync(path.resolve('./src/apps')).reduce((result, app) => {
    const appPath = path.resolve('./src/apps', app);

    if (!fs.lstatSync(appPath).isDirectory()) {
      return result;
    }

    fs.readdirSync(appPath).forEach((module) => {
      const modulePath = path.resolve(appPath, module);

      if (module === 'index.ts') {
        result.push({
          name: `${app}`,
          path: modulePath,
          output: app,
        });
        return;
      }

      const entrypoint = path.join(modulePath, 'index.ts');

      if (fs.existsSync(entrypoint)) {
        result.push({
          name: `${app}-${module}`,
          path: entrypoint,
          output: app,
        });
      }
    });

    return result;
  }, []),
];

const css = sass({
  bundleName: config.projectName,
  name: 'main',
  input: './scss/index.scss',
  watch: './scss/**/*.scss',
  production: isProduction,
});

const loginCss = sass({
  bundleName: config.projectName,
  name: 'login',
  input: './scss/login.scss',
  watch: './scss/**/*.scss',
  production: isProduction,
});

const allJs = modules.map((module) =>
  typescript({
    bundleName: config.projectName,
    output: module.output,
    name: module.name,
    input: module.path,
    watch: './src/**/*.ts',
    production: isProduction,
  }),
);

const imgs = copy({
  bundleName: 'img',
  inputs: {
    '': './assets/imgs/*',
  },
});

export default [css, loginCss, ...allJs, imgs];

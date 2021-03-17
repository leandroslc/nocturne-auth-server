import config from './config/config';
import sass from './config/sass-build';
import typescript from './config/typescript-build';

const css = sass({
  bundleName: config.projectName,
  input: './scss/index.scss',
  watch: './scss/**/*.scss',
});

const js = typescript({
  bundleName: config.projectName,
  input: './src/index.ts',
  watch: './src/**/*.ts',
});

const bootstrapCSS = sass({
  bundleName: 'bootstrap',
  input: './vendor/bootstrap/scss/index.scss',
  watch: false,
});

export default [
  css,
  js,
  bootstrapCSS,
];
